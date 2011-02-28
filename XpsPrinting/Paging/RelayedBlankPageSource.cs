using System;

namespace XpsPrinting.Paging
{
    public static class RelayedBlankPageSource
    {
        public static RelayedBlankPageSource<T> Create<T>(Func<int, T> pageFactoryMethod)
            where T: IBlankPage
        {
            return new RelayedBlankPageSource<T>(pageFactoryMethod);
        }
    }

    public class RelayedBlankPageSource<T> : IBlankPageSource
        where T : IBlankPage
    {
        private readonly Func<int, T> _pageFactoryMethod;

        public RelayedBlankPageSource(Func<int, T> pageFactoryMethod)
        {
            if (pageFactoryMethod == null) throw new ArgumentNullException("pageFactoryMethod");

            _pageFactoryMethod = pageFactoryMethod;
        }

        public IBlankPage CreateBlankPage(int pageNumber)
        {
            return _pageFactoryMethod(pageNumber);
        }
    }
}