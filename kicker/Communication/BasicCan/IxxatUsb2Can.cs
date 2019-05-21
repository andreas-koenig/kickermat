#if CAN_ENABLED
namespace Communication.BasicCan
{
    using System;
    using System.Collections;
    using System.Threading;
    using Ixxat.Vci3;
    using Ixxat.Vci3.Bal;
    using Ixxat.Vci3.Bal.Can;

    /// <summary>
    /// Klasse für die Basisfunktionalitäten zum Ansteuern eines IXXAT USB2CAN Adapters
    /// </summary>
    public class IxxatUsb2Can : IDisposable
    {
        /// <summary>
        /// The instance of this class
        /// </summary>
        private static IxxatUsb2Can instance;

        /// <summary>
        /// Gibt an, ob die CAN-Hardware bereits initialisiert wurde
        /// </summary>
        private static bool initialized;

        /// <summary>
        ///   Reference to the used VCI device.
        /// </summary>
        private IVciDevice vciDevice;

        /// <summary>
        ///   Reference to the CAN message communication channel.
        /// </summary>
        private ICanChannel canChannel;

        /// <summary>
        ///   Reference to the message writer of the CAN message channel.
        /// </summary>
        private ICanMessageWriter canMessageWriter;

        /// <summary>
        ///   Reference to the message reader of the CAN message channel.
        /// </summary>
        private ICanMessageReader canMessageReader;

        /// <summary>
        ///   Event that's set if at least one message was received.
        /// </summary>
        private AutoResetEvent messageReceivedEvent;

        /// <summary>
        ///   Reference to the CAN controller.
        /// </summary>
        private ICanControl canControl;

        /// <summary>
        /// Thread zur Abarbeitung empfangener CAN-Nachrichten
        /// </summary>
        private Thread messageReceiveThread;        

        /// <summary>
        /// Prevents a default instance of the IxxatUsb2Can class from being created
        /// </summary>
        private IxxatUsb2Can()
        {
        }

        /// <summary>
        /// Event wenn eine CAN Nachricht empfangen wurde.
        /// </summary>
        public event EventHandler<CanMessageReceivedEventArgs> CanMessageReceived;       

        /// <summary>
        /// Gets the current instance of BaseCan
        /// </summary>
        public static IxxatUsb2Can Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IxxatUsb2Can();
                }

                return instance;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            if (!initialized) 
            {
                this.InitializeCanDevice();
                this.InitializeCanSocket();

                this.messageReceiveThread = new Thread(this.Receiver);
                this.messageReceiveThread.IsBackground = true;
                this.messageReceiveThread.Start();
                initialized = true;
            }
        }        

        /// <summary>
        /// Implementierung von IDisposeable
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }        

        /// <summary>
        /// Sendet die <see cref="CanMessage"/>
        /// </summary>
        /// <param name="canMessage">Die Nachricht die gesendet werden soll.</param>
        public void Send(CanMessage canMessage)
        {
            if (this.canMessageWriter != null)
            {
                this.canMessageWriter.SendMessage(canMessage);
            }
        }

        /// <summary>
        /// Sendet alle <see cref="CanMessage"/> aus dem Array.
        /// </summary>
        /// <param name="canMessages">Das Array der Nachrichten die gesendet werden sollen.</param>
        public void Send(CanMessage[] canMessages)
        {
            this.canMessageWriter.SendMessages(canMessages);
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.messageReceivedEvent.Close();
            }
        }

        /// <summary>
        /// Disposes the vci object.
        /// </summary>
        /// <param name="obj">The object which will be disposed</param>
        private static void DisposeVciObject(object obj)
        {
            if (null != obj)
            {
                IDisposable dispose = obj as IDisposable;
                if (null != dispose)
                {
                    dispose.Dispose();
                }
            }
        }

        /// <summary>
        /// Initialisiert das CAN Device
        /// </summary>
        private void InitializeCanDevice()
        {
            IVciDeviceManager deviceManager = null;
            IVciDeviceList deviceList = null;
            IEnumerator deviceEnum = null;

            try
            {
                deviceManager = VciServer.GetDeviceManager();
                deviceList = deviceManager.GetDeviceList();
                deviceEnum = deviceList.GetEnumerator();

                if (deviceEnum.MoveNext() == true)
                {
                    this.vciDevice = deviceEnum.Current as IVciDevice;
                }
            }
            finally
            {
                DisposeVciObject(deviceManager);
                DisposeVciObject(deviceList);
                DisposeVciObject(deviceEnum);
            }
        }

        /// <summary>
        /// Initilisiert alle CAN Sockets.
        /// </summary>
        private void InitializeCanSocket()
        {
            IBalObject bal = null;
            const byte CanNumber = 0;

            try
            {
                if (this.vciDevice != null)
                {
                    bal = this.vciDevice.OpenBusAccessLayer();
                    this.canChannel = bal.OpenSocket(CanNumber, typeof(ICanChannel)) as ICanChannel;
                    this.canChannel.Initialize(1024, 128, false);

                    this.canMessageReader = this.canChannel.GetMessageReader();
                    this.canMessageReader.Threshold = 1;

                    // Create and assign the event that's set if at least one message was received.
                    this.messageReceivedEvent = new AutoResetEvent(false);
                    this.canMessageReader.AssignEvent(this.messageReceivedEvent);

                    this.canMessageWriter = this.canChannel.GetMessageWriter();
                    this.canMessageWriter.Threshold = 1;

                    this.canChannel.Activate();

                    this.canControl = bal.OpenSocket(CanNumber, typeof(ICanControl)) as ICanControl;
                    this.canControl.InitLine(CanOperatingModes.Standard | CanOperatingModes.ErrFrame, CanBitrate.Cia1000KBit);

                    this.canControl.SetAccFilter(CanFilter.Std, (uint)CanAccCode.All, (uint)CanAccMask.All);
                    this.canControl.StartLine();
                }
            }
            finally
            {
                DisposeVciObject(bal);
            }
        }       

        /// <summary>
        /// Receiver thread wenn Can Nachrichten empfangen werden.
        /// </summary>
        private void Receiver()
        {
            if (this.messageReceivedEvent != null)
            {
                while (true)
                {
                    this.messageReceivedEvent.WaitOne();

                    CanMessage canMessage;
                    this.canMessageReader.ReadMessage(out canMessage);

                    if (this.CanMessageReceived != null)
                    {
                        this.CanMessageReceived(this, new CanMessageReceivedEventArgs(canMessage));
                    }
                }
            }
        }        
    }
}
#endif