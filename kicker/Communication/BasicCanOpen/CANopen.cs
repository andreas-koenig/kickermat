#if CAN_ENABLED
namespace Communication.BasicCanOpen
{
    using System;
    using System.Threading;
    using BasicCan;
    using GlobalDataTypes;
    using Ixxat.Vci3.Bal.Can;

    /// <summary>
    /// Implementation of CANopen functionality
    /// </summary>
    public class CanOpen : IDisposable
    {
        /// <summary>
        /// The timeout for CANopren responses in milliseconds
        /// </summary>
        private const int ResponseTimeout = 200;                       

        /// <summary>
        /// Number of entries in a read buffer
        /// </summary>
        private const uint ReadBufferLength = 10;

        /// <summary>
        /// Instance for sending/receiving CAN messages
        /// </summary>
        private readonly Can usedCanInterface = new Can();

        /// <summary>
        /// Lesepuffer für CANopen NMT-Leseanforderungen. Beim Lesen des NMT-Status wird
        /// die Knoten-ID hier gespeichert und danach eine Leseanforderung verschickt.
        /// Sobald die Antwort auf die Leseanforderung eintrifft, wird der dazugehörige
        /// Knoten-Status eingetragen
        /// </summary>
        private volatile NmtReadBufferType[] nmtReadBuffer = new NmtReadBufferType[ReadBufferLength];

        /// <summary>
        /// Lesepuffer für CANopen SDO-Leseanforderungen. Beim Lesen eines SDO-Eintrages
        /// werden die Knoten-ID, index und Subindex hier gespeichert und danach eine
        /// Leseanforderung verschickt. Sobald die Antwort auf die Leseanforderung
        /// eintrifft, wird der dazugehörige gelesene Wert eingetragen
        /// </summary>
        private volatile SdoBufferType[] sdoReadBuffer = new SdoBufferType[ReadBufferLength];
        
        /// <summary>
        /// Lesepuffer für CANopen SDO-Leseanforderungen. Beim Lesen eines SDO-Eintrages
        /// werden die Knoten-ID, index und Subindex hier gespeichert und danach eine
        /// Leseanforderung verschickt. Sobald die Antwort auf die Leseanforderung
        /// eintrifft, wird der dazugehörige gelesene Wert eingetragen
        /// </summary>
        private volatile SdoBufferType[] sdoWriteBuffer = new SdoBufferType[ReadBufferLength];        

        /// <summary>
        /// Processes the emcy message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void ProcessEmcyMessage(CanMessage message)
        {
            // Die Motor ID sind immer die 7 niederwertigsten Bits der message-ID
            byte nodeNumber = (byte)(message.Identifier & 0x007F);

            // Variablen, die bei EMCY (Emergency-Nachrichten) benötigt werden
            ushort emcyErrorCode = 0;
            byte emcyErrorRegister = 0;
            ushort emcyObjectErrorCode = 0;
            ushort emcyObjectIndex = 0;
            byte emcyObjectSubIndex = 0;
            emcyErrorCode = message[0];
            emcyErrorCode |= (ushort)(message[1] << 8);
            emcyErrorRegister = message[2];
            emcyObjectErrorCode = message[3];
            emcyObjectErrorCode |= (ushort)(message[4] << 8);
            emcyObjectIndex = message[5];
            emcyObjectIndex |= (ushort)(message[6] << 8);
            emcyObjectSubIndex = message[7];
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialisiert das CANopen-Modul sowie alle darin enthaltenen Read-Puffer
        /// </summary>
        public void Init()
        {
            uint readBufferEntryNumber;
            for (readBufferEntryNumber = 0; readBufferEntryNumber < ReadBufferLength; readBufferEntryNumber++)
            {
                this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore = null;
                this.nmtReadBuffer[readBufferEntryNumber].EventSemaphore = null;
            }

            return;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }        

        /// <summary>
        /// Sendet einen NMT-Befehl an einen CANopen-Knoten
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="command">The command.</param>
        public void SendNmtCommand(byte nodeNumber, NmtService command)
        {
            CanMessage nmtMessage = new CanMessage();

            nmtMessage.Identifier = 0x00;
            nmtMessage[0] = (byte)command;
            nmtMessage[1] = nodeNumber;
            this.usedCanInterface.Send(nmtMessage);
        }

        /// <summary>
        /// Sendet einen NMT-RTR-Befehl an einen CANopen-Knoten
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="readValue">The read value.</param>
        /// <returns>
        ///     <c>Ok</c> if request has been sent successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SendNmtRequest(byte nodeNumber, out byte readValue)
        {
            CanMessage nmtMessage = new CanMessage();
            sbyte readBufferEntryNumber;
            sbyte nextFreeReadBufferEntryNumber = -1;
            ReturnType returnValue = ReturnType.Ok;
            checked
            {
                nmtMessage.Identifier = (uint)CanOpenMessageBaseIdentifier.NmtErrorControl + nodeNumber;
            }

            for (readBufferEntryNumber = 0; readBufferEntryNumber < ReadBufferLength; readBufferEntryNumber++)
            {
                // suche nach unbenutzten Eintrag
                if (this.nmtReadBuffer[readBufferEntryNumber].EventSemaphore == null)
                {
                    nextFreeReadBufferEntryNumber = readBufferEntryNumber;
                    break;
                }
            }

            if (nextFreeReadBufferEntryNumber != -1)
            {
                // unbenutzer Eintrag wurde gefunden
                this.nmtReadBuffer[nextFreeReadBufferEntryNumber].NodeId = nodeNumber;
                /* Init Os Semaphore */
                this.nmtReadBuffer[readBufferEntryNumber].EventSemaphore = new AutoResetEvent(false);
                this.usedCanInterface.Send(nmtMessage);

                // wait for semaphore, warten bis antwort ankommt
                if (this.nmtReadBuffer[readBufferEntryNumber].EventSemaphore.WaitOne(ResponseTimeout) == false)
                {
                    throw new CanOpenException(CanOpenError.PendingOnEventFailed);
                }
                else
                {
                    readValue = this.nmtReadBuffer[nextFreeReadBufferEntryNumber].ReceivedValue;
                    returnValue = ReturnType.Ok;
                }

                // delete semaphore
                this.nmtReadBuffer[readBufferEntryNumber].EventSemaphore = null;
            }
            else
            {
                // Error_ReportError(MODULE_ID, ERROR_NMT_READ_BUFFER_FULL, __LINE__);
                readValue = 0xFF;
            }

            return returnValue;
        }

        /// <summary>
        /// Sendet eine PDO1-Nachricht an einen CANopen-Knoten
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="command">The command.</param>
        public void SendPdo1Message(byte nodeNumber, int command)
        {
            CanMessage pdo1Message = new CanMessage();

            checked
            {
                pdo1Message.Identifier = (uint)CanOpenMessageBaseIdentifier.ReceivePdo1 + nodeNumber;
            }

            pdo1Message[0] = (byte)command;
            pdo1Message[1] = (byte)(command >> 8);

            this.usedCanInterface.Send(pdo1Message);
        }

        /// <summary>
        /// Sendet eine PDO2-Nachricht an einen CANopen-Knoten
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="command">The command.</param>
        /// <param name="parameter">The parameter.</param>
        public void SendPdo2Message(byte nodeNumber, int command, int parameter)
        {
            CanMessage pdo2Message = new CanMessage();
            checked
            {
                pdo2Message.Identifier = (uint)CanOpenMessageBaseIdentifier.ReceivePdo2 + nodeNumber;
            }

            pdo2Message[0] = (byte)command;
            pdo2Message[1] = (byte)(command >> 8);
            pdo2Message[2] = (byte)parameter;
            pdo2Message[3] = (byte)(parameter >> 8);
            pdo2Message[4] = (byte)(parameter >> 16);
            pdo2Message[5] = (byte)(parameter >> 24);

            this.usedCanInterface.Send(pdo2Message);
        }

        /// <summary>
        /// Sendet eine PDO2-Nachricht mit einer Datenlänge von 5 Bytes an einen CANopen-Knoten
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="command">The command.</param>
        /// <param name="parameter">The parameter.</param>
        public void SendPdo2Message5Bytes(byte nodeNumber, byte command, int parameter)
        {
            CanMessage pdo2Message = new CanMessage();

            checked
            {
                pdo2Message.Identifier = (uint)CanOpenMessageBaseIdentifier.ReceivePdo2 + nodeNumber;
            }

            pdo2Message[0] = command;
            pdo2Message[1] = (byte)parameter;
            pdo2Message[2] = (byte)(parameter >> 8);
            pdo2Message[3] = (byte)(parameter >> 16);
            pdo2Message[4] = (byte)(parameter >> 24);

            this.usedCanInterface.Send(pdo2Message);
        }

        /// <summary>
        /// Schickt eine SDO-Leseanforderung für den gewählten index/subIndex an den
        /// entsprechenene CANopen-Knoten und wartet auf die Antwort.
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="readValue">The read value.</param>
        /// <returns>
        ///     <c>Ok</c> if reading has been successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SdoRead(byte nodeNumber, int index, byte subIndex, out int readValue)
        {
            CanMessage readRequest = new CanMessage();
            sbyte readBufferEntryNumber;
            sbyte nextFreeReadBufferEntryNumber = -1;
            ReturnType returnValue = ReturnType.Ok;
            checked
            {
                readRequest.Identifier = (uint)CanOpenMessageBaseIdentifier.ReceiveSdo + nodeNumber;
            }

            // CCD ist immer 0x40 bei Read-Nachricht
            readRequest[0] = (byte)CanOpenCommandCode.ReadRequest;
            
            // index LSB
            readRequest[1] = (byte)index;
            
            // index MSB
            readRequest[2] = (byte)(index >> 8);
            
            // subIndex
            readRequest[3] = subIndex;
            
            // parameter LSB
            readRequest[4] = 0xFF;
            
            // Parameterteil ist bei Read-Nachricht nicht definiert
            readRequest[5] = 0xFF;
            
            // Parameterteil ist bei Read-Nachricht nicht definiert
            readRequest[6] = 0xFF;
            
            // parameter MSB
            readRequest[7] = 0xFF;

            for (readBufferEntryNumber = 0; readBufferEntryNumber < ReadBufferLength; readBufferEntryNumber++)
            {
                // suche nach unbenutzten Eintrag
                if (this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore == null)
                {
                    nextFreeReadBufferEntryNumber = readBufferEntryNumber;
                    break;
                }
            }

            if (nextFreeReadBufferEntryNumber != -1)
            {
                // unbenutzer Eintrag wurde gefunden
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].Index = index;
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].NodeId = nodeNumber;
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].SubIndex = subIndex;
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].EventSemaphore = new AutoResetEvent(false);
                this.usedCanInterface.Send(readRequest);

                // wait for semaphore, warten bis antwort ankommt
                if (this.sdoReadBuffer[nextFreeReadBufferEntryNumber].EventSemaphore.WaitOne(ResponseTimeout) == false)
                {
                    throw new CanOpenException(CanOpenError.PendingOnEventFailed);
                }
                else
                {
                    readValue = this.sdoReadBuffer[nextFreeReadBufferEntryNumber].ReceivedValue;
                    returnValue = ReturnType.Ok;
                }

                // delete semaphore
                this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore = null;
            }
            else
            {
                throw new CanOpenException(CanOpenError.SdoReadBufferFull);
            }

            return returnValue;
        }

        /// <summary>
        /// Schickt eine SDO-Leseanforderung für den gewählten index/subIndex an den
        /// entsprechenene CANopen-Knoten und wartet auf die Antwort.
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="readValue">The read value.</param>
        /// <returns>
        ///     <c>Ok</c> if reading has been successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SdoRead(byte nodeNumber, int index, byte subIndex, out long readValue)
        {
            int readValueInt32;
            ReturnType returnValue = this.SdoRead(nodeNumber, index, subIndex, out readValueInt32);
            readValue = (uint)readValueInt32;
            return returnValue;
        }

        /// <summary>
        /// Schickt eine SDO-Leseanforderung für den gewählten index/subIndex an den
        /// entsprechenene CANopen-Knoten und wartet auf die Antwort.
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="valuePointer">The value pointer.</param>
        /// <returns><c>Ok</c> if reading has been successfully, else <c>NotOk</c></returns>
        public ReturnType SdoReadExtended(byte nodeNumber, int index, byte subIndex, ref long valuePointer)
        {
            ReturnType returnValue = ReturnType.Ok;
            CanMessage readRequest = new CanMessage();
            sbyte readBufferEntryNumber;
            sbyte nextFreeReadBufferEntryNumber = -1;
            checked
            {
                readRequest.Identifier = (uint)CanOpenMessageBaseIdentifier.ReceiveSdo + nodeNumber;
            }

            // CCD ist immer 0x40 bei Read-Nachricht
            readRequest[0] = (byte)CanOpenCommandCode.ReadRequest;

            // index LSB   
            readRequest[1] = (byte)index;

            // index MSB
            readRequest[2] = (byte)(index >> 8);

            // subIndex
            readRequest[3] = subIndex;

            // parameter LSB
            readRequest[4] = 0xFF;

            // Parameterteil ist bei Read-Nachricht nicht definiert
            readRequest[5] = 0xFF;

            // Parameterteil ist bei Read-Nachricht nicht definiert
            readRequest[6] = 0xFF;

            // parameter MSB
            readRequest[7] = 0xFF;

            for (readBufferEntryNumber = 0; readBufferEntryNumber < ReadBufferLength; readBufferEntryNumber++)
            {
                // suche nach unbenutzten Eintrag
                if (this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore == null)
                {
                    nextFreeReadBufferEntryNumber = readBufferEntryNumber;
                    break;
                }
            }

            if (nextFreeReadBufferEntryNumber != -1)
            {
                // unbenutzer Eintrag wurde gefunden
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].Index = index;
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].NodeId = nodeNumber;
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].SubIndex = subIndex;
                this.sdoReadBuffer[nextFreeReadBufferEntryNumber].EventSemaphore = new AutoResetEvent(false);
                this.usedCanInterface.Send(readRequest);
                
                // wait for semaphore, warten bis antwort ankommt
                if (this.sdoReadBuffer[nextFreeReadBufferEntryNumber].EventSemaphore.WaitOne(5) == false)
                {
                    throw new CanOpenException(CanOpenError.PendingOnEventFailed);
                }

                // delete semaphore
                this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore.Close();
                this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore = null;
                valuePointer = (uint)this.sdoReadBuffer[nextFreeReadBufferEntryNumber].ReceivedValue;
            }
            else
            {
                throw new CanOpenException(CanOpenError.SdoReadBufferFull);
            }

            return returnValue;
        }

        /// <summary>
        /// Schickt einen SDO-Schreibbefehl mit angegebenen index/subIndex sowie den
        /// übergebenen Daten an den entsprechenden CANopen-Knoten. Es erfolgt keine
        /// Überprüfung, ob die Daten erfolgreich geschrieben wurden. Für eine Überprüfung
        /// auf korrektes Schreiben muss der Wert nach dem Schreiben ausgelesen werden und
        /// mit dem gesendeten Wert verglichen werden
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="parameterLength">Length of the parameter.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="writeResponse">The write response.</param>
        /// <returns>
        ///     <c>Ok</c> if writing has been successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SdoWrite(byte nodeNumber, byte parameterLength, int index, byte subIndex, long parameter, out int writeResponse)
        {
            CanMessage writeRequest = new CanMessage();
            sbyte writeBufferEntryNumber;
            sbyte nextFreeWriteBufferEntryNumber = -1;

            ReturnType returnValue = ReturnType.Ok;
            checked
            {
                writeRequest.Identifier = (uint)(CanOpenMessageBaseIdentifier.ReceiveSdo + nodeNumber);
            }

            switch (parameterLength)
            {
                    // CCD in Abhängigkeit der Länge der parameter
                case 1:
                    writeRequest[0] = (byte)CanOpenCommandCode.WriteRequest1Byte;
                    break;
                case 2:
                    writeRequest[0] = (byte)CanOpenCommandCode.WriteRequest2Byte;
                    break;
                case 3:
                    writeRequest[0] = (byte)CanOpenCommandCode.WriteRequest3Byte;
                    break;
                case 4:
                    writeRequest[0] = (byte)CanOpenCommandCode.WriteRequest4Byte;
                    break;
            }

            // index LSB
            writeRequest[1] = (byte)index;

            // index MSB
            writeRequest[2] = (byte)(index >> 8);

            // subIndex
            writeRequest[3] = subIndex;

            // parameter LSB
            writeRequest[4] = (byte)parameter;
            writeRequest[5] = (byte)(parameter >> 8);
            writeRequest[6] = (byte)(parameter >> 16);

            // parameter MSB
            writeRequest[7] = (byte)(parameter >> 24);

            for (writeBufferEntryNumber = 0; writeBufferEntryNumber < ReadBufferLength; writeBufferEntryNumber++)
            {
                // suche nach unbenutzten Eintrag
                if (this.sdoWriteBuffer[writeBufferEntryNumber].EventSemaphore == null)
                {
                    nextFreeWriteBufferEntryNumber = writeBufferEntryNumber;
                    break;
                }
            }

            if (nextFreeWriteBufferEntryNumber != -1)
            {
                // unbenutzer Eintrag wurde gefunden
                this.sdoWriteBuffer[nextFreeWriteBufferEntryNumber].Index = index;
                this.sdoWriteBuffer[nextFreeWriteBufferEntryNumber].NodeId = nodeNumber;
                this.sdoWriteBuffer[nextFreeWriteBufferEntryNumber].SubIndex = subIndex;
                this.sdoWriteBuffer[nextFreeWriteBufferEntryNumber].EventSemaphore = new AutoResetEvent(false);
                this.usedCanInterface.Send(writeRequest);
                
                // wait for semaphore, warten bis antwort ankommt
                if (this.sdoWriteBuffer[nextFreeWriteBufferEntryNumber].EventSemaphore.WaitOne(ResponseTimeout) == false)
                {
                    throw new CanOpenException(CanOpenError.PendingOnEventFailed);
                }
                else
                {
                    writeResponse = this.sdoReadBuffer[nextFreeWriteBufferEntryNumber].ReceivedValue;
                    returnValue = ReturnType.Ok;
                }

                // delete semaphore
                this.sdoWriteBuffer[writeBufferEntryNumber].EventSemaphore = null;
            }
            else
            {
                throw new CanOpenException(CanOpenError.SdoReadBufferFull);
            }

            return returnValue;
        }

        /// <summary>
        /// Schickt einen SDO-Schreibbefehl mit angegebenen index/subIndex sowie den
        /// übergebenen Daten mit der Länge 1 byte an den entsprechenden CANopen-Knoten.
        /// Zum Versand wird die CANopenSDO_Write-Funktion verwendet
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="writeResponse">The write response.</param>
        /// <returns>
        ///     <c>Ok</c> if writing has been successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SdoWrite1Bytes(byte nodeNumber, int index, byte subIndex, byte parameter, out int writeResponse)
        {
            return this.SdoWrite(nodeNumber, 1, index, subIndex, parameter, out writeResponse);
        }

        /// <summary>
        /// Schickt einen SDO-Schreibbefehl mit angegebenen index/subIndex sowie den
        /// übergebenen Daten mit der Länge 2 byte an den entsprechenden CANopen-Knoten.
        /// Zum Versand wird die CANopenSDO_Write-Funktion verwendet
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="writeResponse">The write response.</param>
        /// <returns>
        ///     <c>Ok</c> if writing has been successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SdoWrite2Bytes(byte nodeNumber, int index, byte subIndex, int parameter, out int writeResponse)
        {
            return this.SdoWrite(nodeNumber, 2, index, subIndex, parameter, out writeResponse);
        }

        /// <summary>
        /// Schickt einen SDO-Schreibbefehl mit angegebenen index/subIndex sowie den
        /// übergebenen Daten mit der Länge 4 byte an den entsprechenden CANopen-Knoten.
        /// Zum Versand wird die CANopenSDO_Write-Funktion verwendet
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">index of the sub.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="writeResponse">The write response.</param>
        /// <returns>
        ///     <c>Ok</c> if writing has been successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SdoWrite4Bytes(byte nodeNumber, int index, byte subIndex, long parameter, out int writeResponse)
        {
            return this.SdoWrite(nodeNumber, 4, index, subIndex, parameter, out writeResponse);
        }

        /// <summary>
        /// Writes a SDO object with the length of 4 bytes to the motor
        /// </summary>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="index">The index.</param>
        /// <param name="subIndex">The subindex.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="writeResponse">The write response.</param>
        /// <returns><c>Ok</c> if writing has been successfully, else <c>NotOk</c></returns>
        public ReturnType SdoWrite4Bytes(byte nodeNumber, int index, byte subIndex, int parameter, out int writeResponse)
        {
            return this.SdoWrite(nodeNumber, 4, index, subIndex, (uint)parameter, out writeResponse);
        }

        /// <summary>
        /// Processes the received message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessReceivedMessage(CanMessage message)
        {
            // Variablen, die bei jeder Nachricht verwendet werden
            uint messageBaseID = 0;

            // Variablen, die bei PDO1 (Process Data Object 1) benötigt werden
            ushort pdo1StatusWord = 0;
            
            // Variablen, die bei PDO2 (Process Data Object 1) benötigt werden
            ushort pdo2StatusWord = 0;
            int pdo2PositionAcutalValue = 0;

            // Der Rest is die Basis-ID der Nachricht
            messageBaseID = message.Identifier & 0xFF80;
            switch (messageBaseID)
            {
                case (uint)CanOpenMessageBaseIdentifier.Emcy:
                    ProcessEmcyMessage(message);
                    break;
                case (uint)CanOpenMessageBaseIdentifier.NmtErrorControl:
                    this.ProcessNmtMessage(message);
                    break;
                case (uint)CanOpenMessageBaseIdentifier.TransmitSdo:
                    this.ProcessTransmitSdoMessage(message);
                    break;
                case (uint)CanOpenMessageBaseIdentifier.TransmitPdo1:
                    pdo1StatusWord = message[0];
                    pdo1StatusWord |= (ushort)(message[1] << 8);
                    break;
                case (uint)CanOpenMessageBaseIdentifier.TransmitPdo2:
                    pdo2StatusWord = message[0];
                    pdo2StatusWord |= (ushort)(message[1] << 8);
                    pdo2PositionAcutalValue |= message[2];
                    pdo2PositionAcutalValue |= message[3] << 8;
                    
                    // Alle Werte, die ab Bitposition 16 erscheinen, müssen in uint umgewandelt werden, weil sie sonst beim shiften abgeschnitten werden
                    pdo2PositionAcutalValue |= ((int)message[4]) << 16;
                    pdo2PositionAcutalValue |= ((int)message[5]) << 24;
                    break;
                case (uint)CanOpenMessageBaseIdentifier.TransmitPdo3:
                    // Bisher noch nicht verwendet
                    break;
                default:
                    throw new CanOpenException(CanOpenError.UnknownMessageIdentifier);
            }
        }

        /// <summary>
        /// Processes the NMT message.
        /// </summary>
        /// <param name="nessage">The nessage.</param>
        public void ProcessNmtMessage(CanMessage nessage)
        {
            // Variablen, die bei NMT (Network Management) benötigt werden
            byte nmtStatusByte = 0;
            
            // Buffer access variablen
            sbyte readBufferEntryNumber;
            sbyte usedReadBufferEntryNumber = -1;
            
            // Die Motor ID sind immer die 7 niederwertigsten Bits der message-ID
            byte nodeNumber = (byte)(nessage.Identifier & 0x007F);

            // Das oberste Bit des Status byte (Bit 7) ist nur ein alternierendes Bit 
            // und wird deswegen vor dem Speichern ausmaskiert
            nmtStatusByte = (byte)(nessage[0] & 0x7F);

            for (readBufferEntryNumber = 0; readBufferEntryNumber < ReadBufferLength; readBufferEntryNumber++)
            {
                // suche nach verwendetem eintrage
                if ((this.nmtReadBuffer[readBufferEntryNumber].EventSemaphore != null) &&
                    (this.nmtReadBuffer[readBufferEntryNumber].NodeId == nodeNumber))
                {
                    usedReadBufferEntryNumber = readBufferEntryNumber;
                    break;
                }
            }

            if (usedReadBufferEntryNumber != -1)
            {
                this.nmtReadBuffer[usedReadBufferEntryNumber].ReceivedValue = nmtStatusByte;
                
                // release semaphore
                this.nmtReadBuffer[usedReadBufferEntryNumber].EventSemaphore.Reset();
            }
        }

        /// <summary>
        /// Sets the write response.
        /// </summary>
        /// <param name="sdoIndex">Index of the sdo.</param>
        /// <param name="sdoSubIndex">Index of the sdo sub.</param>
        /// <param name="nodeNumber">The node number.</param>
        /// <param name="sdoData">The sdo data.</param>
        public void SetWriteResponse(int sdoIndex, byte sdoSubIndex, byte nodeNumber, int sdoData)
        {
            // Buffer access variablen
            sbyte writeBufferEntryNumber;
            sbyte usedWriteBufferEntryNumber = -1;
            for (writeBufferEntryNumber = 0; writeBufferEntryNumber < ReadBufferLength; writeBufferEntryNumber++)
            {
                // suche nach verwendetem eintrage
                if ((this.sdoWriteBuffer[writeBufferEntryNumber].EventSemaphore != null) &&
                    (this.sdoWriteBuffer[writeBufferEntryNumber].Index == sdoIndex) &&
                    (this.sdoWriteBuffer[writeBufferEntryNumber].NodeId == nodeNumber) &&
                    (this.sdoWriteBuffer[writeBufferEntryNumber].SubIndex == sdoSubIndex))
                {
                    usedWriteBufferEntryNumber = writeBufferEntryNumber;
                    break;
                }
            }

            if (usedWriteBufferEntryNumber != -1)
            {
                this.sdoWriteBuffer[usedWriteBufferEntryNumber].ReceivedValue = sdoData;
                this.sdoWriteBuffer[usedWriteBufferEntryNumber].EventSemaphore.Reset();
            }
        }

        /// <summary>
        /// Verarbeitet eine empfangene CANopen SDO-Nachricht und speichert die enthaltenen
        /// Daten im SDO-Lesepuffer
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessTransmitSdoMessage(CanMessage message)
        {
            // Variablen, die bei SDO (Service Data Object) benötigt werden
            byte sdoCommandCode = 0;
            ushort sdoIndex = 0;
            byte sdoSubIndex = 0;
            int sdoData = 0;
            
            // Buffer access variablen
            sbyte readBufferEntryNumber;
            sbyte usedReadBufferEntryNumber = -1;

            // Die Motor ID sind immer die 7 niederwertigsten Bits der message-ID
            byte nodeNumber = (byte)(message.Identifier & 0x007F);

            sdoCommandCode = message[0];
            sdoIndex = message[1];
            sdoIndex |= (ushort)(message[2] << 8);
            sdoSubIndex = message[3];
            for (readBufferEntryNumber = 0; readBufferEntryNumber < ReadBufferLength; readBufferEntryNumber++)
            {
                // suche nach verwendetem eintrage
                if ((this.sdoReadBuffer[readBufferEntryNumber].EventSemaphore != null) &&
                    (this.sdoReadBuffer[readBufferEntryNumber].Index == sdoIndex) &&
                    (this.sdoReadBuffer[readBufferEntryNumber].NodeId == nodeNumber) &&
                    (this.sdoReadBuffer[readBufferEntryNumber].SubIndex == sdoSubIndex))
                {
                    usedReadBufferEntryNumber = readBufferEntryNumber;
                    break;
                }
            }

            switch (sdoCommandCode)
            {
                case (int)CanOpenCommandCode.WriteResponse:
                    sdoData = message[4];
                    sdoData |= (int)(message[5] << 8);
                    
                    // Alle Werte, die ab Bitposition 16 erscheinen, müssen in uint umgewandelt werden, weil sie sonst beim shiften abgeschnitten werden
                    sdoData |= ((int)message[6]) << 16;
                    sdoData |= ((int)message[7]) << 24;
                    this.SetWriteResponse(sdoIndex, sdoSubIndex, nodeNumber, sdoData);
                    return;
                case (int)CanOpenCommandCode.ErrorResponse:
                    sdoData = message[4];
                    sdoData |= (int)(message[5] << 8);
                    
                    // Alle Werte, die ab Bitposition 16 erscheinen, müssen in uint umgewandelt werden, weil sie sonst beim shiften abgeschnitten werden
                    sdoData |= ((int)message[6]) << 16;
                    sdoData |= ((int)message[7]) << 24;
                    this.SetWriteResponse(sdoIndex, sdoSubIndex, nodeNumber, sdoData);
                    return;
                case (int)CanOpenCommandCode.ReadResponse1Byte:
                    sdoData = message[4];
                    break;
                case (int)CanOpenCommandCode.ReadResponse2Byte:
                    sdoData = message[4];
                    sdoData |= (int)(message[5] << 8);
                    break;
                case (int)CanOpenCommandCode.ReadResponse3Byte:
                    // Benutze SDO_Data4Byte, da es keinen Datentyp mit 3Byte größe gibt
                    sdoData = message[4];
                    sdoData |= (int)(message[5] << 8);
                    
                    // Alle Werte, die ab Bitposition 16 erscheinen, müssen in uint umgewandelt werden, weil sie sonst beim shiften abgeschnitten werden
                    sdoData |= ((int)message[6]) << 16;
                    break;
                case (int)CanOpenCommandCode.ReadResponse4Byte:
                    sdoData = message[4];
                    sdoData |= (int)(message[5] << 8);
                    
                    // Alle Werte, die ab Bitposition 16 erscheinen, müssen in uint umgewandelt werden, weil sie sonst beim shiften abgeschnitten werden
                    sdoData |= ((int)message[6]) << 16;
                    sdoData |= ((int)message[7]) << 24;
                    break;
            }

            if (usedReadBufferEntryNumber != -1)
            {
                this.sdoReadBuffer[usedReadBufferEntryNumber].ReceivedValue = sdoData;
                this.sdoReadBuffer[usedReadBufferEntryNumber].EventSemaphore.Reset();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
#endif