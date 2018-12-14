using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public class SongModel
    {
        private string _title;
        private string _interpreter;
        private int    _seconds;

        /// <summary>
        /// Song title
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        /// <summary>
        /// Interpreter
        /// </summary>
        public string Interpreter
        {
            get
            {
                return this._interpreter;
            }
            set
            {
                this._interpreter = value;
            }
        }

        /// <summary>
        /// Duration in seconds
        /// </summary>
        public int Seconds
        {
            get
            {
                return this._seconds;
            }
            set
            {
                this._seconds = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SongModel()
        {

        }
    }
}
