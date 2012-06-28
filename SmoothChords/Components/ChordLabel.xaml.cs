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

namespace SmoothChords.Components
{
    /// <summary>
    /// Interaction logic for ChordLabel.xaml
    /// </summary>
    public partial class ChordLabel : UserControl
    {
        public ChordLabel()
        {
            InitializeComponent();
        }

        private void WrapPanel_LostFocus(object sender, RoutedEventArgs e)
        {
            ChordBase.IsReadOnly = true;
            ChordExp.IsReadOnly = true;
        }
    }
}
