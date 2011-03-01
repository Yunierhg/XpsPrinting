using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Paging
{
    /// <summary>
    /// Represents work piece of a page.
    /// </summary>
    /// <remarks>
    /// Despite the fact of similarity to IPage interface the purpose of these interfaces are different. IPage represents ready-to-print
    /// readonly page abstraction, while IBlankPage allows to adjust PageSize and get modified boxes and Visual for this PageSize.
    /// 
    /// So there's no 'IS A' relationship and no inheritance.
    /// </remarks>
    public interface IBlankPage
    {
        /// <summary>
        /// This property represents physical page size in device-independent points (DIP). 1 DIP = 1/96 inch. This is the only
        /// assignable element on the page. Everything else determined from PageSize on each concrete BlankPage class.
        /// </summary>
        Size PageSize { get; set; }
        
        /// <summary>
        /// This rect specifies region on the page that will contain printed elements. Essentially it is all page except margins.
        /// </summary>
        Rect ContentBox { get; }

        /// <summary>
        /// This rect specifies region on the page that will contain real data chunks. Thus it is part of the ContentBox rect except
        /// service regions for footers, headers etc.
        /// </summary>
        Rect DataContentBox { get; }

        /// <summary>
        /// When PageSize is set correctly this property allows client to obtain concrete representation of the page.
        /// When rendered returned Visual should be equal in size to PageSize was set (or default if it wasn't set).
        /// </summary>
        Visual PageVisual { get; }
    }
}