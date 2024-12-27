using System.Windows;
using CHESSGAME.ViewModel.Game;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;
using System.Media;
using System;
using System.Windows.Resources;

namespace CHESSGAME.Files
{
    public static class Sound
    {
        public static void PlayClickSound()
        {
            var uri = new Uri("pack://application:,,,/Files/click_sound.wav");

            StreamResourceInfo resource = Application.GetResourceStream(uri);
            SoundPlayer player = new SoundPlayer(resource.Stream);
            player.Play();
        }
        public static void SelectClickSound()
        {
            var uri = new Uri("pack://application:,,,/Files/select_sound.wav");

            StreamResourceInfo resource = Application.GetResourceStream(uri);
            SoundPlayer player = new SoundPlayer(resource.Stream);
            player.Play();
        }
        public static void MoveClickSound()
        {
            var uri = new Uri("pack://application:,,,/Files/move_sound.wav");

            StreamResourceInfo resource = Application.GetResourceStream(uri);
            SoundPlayer player = new SoundPlayer(resource.Stream);
            player.Play();
        }
    }
}
