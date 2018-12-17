using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicPlayer.Commands
{
    public static class PlayerCommand
	{
		public static readonly RoutedUICommand PlayPause = new RoutedUICommand(
			"Play/Pause",
			"Play/Pause",
			typeof(PlayerCommand)
		);
        public static readonly RoutedUICommand PlayNew = new RoutedUICommand(
            "Play",
            "Play",
            typeof(PlayerCommand)    
        );
        public static readonly RoutedUICommand SkipNext = new RoutedUICommand(
            "Skip next",
            "Skip next",
            typeof(PlayerCommand)
        );
        public static readonly RoutedUICommand SkipPrevious = new RoutedUICommand(
            "Skip previous",
            "Skip previous",
            typeof(PlayerCommand)
        );
        public static readonly RoutedUICommand SavePlaylist = new RoutedUICommand(
            "Save Playlist",
            "Save Playlist",
            typeof(PlayerCommand)
        );
        public static readonly RoutedUICommand LoadPlaylist = new RoutedUICommand(
            "Load Playlist",
            "Load Playlist",
            typeof(PlayerCommand)
        );
	}
}
