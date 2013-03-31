using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Anime_Quiz_3.Classes
{
    class SoundMessageBox
    {
        public static MessageBoxResult Show(String message, Stream soundRes)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundRes);
            soundPlayer.Play();
            return MessageBox.Show(message);
        }
        public static MessageBoxResult Show(String message, String caption, Stream soundRes)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundRes);
            soundPlayer.Play();
            return MessageBox.Show(message, caption);
        }
        public static MessageBoxResult Show(String message, String caption, MessageBoxButton buttons, Stream soundRes)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundRes);
            soundPlayer.Play();
            return MessageBox.Show(message, caption, buttons);
        }
    }
}
