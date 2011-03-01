using System.Windows;
using System.Windows.Media;

namespace XpsPrinting.Formatting
{
    /// <summary>
    /// Represents mechanism that allows formatting and division of data by chunks of given size. 
    /// For example by pages. Or by columns inside a page.
    /// </summary>
    /// <remarks>
    /// Basically it is something like Enumerator except the fact that next chunk depends on previous and parameter - how much space
    /// next chunk could take. So, implementation should maintain internally some concept of a 'current position' in the data.
    /// </remarks>
    public interface IDataFormatter
    {
        /// <summary>
        /// Takes data from current position and formats it to fit into given available space. After that advances current position to the end
        /// of formatted part.
        /// </summary>
        /// <returns>Visual of requested chunk or null if the end of data reached.</returns>
        Visual GetNextPortion(Size availableSpace);

        /// <summary>
        /// Resets the current position to the beginning of the data.
        /// </summary>
        void ResetPosition();
    }
}