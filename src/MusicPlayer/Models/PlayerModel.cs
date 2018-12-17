using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using CommonCoreLibrary.Serialisation;
using NAudio.Wave;

namespace MusicPlayer.Models
{
    public class PlayerModel : INotifyPropertyChanged
    {
        private PlaylistModel _playlist;
        private WaveOutEvent  _outputDevice;
        private string        _lastSavePath;

        /// <summary>
        /// Playlist
        /// </summary>
        public PlaylistModel Playlist
        {
            get
            {
                return this._playlist;
            }
            set
            {
                this._playlist = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Playlist"));
            }
        }

        /// <summary>
        /// Output device
        /// </summary>
        public WaveOutEvent OutputDevice
        {
            get
            {
                return this._outputDevice;
            }
        }

        /// <summary>
        /// Current song
        /// </summary>
        public SongModel CurrentSong
        {
            get
            {
                return this._playlist.CurrentSong;
            }
            set
            {
                this._playlist.CurrentSong = value;
            }
        }

        /// <summary>
        /// Playback state
        /// </summary>
        public PlaybackState PlaybackState
        {
            get
            {
                return this._outputDevice.PlaybackState;
            }
        }

        /// <summary>
        /// Last save path
        /// </summary>
        public string LastSavePath
        {
            get
            {
                return this._lastSavePath;
            }
        }

        /// <summary>
        /// Property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Playback stopped event
        /// </summary>
        public event EventHandler<StoppedEventArgs> PlaybackStopped
        {
            add
            {
                this._outputDevice.PlaybackStopped += value;
            }
            remove
            {
                this._outputDevice.PlaybackStopped -= value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerModel(string lastPlaylist)
        {
            if (!this.LoadPlaylist(lastPlaylist))
                this._playlist = new PlaylistModel("Playlist 1");

            this._outputDevice = new WaveOutEvent();
        }

        /// <summary>
        /// Play
        /// </summary>
        public void Play()
        {
            if (this._playlist.CurrentSong == null)
                return;

            if (this._outputDevice.PlaybackState != PlaybackState.Playing)
                this._outputDevice.Play();
        }

        /// <summary>
        /// Pause 
        /// </summary>
        public void Pause()
        {
            if (this._playlist.CurrentSong == null)
                return;

            if (this._outputDevice.PlaybackState == PlaybackState.Playing)
                this._outputDevice.Pause();
        }

        /// <summary>
        /// Play song
        /// </summary>
        /// <param name="song"></param>
        public void SetSong(SongModel song, PlaybackState state)
        {
            if (this._playlist.CurrentSong != song)
            {
                this._outputDevice.Stop();

                this._playlist.CurrentSong = song;

                this._outputDevice.Init(this.CurrentSong.AudioReader);

                if (state == PlaybackState.Playing)
                    this._outputDevice.Play();
            }
        }

        /// <summary>
        /// Skip to next song
        /// </summary>
        public void SkipNext(PlaybackState state)
        {
            SongModel song = this._playlist.GetNextSong();
            if (song != null)
                this.SetSong(song, state);
        }

        /// <summary>
        /// Skip to previous song
        /// </summary>
        public void SkipPrevious(PlaybackState state)
        {
            SongModel song = this._playlist.GetPreviousSong();
            if (song != null)
                this.SetSong(song, state);
        }

        /// <summary>
        /// Save playlist
        /// </summary>
        /// <param name="path"></param>
        public bool SavePlaylist(string path)
        {
            SrlSerializer serializer = new SrlSerializer(path);
            serializer.Serializer(PlaylistModel.Save(this.Playlist));
            this._lastSavePath = path;
            return true;
        }

        /// <summary>
        /// Load playlist
        /// </summary>
        /// <param name="path"></param>
        public bool LoadPlaylist(string path)
        {
            if (File.Exists(path))
            {
                SrlSerializer serializer = new SrlSerializer(path);
                this.Playlist = PlaylistModel.Load(serializer.Deserialize());
                this._lastSavePath = path;
                return true;
            }
            return false;
        }
    }
}
