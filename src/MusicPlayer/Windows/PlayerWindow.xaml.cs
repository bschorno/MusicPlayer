using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using MusicPlayer.Models;
using System.Windows.Controls.Primitives;

namespace MusicPlayer.Windows
{
    public partial class PlayerWindow : Window
    {
        private PlayerModel  _model;
        private Thread       _updateThread;
        private SongModel    _songDndSource;

        /// <summary>
        /// Consctuctor
        /// </summary>
        public PlayerWindow()
        {
            InitializeComponent();

            this._model = new PlayerModel(Properties.Settings.Default.LastPlaylist);
            this._model.PlaybackStopped += new EventHandler<StoppedEventArgs>(OnPlayerStopped);

            this._updateThread = new Thread(new ThreadStart(Update));
            this._updateThread.Start();

            this.DataContext = this._model;
        }

        /// <summary>
        /// Update thread
        /// </summary>
        private void Update()
        {
            while (true)
            {
                if (this._model.PlaybackState == PlaybackState.Playing)
                {
                    BindingExpression binding1 = sldCurrentTime.GetBindingExpression(Slider.ValueProperty);
                    Dispatcher.Invoke(() => { binding1.UpdateTarget(); } );

                    BindingExpression binding2 = lblCurrentTime.GetBindingExpression(Label.ContentProperty);
                    Dispatcher.Invoke(() => { binding2.UpdateTarget(); });
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// On window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            this._updateThread.Abort();
            Properties.Settings.Default["LastPlaylist"] = this._model.LastSavePath;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// On play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlay(object sender, RoutedEventArgs e)
        {
            if (this._model.PlaybackState == PlaybackState.Playing)
                this._model.Pause();
            else
                this._model.Play();
        }

        /// <summary>
        /// On skip to next song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkipNext(object sender, RoutedEventArgs e)
        {
            this._model.SkipNext(this._model.PlaybackState);
        }

        /// <summary>
        /// On skip to previous song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkipPrevious(object sender, RoutedEventArgs e)
        {
            this._model.SkipPrevious(this._model.PlaybackState);
        }

        /// <summary>
        /// On slider value changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioFileReader reader = this._model.CurrentSong.AudioReader;
            reader.Position = (long)((e.NewValue * reader.Length) / sldCurrentTime.Maximum);
        }

        /// <summary>
        /// On player stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPlayerStopped(object sender, StoppedEventArgs args)
        {
            if (this._model.CurrentSong.Finished)
            {
                this._model.SkipNext(PlaybackState.Playing);
            }
        }

        /// <summary>
        /// On play a new song from the playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistPlay(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.OriginalSource;
            SongModel song = (SongModel)button.DataContext;

            this._model.SetSong(song, this._model.PlaybackState);
        }

        /// <summary>
        /// On playlist drop 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                for (int i = 0; i < paths.Length; i++)
                {
                    this._model.Playlist.AddSong(new SongModel(new FileInfo(paths[i])));
                }
                e.Handled = true;
            }
            else
            {
                int targetIndex = 0;
                int sourceIndex = this._model.Playlist.Songs.IndexOf(this._songDndSource);
                if (sender is ListView)
                {
                    targetIndex = this._model.Playlist.Songs.Count;
                }
                else if (e.Data.GetDataPresent(typeof(SongModel)))
                {
                    ListBoxItem listBoxItem = sender as ListBoxItem;
                    SongModel model = listBoxItem.DataContext as SongModel;
                    targetIndex = this._model.Playlist.Songs.IndexOf(model);
                }
                else
                {
                    return;
                }
                
                if (sourceIndex == targetIndex)
                {
                    e.Handled = true;
                }
                else if (sourceIndex > targetIndex)
                {
                    if (sourceIndex != -1)
                        this._model.Playlist.Songs.RemoveAt(sourceIndex);
                    this._model.Playlist.Songs.Insert(targetIndex, (this._songDndSource));
                    e.Handled = true;
                }
                else
                {
                    if (targetIndex + 1 > this._model.Playlist.Songs.Count)
                        targetIndex = this._model.Playlist.Songs.Count - 1;
                    this._model.Playlist.Songs.Insert(targetIndex + 1, (this._songDndSource));
                    if (sourceIndex != -1)
                        this._model.Playlist.Songs.RemoveAt(sourceIndex);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// On playlist item drag over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistItemDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(SongModel)) && !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// On playlist item drag over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistItemDrop(object sender, DragEventArgs e)
        {
            this.OnPlaylistDrop(sender, e);
        }

        /// <summary>
        /// On playlist item preview mouse move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistItemPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            ListBoxItem listboxItem = (ListBoxItem)sender;
            if (listboxItem == null)
                return;
            this._songDndSource = listboxItem.DataContext as SongModel;
            if (this._songDndSource == null)
                return;
            DataObject data = new DataObject();
            data.SetData(this._songDndSource);
            DragDropEffects effect = DragDrop.DoDragDrop(listboxItem, data, DragDropEffects.Move | DragDropEffects.Copy);
        }

        /// <summary>
        /// On playlist save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Playlist (*.mpp)|*.mpp";
            if (saveFileDialog.ShowDialog() == true)
            {
                this._model.SavePlaylist(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// On playlist load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaylistLoad(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Playlist (*.mpp)|*.mpp";
            if (openFileDialog.ShowDialog() == true)
            {
                this._model.LoadPlaylist(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Can execute play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanExecutePlay(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this._model             != null &&
                this._model.CurrentSong != null)
                e.CanExecute = true;
        }

        /// <summary>
        /// Can execute skip next
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanExecuteSkipNext(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this._model != null &&
                this._model.Playlist.GetNextSong() != null)
                e.CanExecute = true;
        }

        /// <summary>
        /// Can execute skip previous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanExecuteSkipPrevious(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this._model != null &&
                this._model.Playlist.GetPreviousSong() != null)
                e.CanExecute = true;
        }       
     }
}
