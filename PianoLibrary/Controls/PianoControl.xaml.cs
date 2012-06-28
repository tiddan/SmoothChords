using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace PianoLibrary.Controls
{
    /// <summary>
    /// Interaction logic for PianoControl.xaml
    /// </summary>
    public partial class PianoControl : UserControl
    {
        public ObservableCollection<int> NotesPlayed
        {
            get
            {
                return (ObservableCollection<int>)this.GetValue(NotesPlayedProperty);
            }
            set
            {
                SetValue(NotesPlayedProperty, value);
            }
        }

        public static readonly DependencyProperty NotesPlayedProperty = DependencyProperty.Register(
            "NotesPlayed", typeof(ObservableCollection<int>), typeof(PianoControl), new PropertyMetadata(new ObservableCollection<int>()));

        private Piano _piano = null;

        public Piano Piano
        {
            get
            {
                if (_piano == null)
                {
                    _piano = new Piano();
                }
                return _piano;
            }
        }

        public PianoControl()
        {
            InitializeComponent();

            foreach (PianoKey key in Piano.Keys)
            {
                PianoCanvas.Children.Add(key.Geometry);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == NotesPlayedProperty)
            {
                for (int i = 0; i < Piano.Keys.Count(); i++)
                {
                    Piano.Keys[i].ResetColor();
                }

                foreach (int i in NotesPlayed)
                {
                    Piano.Keys[i].Geometry.Fill = Brushes.Yellow;
                }
            }
        }
    }
}
