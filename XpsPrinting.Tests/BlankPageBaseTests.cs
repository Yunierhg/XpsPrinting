using System.Windows;
using NUnit.Framework;
using XpsPrinting.Paging;

namespace XpsPrinting.Tests
{
    [TestFixture]
    public class BlankPageBaseTests
    {
        [Test]
        public void ContentBox_PageSizeAndMarginsWasSet_EqualPageExceptMargins()
        {
            var sut = new BlankPageBase();
            sut.PageSize = new Size(100, 100);
            sut.Margin = new Thickness(10);
            Assert.AreEqual(new Rect(10, 10, 80, 80), sut.ContentBox);
        }

        [Test]
        public void DataContentBox_PageSizeAndMarginsWasSet_EqualPageExceptMargins()
        {
            var sut = new BlankPageBase();
            sut.PageSize = new Size(100, 100);
            sut.Margin = new Thickness(10);
            Assert.AreEqual(new Rect(10, 10, 80, 80), sut.DataContentBox);
        }
    }
}