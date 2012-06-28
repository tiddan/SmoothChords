using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace SmoothChords.Components
{
    public class ClosableTabItem : TabItem
    {
        public ClosableTabItem()
        {
            ClosableTabHeader tabHeader = new ClosableTabHeader();
            Header = tabHeader;
            (Header as ClosableTabHeader).HeaderTitle.Content = "Untitled";

            (Header as ClosableTabHeader).CloseButton.Click += new System.Windows.RoutedEventHandler(CloseButton_Click);


        }

        void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.Parent as TabControl).Items.Remove(this);
        }

        public string Title
        {
            set
            {
                (Header as ClosableTabHeader).HeaderTitle.Content = value;
            }
        }

        protected override void OnSelected(System.Windows.RoutedEventArgs e)
        {
            base.OnSelected(e);

            (Header as ClosableTabHeader).CloseButton.Visibility = System.Windows.Visibility.Visible;
        }

        protected override void OnUnselected(System.Windows.RoutedEventArgs e)
        {
            base.OnUnselected(e);

            (Header as ClosableTabHeader).CloseButton.Visibility = System.Windows.Visibility.Hidden;
        }


    }
}
