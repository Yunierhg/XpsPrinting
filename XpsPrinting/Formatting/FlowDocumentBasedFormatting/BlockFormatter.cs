using System;
using System.Windows.Documents;

namespace XpsPrinting.Formatting.FlowDocumentBasedFormatting
{
    public interface IBlockFormatter
    {
        Block Format(double width);
    }

    public class RelayBlockFormatter : IBlockFormatter
    {
        private readonly Func<double, Block> _formatter;

        public RelayBlockFormatter(Func<double, Block> formatter)
        {
            if (formatter == null) throw new ArgumentNullException("formatter");
            _formatter = formatter;
        }

        public Block Format(double width)
        {
            return _formatter(width);
        }
    }
}