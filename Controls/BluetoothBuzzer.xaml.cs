using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;

namespace Anime_Quiz_3.Controls
{
    /// <summary>
    /// Interaction logic for BluetoothBuzzer.xaml
    /// </summary>
    public partial class BluetoothBuzzer : UserControl, IDisposable
    {
        string _comPort;
        int _baudRate = 9600;
        int _dataBits = 8;
        Parity _parity = Parity.None;
        StopBits _stopBits = StopBits.One;
        
        int teamId;
        SerialPort serialPort;

        public BluetoothBuzzer(string comPort, int teamId)
        {
            InitializeComponent();
            this._comPort = comPort;
            this.teamId = teamId;

            this.Visibility = Visibility.Collapsed;
            BuzzerSetUp();
        }

        #region Constructors
        public int baudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }
        public int dataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }
        public Parity parity
        {
            get { return _parity; }
            set { _parity = value; }
        }
        public StopBits stopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }
        #endregion

        /// <summary>
        ///     Send the teamId to the buzzer and get the response.
        ///     This doubles as checking if the buzzer is properly paired.
        /// </summary>
        private void BuzzerSetUp()
        {
            serialPort = new SerialPort(_comPort, _baudRate, _parity, _dataBits, _stopBits);
            teamName.Content = (from team in App.teams where team.TeamId.Equals(teamId) select team).Single().Name;
            // ugly hack
            try
            {
                serialPort.Open();
                serialPort.WriteLine(teamId.ToString());

                string response = serialPort.ReadLine();
                //SoundMessageBox.Show(response, Properties.Resources.W_hello);
                serialPort.Close();
            }
            catch (Exception crap)
            {
                BuzzerSetUp();
            }
        }

        /// <summary>
        ///     Get the team name registered to the button
        /// </summary>
        /// <returns>The team name</returns>
        public String getTeamName()
        {
            return teamName.Content.ToString();
        }

        /// <summary>
        ///     Hide the buzzer label.
        /// </summary>
        public void BuzzerReset()
        {
            this.Visibility = Visibility.Collapsed;
            //serialPort.Close();
        }

        /// <summary>
        ///     Closes the serial connection and hide the buzzer label.
        /// </summary>
        public void BuzzerClose()
        {
            BuzzerReset();
            serialPort.Close();
        }

        /// <summary>
        ///     Open the serial connection and listen to buzz-in.
        /// </summary>
        public void BuzzerStandby()
        {
            if (serialPort != null && serialPort.IsOpen)
                return;
                //serialPort.Close();
            serialPort = new SerialPort(_comPort, _baudRate, _parity, _dataBits, _stopBits);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            try
            {
                serialPort.Open();
            }
            catch (Exception crap)
            {
                BuzzerStandby();
            }
        }
        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OnBuzzerPressed(EventArgs.Empty);
            //this.Visibility = Visibility.Visible;
            /*
            this.Dispatcher.Invoke((Action)(() =>
            {
                Visibility = Visibility.Visible;
            }));*/
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                serialPort.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
        public event EventHandler BuzzerPressed;
        protected virtual void OnBuzzerPressed(EventArgs e)
        {
            EventHandler handler = BuzzerPressed;
            if (handler != null)
                handler(this, e);
        }
    }
}
