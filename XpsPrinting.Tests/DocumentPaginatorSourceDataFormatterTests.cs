using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using NUnit.Framework;
using Rhino.Mocks;
using XpsPrinting.Formatting;

namespace XpsPrinting.Tests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class DocumentPaginatorSourceDataFormatterTests
    {
        private class Testable : DocumentPaginatorDataFormatter
        {
            private DocumentPaginator _returnResult;

            public Testable(DocumentPaginator returnResult)
            {
                _returnResult = returnResult;
            }

            protected override DocumentPaginator GetDocumentPaginator(Size maxPageSize)
            {
                return _returnResult;
            }
        }

        private class VisualStub : Visual
        {
            public int Id { get; private set; }

            public VisualStub(int id)
            {
                Id = id;
            }
        }

        private Testable GetSut(params DocumentPage[] pages)
        {
            var paginatorStub = MockRepository.GenerateStub<DocumentPaginator>();
            paginatorStub.Stub(p => p.GetPage(0))
                .IgnoreArguments()
                .Return(null)
                .WhenCalled(mi =>
                                {
                                    int pageNum = (int) mi.Arguments[0];
                                    mi.ReturnValue = pageNum >= 0 && pageNum < pages.Length 
                                        ? pages[pageNum] 
                                        : DocumentPage.Missing;
                                })
                .Repeat.Any();

            return new Testable(paginatorStub);
        }

        private DocumentPage CreatePage(int visualId, double width, double heigth)
        {
            var visualStub = new VisualStub(visualId);
            var pageStub = MockRepository.GenerateStub<DocumentPage>(visualStub);
            pageStub.Stub(ps => ps.Visual).Repeat.Any().Return(visualStub);
            pageStub.Stub(ps => ps.Size).Repeat.Any().Return(new Size(width, heigth));
            return pageStub;
        }

        private void AssertPageVisual(int expectedId, Visual pageVisual)
        {
            var visualStub = (VisualStub) pageVisual;
            Assert.AreEqual(expectedId, visualStub.Id);
        }

        [Test]
        public void GetNextPortion_AvailableSizeIsEqualToPageSize_ReturnsCorrectPageVisual()
        {
            var sut = GetSut(CreatePage(1, 10, 15));
            var pageVisual = sut.GetNextPortion(new Size(10, 15));
            AssertPageVisual(1, pageVisual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNextPortion_AvailableWidthIsLessThanPageSize_Throws()
        {
            var sut = GetSut(CreatePage(1, 10, 15));
            sut.GetNextPortion(new Size(9, 15));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNextPortion_AvailableHeightIsLessThanPageSize_Throws()
        {
            var sut = GetSut(CreatePage(1, 10, 15));
            sut.GetNextPortion(new Size(10, 14));
        }

        [Test]
        public void GetNextPortion_NoPagesPresent_ReturnsNull()
        {
            var sut = GetSut();
            sut.GetNextPortion(new Size(1, 1));
        }

        [Test]
        public void GetNextPortion_SinglePage_ReturnsNullOnSecondCall()
        {
            var sut = GetSut(CreatePage(1, 10, 10));
            sut.GetNextPortion(new Size(10, 10));
            var second = sut.GetNextPortion(new Size(1, 1));
            Assert.IsNull(second);
        }

        [Test]
        public void GetNextPortion_TwoPages_ReturnsBothPagesCorrectly()
        {
            var sut = GetSut(CreatePage(1, 10, 10), CreatePage(2, 10, 10));
            var first = sut.GetNextPortion(new Size(10, 10));
            var second = sut.GetNextPortion(new Size(10, 10));
            var third = sut.GetNextPortion(new Size(10, 10));
            AssertPageVisual(1, first);
            AssertPageVisual(2, second);
            Assert.IsNull(third);
        }

        [Test]
        public void ResetPosition_CalledAfterOnePageAcquired_GetsFirstPageNext()
        {
            var sut = GetSut(CreatePage(1, 10, 10), CreatePage(2, 10, 10));
            sut.GetNextPortion(new Size(10, 10));
            sut.ResetPosition();
            var first = sut.GetNextPortion(new Size(10, 10));
            AssertPageVisual(1, first);
        }
    }
    // ReSharper restore InconsistentNaming
}