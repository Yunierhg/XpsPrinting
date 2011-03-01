using System.Windows.Documents;

namespace TestApp._3
{
    public partial class TitledBlockContentTemplate : FlowDocument
    {
        public TitledBlockContentTemplate()
        {
            InitializeComponent();
        }

        public string Title
        {
            set { runTitle.Text = value; }
        }

        public void AppendBlock(Block block)
        {
            sectPlaceholder.Blocks.Add(block);
        }
    }
}