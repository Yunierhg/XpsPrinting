using System;
using System.Windows;

namespace XpsPrinting.Paging
{
    internal class TypedBlankPageSource<T> : IBlankPageSource
        where T : IBlankPage, new()
    {
        private readonly Size _pageSize;
        private readonly Action<int, T> _customPageAction;

        public TypedBlankPageSource(Size pageSize)
        {
            _pageSize = pageSize;
        }

        public TypedBlankPageSource(Size pageSize, Action<int, T> customPageAction)
        {
            _pageSize = pageSize;
            _customPageAction = customPageAction;
        }

        public IBlankPage CreateBlankPage(int pageNumber)
        {
            var result = new T
                             {
                                 PageSize = _pageSize
                             };
            if (_customPageAction != null)
                _customPageAction(pageNumber, result);
            return result;
        }
    }
}