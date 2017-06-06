using System;
using NUnit.Framework;
using Minesweeper.Library;
using Minesweeper.Library.Converters;

namespace Minesweeper.Library.Test
{
    [TestFixture]
    public class InvertedBoolConverterTest
    {
        [Test]
        public void Converter_Returns_Inverse()
        {
            var converter = new InvertedBoolConverter();
            Assert.AreNotEqual(true, converter.Convert(true, null, null, null));
            Assert.AreNotEqual(false, converter.Convert(false, null, null, null));
        }
    }
}
