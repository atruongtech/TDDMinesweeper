using NUnit.Framework;
using Minesweeper.Library.Converters;
using System.Windows;

namespace Minesweeper.Library.Test
{
    [TestFixture]
    public class BoolToVisConverterTest
    {
        [Test]
        public void NormalInversion_Returns_VisForTrue_HidForFalse()
        {
            var converter = new BoolToVisibilityConverter();
            Assert.AreEqual(Visibility.Visible, converter.Convert(true, null, "Normal", null));
            Assert.AreEqual(Visibility.Hidden, converter.Convert(false, null, "Normal", null));
        }

        [Test]
        public void Inversion_Returns_HidForTrue_VisForFalse()
        {
            var converter = new BoolToVisibilityConverter();
            Assert.AreEqual(Visibility.Visible, converter.Convert(false, null, "Inverted", null));
            Assert.AreEqual(Visibility.Hidden, converter.Convert(true, null, "Inverted", null));
        }
    }
}
