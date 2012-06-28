using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Windows.Controls;
using SmoothChords.Helpers;
using System.Windows;

namespace SmoothChords.Tests
{
    [TestFixture, RequiresSTA]
    public class HelpersTests
    {
        [Test]
        public void FindAncestor_FindsParent()
        {
            Grid grid1 = new Grid();
            Button button1 = new Button();
            grid1.Children.Add(button1);
            var result = Ancestors.FindAncestorOfType<Grid>(button1);
            Assert.IsTrue(result is Grid, "Result should be of type 'Grid'.");
            Assert.IsTrue((result as Grid) == grid1, "Result should be equal to 'grid1'.");
        }

        [Test]
        public void FindAncestor_NoStackPanelInVisualTree_ReturnsNull()
        {
            Grid grid1 = new Grid();
            Button button1 = new Button();
            grid1.Children.Add(button1);
            var result = Ancestors.FindAncestorOfType<StackPanel>(button1);
            Assert.IsNull(result, "Result should be 'null'.");
        }
    }
}
