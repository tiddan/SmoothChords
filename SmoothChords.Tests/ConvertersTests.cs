using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothChords.Converters;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows;

namespace SmoothChords.Tests
{
    [TestFixture]
    public class ConvertersTests
    {
        [Test]
        public void BoolInverter_FalseBecomeTrue()
        {
            BoolInverter b = new BoolInverter();
            var res = b.Convert(false, typeof(bool), null, null);
            Assert.IsTrue((bool)res == true, "Result should be 'true'.");
        }

        [Test]
        public void BoolInverter_TrueBecomeFalse()
        {
            BoolInverter b = new BoolInverter();
            var res = b.Convert(true, typeof(bool), null, null);
            Assert.IsTrue((bool)res == false, "Result should be 'false'.");
        }

        // --

        [Test]
        public void BoolToColorConverter_TrueReturnsYellowBrush()
        {
            BoolToColorConverter converter = new BoolToColorConverter();
            var res = converter.Convert(true, typeof(Brush), null, null);
            res = res as Brush;
            Assert.IsTrue(res == Brushes.Yellow, "Returned brush should be 'Yellow'.");
        }

        [Test]
        public void BoolToColorConverter_FalseReturnsWhiteBrush()
        {
            BoolToColorConverter converter = new BoolToColorConverter();
            var res = converter.Convert(false, typeof(Brush), null, null);
            res = res as Brush;
            Assert.IsTrue(res == Brushes.White, "Returned brush should be 'White'.");
        }

        // --

        [Test]
        public void BoolToVisibilityConverter_TrueReturnsVisible()
        {
            BoolToVisibilityConverter c = new BoolToVisibilityConverter();
            var res = c.Convert(true, typeof(Visibility), null, null);
            Assert.IsTrue((Visibility)res == Visibility.Visible, "Should have been 'Visible'");
        }

        [Test]
        public void BoolToVisibilityConverter_FalseReturnsHidden()
        {
            BoolToVisibilityConverter c = new BoolToVisibilityConverter();
            var res = c.Convert(false, typeof(Visibility), null, null);
            Assert.IsTrue((Visibility)res == Visibility.Hidden, "Should have been 'Hidden'");
        }

        [Test]
        public void BoolToVisibilityConverterReverse_TrueReturnsHidden()
        {
            BoolToVisibilityConverterReverse c = new BoolToVisibilityConverterReverse();
            var res = c.Convert(true, typeof(Visibility), null, null);
            Assert.IsTrue((Visibility)res == Visibility.Hidden, "Should have been 'Hidden'");
        }

        [Test]
        public void BoolToVisibilityConverterReverse_FalseReturnsVisible()
        {
            BoolToVisibilityConverterReverse c = new BoolToVisibilityConverterReverse();
            var res = c.Convert(false, typeof(Visibility), null, null);
            Assert.IsTrue((Visibility)res == Visibility.Visible, "Should have been 'Visible'");
        }
    }
}
