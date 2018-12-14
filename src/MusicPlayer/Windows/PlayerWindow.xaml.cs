using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MusicPlayer.Models;

namespace MusicPlayer.Windows
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private PlayerModel _model;

        /// <summary>
        /// Consctuctor
        /// </summary>
        public PlayerWindow()
        {
            InitializeComponent();

            this._model = new PlayerModel();

            this.DataContext = this._model;
        }
    }
}
