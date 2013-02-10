using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Anime_Quiz.Properties;
using WMPLib;

namespace Anime_Quiz.DataModel
{
    class MusicPlayer
    {
        FlowLayoutPanel audioButtons;
        WindowsMediaPlayer WMP;
        TrackBar progressBar;
        Timer timer;
        Timer playTimer;

        String filename;
        public MusicPlayer(String filename, Control parentControl)
        {
            this.filename = filename;

            audioButtons = new FlowLayoutPanel();
            audioButtons.Size = new Size(200, 100);
            audioButtons.Location = new Point(12, 200);
            parentControl.Controls.Add(audioButtons);

            addWindowsMediaPlayer();
            addProgressBar();
            addTimer();
            addMediaButtons();
        }

        #region Controls
        private void addWindowsMediaPlayer()
        {
            WMP = new WindowsMediaPlayer();
            WMP.MediaError += new _WMPOCXEvents_MediaErrorEventHandler(WMP_MediaError);
            //WMP.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(WMP_PlayStateChange);
            WMP.settings.autoStart = Settings.Default.autostartSong;
            WMP.URL = filename;
            WMP.settings.volume = Settings.Default.defaultVolume;
            //Small trick to allow WMP.currentMedia.duration to work
            WMP.currentMedia.name = WMP.currentMedia.name;
        }
        private void addProgressBar()
        {
            progressBar = new TrackBar();
            progressBar.Width = 200;
            progressBar.Minimum = 0;
            progressBar.Maximum = (int)WMP.currentMedia.duration;
            progressBar.TickStyle = TickStyle.None; //Do not display ticks for smother scrolling
            progressBar.Scroll += new EventHandler(progressBar_Scroll);
        }
        private void addTimer()
        {
            //Add a timer to refresh the progressBar position every second
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            //Start the timers
            if (Settings.Default.autostartSong)
            {
                timer.Start();
                createPlayTimer();
            }
        }
        private void addMediaButtons()
        {
            Button playBtn = new Button();
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Stream playImgStream = currentAssembly.GetManifestResourceStream("Anime_Quiz.Data.play.png");
            playBtn.BackgroundImage = Image.FromStream(playImgStream);
            playBtn.Size = new Size(48, 48);
            //playBtn.Enabled = false;
            playBtn.Click += new EventHandler(playBtn_Click);
            Button pauseBtn = new Button();
            Stream pauseImgStream = currentAssembly.GetManifestResourceStream("Anime_Quiz.Data.pause.png");
            pauseBtn.BackgroundImage = Image.FromStream(pauseImgStream);
            pauseBtn.Size = new Size(48, 48);
            pauseBtn.Click += new EventHandler(pauseBtn_Click);
            Button stopBtn = new Button();
            Stream stopImgStream = currentAssembly.GetManifestResourceStream("Anime_Quiz.Data.stop.png");
            stopBtn.BackgroundImage = Image.FromStream(stopImgStream);
            stopBtn.Size = new Size(48, 48);
            stopBtn.Click += new EventHandler(stopBtn_Click);
            audioButtons.Controls.Add(progressBar);
            audioButtons.Controls.Add(playBtn);
            audioButtons.Controls.Add(pauseBtn);
            audioButtons.Controls.Add(stopBtn);
        }
        #endregion

        #region Music Buttons
        void playBtn_Click(object sender, EventArgs e)
        {
            WMP.controls.play();
            timer.Start();
            createPlayTimer();
        }
        void pauseBtn_Click(object sender, EventArgs e)
        {
            if (WMP.playState.Equals(WMPPlayState.wmppsPaused)) WMP.controls.play();
            else if (WMP.playState.Equals(WMPPlayState.wmppsPlaying))
            {
                WMP.controls.pause();
                playTimer.Dispose();
            }
        }
        void stopBtn_Click(object sender, EventArgs e)
        {
            if (WMP.playState.Equals(WMPPlayState.wmppsPaused) || WMP.playState.Equals(WMPPlayState.wmppsPlaying))
            {
                WMP.controls.stop();
                playTimer.Dispose();
            }
        }
        void progressBar_Scroll(object sender, EventArgs e)
        {
            WMP.controls.currentPosition = Convert.ToDouble(progressBar.Value);
        }

        void WMP_MediaError(object pMediaObject)
        {
            MessageBox.Show("Error: cannot open or play music file");
        }
        #endregion

        #region Timers
        void timer_Tick(object sender, EventArgs e)
        {
            progressBar.Value = (int)WMP.controls.currentPosition;
        }
        void createPlayTimer()
        {
            if (WMP.controls.currentPosition <= Settings.Default.songDuration)
            {
                playTimer = new Timer();
                playTimer.Interval = 1000 * (Settings.Default.songDuration - (int)WMP.controls.currentPosition);
                playTimer.Tick += new EventHandler(playTimer_Tick);
                playTimer.Start();
            }
        }
        void playTimer_Tick(object sender, EventArgs e)
        {
            WMP.controls.pause();
            playTimer.Dispose();
        }
        #endregion

        public void dispose(Control parentControl)
        {
            WMP.close();
            parentControl.Controls.Remove(audioButtons);
        }
    }
}
