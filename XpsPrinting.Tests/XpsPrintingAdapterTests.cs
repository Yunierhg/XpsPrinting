using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using NUnit.Framework;
using XpsPrinting.Documents;
using System.Linq;

namespace XpsPrinting.Tests
{
    [TestFixture]
    public class XpsPrintingAdapterTests
    {
        private DocumentStub _documentStub;

        private class DocumentStub : IDocument
        {
            private readonly IEnumerable<IPage> _pages;

            public int EnumeratorCreationCallCount { get; private set; }

            public DocumentStub(IEnumerable<IPage> pages)
            {
                _pages = pages;
            }

            public IEnumerator<IPage> GetEnumerator()
            {
                EnumeratorCreationCallCount++;
                return _pages.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                EnumeratorCreationCallCount++;
                return GetEnumerator();
            }
        }

        private class PageStub : IPage
        {
            public int Id { get; set; }

            public PageStub(int id)
            {
                Id = id;
            }

            public Size PageSize
            {
                get { throw new NotImplementedException(); }
            }

            public Rect ContentBox
            {
                get { throw new NotImplementedException(); }
            }

            public Visual Visual
            {
                get { throw new NotImplementedException(); }
            }
        }

        private class TestableXpsPrintingDocumentPaginator : XpsPrintingDocumentPaginator
        {
            public List<int> Callings { get; private set; }

            public TestableXpsPrintingDocumentPaginator(IDocument document) : base(document)
            {
                Callings = new List<int>();
            }

            protected override DocumentPage CreateDocumentPage(IPage page)
            {
                Callings.Add(((PageStub)page).Id);
                return null;
            }
        }

        private TestableXpsPrintingDocumentPaginator GetPaginator(params int[] pageIds)
        {
            var pages = pageIds.Select(id => (IPage)new PageStub(id));
            _documentStub = new DocumentStub(pages);
            return new TestableXpsPrintingDocumentPaginator(_documentStub);
        }

        [Test]
        public void Ctor_EmptyDocument_PageCountIsNullAndValid()
        {
            var sut = GetPaginator();
            Assert.AreEqual(0, sut.PageCount);
            Assert.IsTrue(sut.IsPageCountValid);
        }

        [Test]
        public void GetPage_EmptyDocument_ReturnsMissingPage()
        {
            var sut = GetPaginator();
            var r = sut.GetPage(0);
            Assert.AreSame(DocumentPage.Missing, r);
        }

        [Test]
        public void GetPage_NegativePageNumber_ReturnsMissingPage()
        {
            var sut = GetPaginator(1, 2, 3);
            var r = sut.GetPage(-1);
            Assert.AreSame(DocumentPage.Missing, r);
        }

        [Test]
        public void GetPage_PageNumberBeyondDocument_ReturnsMissingPage()
        {
            var sut = GetPaginator(1, 2, 3);
            var r = sut.GetPage(3);
            Assert.AreSame(DocumentPage.Missing, r);
        }

        [Test]
        public void GetPage_PageNumberBeyondDocument_PageCountSetsCorrectly()
        {
            var sut = GetPaginator(1, 2, 3);
            sut.GetPage(3);
            Assert.IsTrue(sut.IsPageCountValid);
            Assert.AreEqual(3, sut.PageCount);
        }

        [Test]
        public void GetPage_PageNumberIsCorrect_CallsCreatePageCorrectly()
        {
            var sut = GetPaginator(10, 20, 30);
            sut.GetPage(1);
            CollectionAssert.AreEqual(new[] { 20 }, sut.Callings);
        }

        [Test]
        public void GetPage_PageNumberWithGreaterNumberAlreadyReqested_EnumeratorRecreated()
        {
            var sut = GetPaginator(10, 20, 30);
            sut.GetPage(1);
            sut.GetPage(0);
            Assert.AreEqual(2, _documentStub.EnumeratorCreationCallCount);
            CollectionAssert.AreEqual(new[]{20, 10}, sut.Callings);
        }

        [Test]
        public void GetPage_PageNumberWithLessNumberAlreadyReqested_EnumeratorIsNotRecreated()
        {
            var sut = GetPaginator(10, 20, 30);
            sut.GetPage(0);
            sut.GetPage(1);
            Assert.AreEqual(1, _documentStub.EnumeratorCreationCallCount);
            CollectionAssert.AreEqual(new[] { 10, 20 }, sut.Callings);
        }

        [Test]
        public void GetPage_LastPageNumber_PageCountIsSet()
        {
            var sut = GetPaginator(10, 20, 30);
            sut.GetPage(2);
            Assert.IsTrue(sut.IsPageCountValid);
            Assert.AreEqual(3, sut.PageCount);
        }
    }
}