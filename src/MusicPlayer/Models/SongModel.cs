using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using TagLib;
using NAudio.Wave;
using CommonCoreLibrary.Serialisation;

namespace MusicPlayer.Models
{
    public class SongModel : INotifyPropertyChanged
    {
        private FileInfo _fileInfo;
        private AudioFileReader _audioReader;
        private TagLib.File _fileTag;

        /// <summary>
        /// Audio reader
        /// </summary>
        public AudioFileReader AudioReader
        {
            get
            {
                return this._audioReader;
            }
        }

        /// <summary>
        /// File info
        /// </summary>
        public FileInfo FileInfo
        {
            get
            {
                return this._fileInfo;
            }
        }

        /// <summary>
        /// Song title
        /// </summary>
        public string Title
        {
            get
            {
                return this._fileTag.Tag.Title ?? this._fileInfo.Name;
            }
        }

        /// <summary>
        /// Interpreter
        /// </summary>
        public string Interpreter
        {
            get
            {
                string interpreter = string.Empty;
                foreach (string performer in this._fileTag.Tag.Performers)
                    interpreter += ", " + performer;
                return interpreter.Length > 2 ? interpreter.Substring(2) : string.Empty;
            }
        }

        /// <summary>
        /// Audio file is finished
        /// </summary>
        public bool Finished
        {
            get
            {
                return this._audioReader.Position == this._audioReader.Length;
            }
        }

        /// <summary>
        /// Property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public SongModel(FileInfo fileInfo)
        {
            this._fileInfo = fileInfo;
            this._audioReader = new AudioFileReader(this._fileInfo.FullName);
            this._fileTag = TagLib.File.Create(this._fileInfo.FullName);
        }

        /// <summary>
        /// Save model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SrlTag Save(SongModel model)
        {
            SrlCompound compound = new SrlCompound();
            compound.Add(new SrlString(model.FileInfo.FullName));
            return compound;
        }

        /// <summary>
        /// Load model
        /// </summary>
        /// <param name="srl"></param>
        /// <returns></returns>
        public static SongModel Load(SrlTag srl)
        {
            SrlCompound compound = (SrlCompound)srl;
            SongModel model = new SongModel(new FileInfo(compound.GetString().Value));
            return model;
        }
    }
}
