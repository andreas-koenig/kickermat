namespace Logging
{
    using System;
    using System.IO;
    using System.IO.Ports;
    using System.Windows.Forms;

    /// <summary>
    /// UserControl, welches Log-Nachrichten von einer seriellen Schnittstelle ausliest und anzeigt
    /// </summary>
    public partial class SerialLogReader : UserControl
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="SerialLogReader"/> class.
        /// </summary>
        public SerialLogReader()
        {
            this.InitializeComponent();
            this.InitComPortList();
            this.InitBaudRateList();
            this.InitDataBitList();
            this.InitParityList();
            this.InitStopBitList();
        }        

        /// <summary>
        /// Inits the stop bit list.
        /// </summary>
        private void InitStopBitList()
        {
            this.comboBoxStopBits.Items.Clear();
            foreach (StopBits stopBitsConfiguration in Enum.GetValues(typeof(StopBits)))
            {
                this.comboBoxStopBits.Items.Add(stopBitsConfiguration);
            }

            this.comboBoxStopBits.SelectedItem = this.SerialPortLogging.StopBits;
        }

        /// <summary>
        /// Inits the parity list.
        /// </summary>
        private void InitParityList()
        {
            this.comboBoxParity.Items.Clear();
            foreach (Parity parityConfiguration in Enum.GetValues(typeof(Parity)))
            {
                this.comboBoxParity.Items.Add(parityConfiguration);
            }

            this.comboBoxParity.SelectedItem = this.SerialPortLogging.Parity;
        }

        /// <summary>
        /// Inits the data bit list.
        /// </summary>
        private void InitDataBitList()
        {
            int[] dataBitsConfigurations = new[] { 7, 8 };
            this.comboBoxDataBits.Items.Clear();
            foreach (int dataBitsConfiguration in dataBitsConfigurations)
            {
                this.comboBoxDataBits.Items.Add(dataBitsConfiguration);
            }

            if (this.comboBoxDataBits.Items.Contains(this.SerialPortLogging.DataBits))
            {
                this.comboBoxDataBits.SelectedItem = this.SerialPortLogging.DataBits;
            }
            else
            {
                this.comboBoxDataBits.SelectedItem = 8;
            }
        }

        /// <summary>
        /// Inits the baud rate list.
        /// </summary>
        private void InitBaudRateList()
        {
            int[] baudrates = new[] { 300, 600, 1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
            this.comboBoxBaudrate.Items.Clear();
            foreach (int baudrate in baudrates)
            {
                this.comboBoxBaudrate.Items.Add(baudrate);
            }

            if (this.comboBoxBaudrate.Items.Contains(this.SerialPortLogging.BaudRate))
            {
                this.comboBoxBaudrate.SelectedItem = this.SerialPortLogging.BaudRate;
            }
            else
            {
                this.comboBoxDataBits.SelectedItem = 9600;
            }
        }

        /// <summary>
        /// Inits the COM port list.
        /// </summary>
        private void InitComPortList()
        {
            this.comboBoxSerialPorts.Items.Clear();
            this.comboBoxSerialPorts.Items.AddRange(SerialPort.GetPortNames());
            if (this.comboBoxSerialPorts.Items.Contains(this.SerialPortLogging.PortName))
            {
                this.comboBoxSerialPorts.SelectedItem = this.SerialPortLogging.PortName;
            }
            else
            {
                if (this.comboBoxSerialPorts.Items.Count > 0)
                {
                    this.comboBoxSerialPorts.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxLoggingEnabled control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxLoggingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBoxLoggingEnabled.Checked == true)
            {
                this.StartLogging();
            }
            else
            {
                this.StopLogging();
            }
        }

        /// <summary>
        /// Stops the logging.
        /// </summary>
        private void StopLogging()
        {
            if (this.SerialPortLogging.IsOpen)
            {
                this.SerialPortLogging.Close();
            }

            this.SetConfigurationControlEnabedState(true);
        }

        /// <summary>
        /// Starts the logging.
        /// </summary>
        private void StartLogging()
        {
            this.SerialPortLogging.PortName = this.comboBoxSerialPorts.SelectedItem.ToString();
            this.SerialPortLogging.Parity = (Parity)this.comboBoxParity.SelectedItem;
            this.SerialPortLogging.BaudRate = (int)this.comboBoxBaudrate.SelectedItem;
            this.SerialPortLogging.DataBits = (int)this.comboBoxDataBits.SelectedItem;
            this.SerialPortLogging.StopBits = (StopBits)this.comboBoxStopBits.SelectedItem;
            this.SerialPortLogging.ReadTimeout = 200;
            this.SerialPortLogging.Open();
            this.SetConfigurationControlEnabedState(false);
        }

        /// <summary>
        /// Sets the state of the configuration control enabed.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void SetConfigurationControlEnabedState(bool enabled)
        {
            this.comboBoxSerialPorts.Enabled = enabled;
            this.comboBoxBaudrate.Enabled = enabled;
            this.comboBoxDataBits.Enabled = enabled;
            this.comboBoxParity.Enabled = enabled;
            this.comboBoxStopBits.Enabled = enabled;
        }

        /// <summary>
        /// Handles the DataReceived event of the this.serialPort1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.IO.Ports.SerialDataReceivedEventArgs"/> instance containing the event data.</param>
        private void SerialPortLogging_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                string message;
                try
                {
                    message = this.SerialPortLogging.ReadLine();
                }
                catch (TimeoutException ex)
                {
                    message = ex.Message;
                }

                if (this.listBoxLogging.InvokeRequired)
                {
                    this.listBoxLogging.Invoke(new Action<string>(this.AddMessageToListBox), message);
                }
                else
                {
                    this.AddMessageToListBox(message);
                }
            }
        }

        /// <summary>
        /// Adds the message to list box.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AddMessageToListBox(string message)
        {
            this.listBoxLogging.Items.Add(DateTime.Now.ToString());
            this.listBoxLogging.Items.Add(message);
            this.listBoxLogging.SelectedIndex = this.listBoxLogging.Items.Count - 1;
        }

        /// <summary>
        /// Handles the Click event of the clearToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            this.listBoxLogging.Items.Clear();
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog logFileDialog = new SaveFileDialog();
            logFileDialog.Filter = "*.log|Log-File";
            if (logFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter logWriter = new StreamWriter(logFileDialog.FileName);
                foreach (object entry in this.listBoxLogging.Items)
                {
                    logWriter.WriteLine(entry);
                }

                logWriter.Close();
            }
        }
    }
}