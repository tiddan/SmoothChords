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
    public class PianoControl : Control
    {
        private static Pen _defaultPen = new Pen(Brushes.Black, 2.0);

        static PianoControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PianoControl), new FrameworkPropertyMetadata(typeof(PianoControl)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            for (int i = 0; i < 49; i++)
            {
                drawingContext.DrawRectangle(Brushes.White, _defaultPen, new Rect(new Point(i * 13, 0), new Point((i + 1) * 13, 85)));
            }
        }
    }
}
