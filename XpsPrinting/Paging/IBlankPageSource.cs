namespace XpsPrinting.Paging
{
    public interface IBlankPageSource
    {
        IBlankPage CreateBlankPage(int pageNumber);
    }
}