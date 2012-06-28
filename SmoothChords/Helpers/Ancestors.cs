using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SmoothChords.Helpers
{
    public class Ancestors
    {
        public static DependencyObject FindAncestorOfType<T>(DependencyObject obj)
        {
            if (obj != null)
            {
                var parent = VisualTreeHelper.GetParent(obj);
                while (parent != null)
                {
                    if (parent.GetType() == typeof(T))
                    {
                        return parent;
                    }
                    Console.WriteLine(parent.ToString());
                    parent = VisualTreeHelper.GetParent(parent);
                    
                }
            }

            return null;
        }
    }
}
