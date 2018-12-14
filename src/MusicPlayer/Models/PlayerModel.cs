using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class PlayerModel
    {
        private List<SongModel> _songs = new List<SongModel>();

        /// <summary>
        /// Songs
        /// </summary>
        public List<SongModel> Songs
        {
            get
            {
                return this._songs;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerModel()
        {
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
            this._songs.Add(new SongModel() { Title = "Feuer frei!", Interpreter = "Rammstein", Seconds = 186 });
        }
    }
}
