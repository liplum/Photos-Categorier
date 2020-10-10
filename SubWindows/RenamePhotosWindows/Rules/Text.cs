using System.Windows.Controls;

namespace PhotosCategorier.SubWindows.RenamePhotosWindows.Rules
{
    public class Text : IRule
    {
        private readonly Label label = new Label
        {
            Content = "Content:"
        };

        private readonly TextBox textBox = new TextBox
        {

        };

        private readonly Grid grid;

        public Text()
        {
            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetColumn(label, 0);
            Grid.SetColumn(textBox, 1);
            grid.Children.Add(label);
            grid.Children.Add(textBox);
        }
        public string GenerateString()
        {
            return (string)label.Content;
        }

        public Panel GetFuntionArea()
        {
            return grid;
        }
    }
}
