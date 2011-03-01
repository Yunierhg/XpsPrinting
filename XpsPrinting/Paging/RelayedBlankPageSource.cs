using System;

namespace XpsPrinting.Paging
{
    public class RelayedBlankPageSource : IBlankPageSource
    {
        private readonly Func<int, IBlankPage> _pageFactoryMethod;

        public RelayedBlankPageSource(Func<int, IBlankPage> pageFactoryMethod)
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