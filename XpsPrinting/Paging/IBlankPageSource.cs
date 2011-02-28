namespace XpsPrinting.Paging
{
    /// <summary>
    /// Incapsulates logic of getting different page templates.
    /// </summary>
    /// 
    /// <remarks>
    /// For simple use cases where single page template could be used for entire document it is possible to use just one template you could use
    /// <see cref="RelayedBlankPageSource{T}"/>.
    /// For more complicated cases logic could be more complex and making just single page template class that knows about entire document
    /// structure and which page should be used where could increase coupling without reasons.
    /// 
    /// For example imagine that title page (first) should be formatted in separate way with its own page template or multipart document with some
    /// pages in landscape and some in portrait.
    /// </remarks>
    public interface IBlankPageSource
    {
        IBlankPage CreateBlankPage(int pageNumber);
    }
}