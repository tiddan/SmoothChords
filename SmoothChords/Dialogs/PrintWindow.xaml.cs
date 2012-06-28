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
using System.Windows.Shapes;
using SmoothChords.ViewModel;
using SmoothChords.Model;
using SmoothChords.Components;
using SmoothChords.View;
using System.Windows.Media.Effects;
using GalaSoft.MvvmLight.Command;

namespace SmoothChords.Dialogs
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        private RelayCommand _printCommand = null;


        public PrintWindow(DocumentViewModel model )
        {
            InitializeComponent();
            DataContext = this;
            BuildDocument(model);
        }

        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(PrintExecute);
                }
                return _printCommand;
            }
        }



        private void BuildDocument(DocumentViewModel model)
        {
            ChordDocument.PageWidth = 850;
            ChordDocument.PageHeight = 1190;

            char[] sep = { '.' };
            var songTitle = model.Title.Split(sep)[0];

            Run title = new Run(songTitle);
            title.FontSize=40;

            Paragraph titlePar = new Paragraph();
            titlePar.Inlines.Add(title);
            titlePar.TextAlignment = TextAlignment.Center;
            titlePar.Margin = new Thickness(30);

            ChordDocument.Blocks.Add(titlePar);

            foreach (ChordLine line in model.ChordLines)
            {
                var section = new Section();
                var chordsPar = new Paragraph();
                chordsPar.Margin = new Thickness(0, 0, 10, 0);
                var grid = new NoteGrid();
                grid.DataContext = line;
                var gridPar = new Paragraph(new InlineUIContainer(grid));
                gridPar.Margin = new Thickness(10,0,10,0);

                var chords = new Canvas();
                chords.Height = 30;

                foreach (Chord chord in line.Chords)
                {
                    var chordView = new ChordView();
                    chordView.DataContext = chord;
                    chordView.MainBorder.BorderThickness = new Thickness(0);
                    chordView.MainBorder.BorderBrush = Brushes.Transparent;
                    chordView.MainBorder.BitmapEffect = null;
                    chordView.ChordLabel.FontSize = 20;
                    chords.Children.Add(chordView);
                }
                
                chordsPar.KeepWithNext = true;
                chordsPar.Inlines.Add(new InlineUIContainer(chords));

                section.Blocks.Add(chordsPar);
                section.Blocks.Add(gridPar);

                ChordDocument.Blocks.Add(section);
            }
        }

        public void PrintExecute()
        {
            ChordDocumentReader.Print();
        }

        
    }
}
