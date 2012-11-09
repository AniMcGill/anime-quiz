using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Anime_Quiz.Properties;
using System.Reflection;
using System.IO;
using WMPLib;

namespace Anime_Quiz
{
    public partial class QuestionForm : Form
    {
        public string question { get; set; }
        public string answer { get; set; }
        public string questionType { get; set; }
        public bool answered { get; set; }
        FlowLayoutPanel questionPanel = new FlowLayoutPanel();
        Label questionLabel = new Label();

        //Constructors for Music question
        FlowLayoutPanel audioButtons;
        WindowsMediaPlayer WMP;
        TrackBar progressBar;
        Timer timer;
        Timer playTimer;

        //Constructors for Screenshot question
        PictureBox screenshotBox;
        
        public QuestionForm()
        {
            InitializeComponent();
            closeBtn.Visible = false;
            
            //Load some Question properties from memory
            question = Settings.Default.tempQuestion;
            questionType = Settings.Default.tempType;

            //Initialize the questionPanel
            questionPanel.Location = new Point(12, 50);
            questionPanel.Width = ClientRectangle.Width;
            questionPanel.Height = ClientRectangle.Height - 50;
            Controls.Add(questionPanel);

            questionLabel.Text = "Entry）";
            questionLabel.TextAlign = ContentAlignment.TopLeft;
            questionLabel.AutoSize = true;
            questionLabel.Width = ClientRectangle.Width;
            questionLabel.Height = 150;
            questionLabel.Font = new Font("Microsoft Sans Serif", 50);
            
            questionLabel.Location = new Point(12, 50);
            questionPanel.Controls.Add(questionLabel);

            loadQuestion();
        }
        private void loadQuestion()
        {
            switch (questionType)
            {
                case "Question":
                    questionLabel.Text += question;
                    questionLabel.Width = ClientRectangle.Width - 20;
                    questionLabel.Height = ClientRectangle.Height - 64;
                    break;
                case "Music":
                    //Use a music player to load the music file.
                    audioButtons = new FlowLayoutPanel();
                    audioButtons.Size = new Size(200, 100);
                    audioButtons.Location = new Point(12, 200);
                    questionPanel.Controls.Add(audioButtons);

                    WMP = new WindowsMediaPlayer();
                    WMP.MediaError += new _WMPOCXEvents_MediaErrorEventHandler(WMP_MediaError);
                    //WMP.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(WMP_PlayStateChange);
                    WMP.settings.autoStart = Settings.Default.autostartSong;
                    WMP.URL = question;
                    WMP.settings.volume = Settings.Default.defaultVolume;
                    //Small trick to allow WMP.currentMedia.duration to work
                    WMP.currentMedia.name = WMP.currentMedia.name;

                    progressBar = new TrackBar();
                    progressBar.Width = 200;
                    progressBar.Minimum = 0;
                    progressBar.Maximum = (int) WMP.currentMedia.duration;
                    progressBar.TickStyle = TickStyle.None; //Do not display ticks for smother scrolling
                    progressBar.Scroll += new EventHandler(progressBar_Scroll);

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
                    
                    break;
                case "Screenshot":
                    Image screenshot = Image.FromFile(question);
                    //Resize the image to fit a safe size (projector 1024x768)
                    int width = ClientRectangle.Width;
                    int height = width * screenshot.Height / screenshot.Width;
                    if (height > ClientRectangle.Height - 100)
                    {
                        height = ClientRectangle.Height - 100;
                        width = height * screenshot.Width / screenshot.Height;
                    }
                    Bitmap resizedScreenshot = new Bitmap(screenshot, new Size(width,height));

                    //Initialize the image
                    screenshotBox = new PictureBox();
                    screenshotBox.Image = resizedScreenshot;
                    screenshotBox.Location = new Point(12, 200);
                    screenshotBox.Width = ClientRectangle.Width;
                    screenshotBox.Height = ClientRectangle.Height;
                    questionPanel.Controls.Add(screenshotBox);
                    break;
                default:
                    break;
            }
        }

        #region Music Control
        void playBtn_Click(object sender, EventArgs e)
        {
            WMP.controls.play();
            timer.Start();
            createPlayTimer();
        }
        void pauseBtn_Click(object sender, EventArgs e)
        {
            if (WMP.playState.Equals(WMPPlayState.wmppsPaused)) WMP.controls.play();
            else
            {
                WMP.controls.pause();
                playTimer.Dispose();
            }
        }
        void stopBtn_Click(object sender, EventArgs e)
        {
            WMP.controls.stop();
            playTimer.Dispose();
        }
        void progressBar_Scroll(object sender, EventArgs e)
        {
            WMP.controls.currentPosition = Convert.ToDouble(progressBar.Value);
        }

        void WMP_MediaError(object pMediaObject)
        {
            MessageBox.Show("Error: cannot open or play music file");
            this.Close();
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

        private void answerBtn_Click(object sender, EventArgs e)
        {
            questionLabel.Text = answer;
            questionLabel.TextAlign = ContentAlignment.TopCenter;
            answered = true;
            closeBtn.Visible = true;
            switch (questionType)
            {
                case "Question":
                    break;
                case "Music":
                    WMP.close();
                    audioButtons.Controls.Clear();
                    break;
                case "Screenshot":
                    screenshotBox.Dispose();
                    break;
                default:
                    break;
            }
        }

        void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QuestionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (questionType != null && questionType.Equals("Music")) WMP.close();
            Settings.Default.tempQuestion = null;
            Settings.Default.tempType = null;
        }
    }
}
