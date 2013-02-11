using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anime_Quiz.Classes
{
    class SoundMessageBox
    {
        public static DialogResult Show(String message, Stream soundRes)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundRes);
            soundPlayer.Play();
            return MessageBox.Show(message);
        }
        public static DialogResult Show(String message, String caption, Stream soundRes)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundRes);
            soundPlayer.Play();
            return MessageBox.Show(message, caption);
        }
        public static DialogResult Show(String message, String caption, MessageBoxButtons buttons, Stream soundRes)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundRes);
            soundPlayer.Play();
            return MessageBox.Show(message, caption, buttons);
        }
    }
}
