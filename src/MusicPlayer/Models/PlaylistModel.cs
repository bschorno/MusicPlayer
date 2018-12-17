using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CommonCoreLibrary.Serialisation;

namespace MusicPlayer.Models
{
    public class PlaylistModel : INotifyPropertyChanged
    {
        private ObservableCollection<SongModel> _songs = new ObservableCollection<SongModel>();
        private SongModel                       _currentSong;
        private string                          _name;

        /// <summary>
        /// Songs
        /// </summary>
        public ObservableCollection<SongModel> Songs
        {
            get
            {
                return this._songs;
            }
        }

        /// <summary>
        /// Curent song
        /// </summary>
        public SongModel CurrentSong
        {
            get
            {
                return this._currentSong;
            }
            set
            {
                this._currentSong = value;
                this._currentSong.AudioReader.Position = 0;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentSong"));

            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        /// Property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlaylistModel(string name)
        {
            this._name = name;
        }

        /// <summary>
        /// Get next song
        /// </summary>
        /// <returns></returns>
        public SongModel GetNextSong()
        {
            if (this._currentSong == null)
                return null;
            int index = this._songs.IndexOf(this._currentSong);
            if (index >= this._songs.Count - 1)
                return null;
            return this._songs[++index];
        }

        /// <summary>
        /// Get prevous song
        /// </summary>
        /// <returns></returns>
        public SongModel GetPreviousSong()
        {
            if (this._currentSong == null)
                return null;
            int index = this._songs.IndexOf(this._currentSong);
            if (index <= 0)
                return null;
            return this._songs[--index];
        }

        /// <summary>
        /// Add new song
        /// </summary>
        /// <param name="song"></param>
        public void AddSong(SongModel song)
        {
            // Check file type
            if (song.FileInfo.Extension.ToLower() != ".mp3" &&
                song.FileInfo.Extension.ToLower() != ".wav")
                throw new Exception("No supported file format!");

            // Check if song already is contained
            foreach (SongModel var1 in this._songs)
            {
                if (var1.FileInfo.FullName == song.FileInfo.FullName)
                    throw new Exception("Song already in this playlist!");
            }

            // Add song to playlist
            this._songs.Add(song);
        }

        /// <summary>
        /// Remove song
        /// </summary>
        /// <param name="song"></param>
        public void RemoveSong(SongModel song)
        {
            // Remove song
            this._songs.Remove(song);
        }

        /// <summary>
        /// Save model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SrlTag Save(PlaylistModel model)
        {
            SrlCompound compound = new SrlCompound();
            SrlList list = new SrlList(SrlType.Compound);

            compound.Add(new SrlString(model.Name));

            foreach (SongModel song in model.Songs)
                list.Add(SongModel.Save(song));

            compound.Add(list);

            return compound;
        }

        /// <summary>
        /// Load model
        /// </summary>
        /// <param name="srl"></param>
        /// <returns></returns>
        public static PlaylistModel Load(SrlTag srl)
        {
            SrlCompound compound = (SrlCompound)srl;
            
            PlaylistModel model = new PlaylistModel(compound.GetString().Value);

            SrlList list = compound.GetList();

            foreach (SrlTag tag in list)
                model._songs.Add(SongModel.Load(tag));

            return model;
        }
    }
}
