using System.Windows;
using System.Windows.Controls;

namespace PhotosCategorier.UserControls
{
    /// <summary>
    /// SettingSelect.xaml 的交互逻辑
    /// </summary>
    public partial class SettingSelect : UserControl
    {
        public SettingSelect()
        {
            InitializeComponent();
        }

        public string LabelContent
        {
            get => (string)GetValue(LabelContentProperty);
            set
            {
                SetValue(LabelContentProperty, value);
                SettingLabel.Content = value;
            }
        }

        // Using a DependencyProperty as the backing store for LabelContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(string), typeof(Window));

        public ComboBox ComboBox => Combo;

        public Button OK => OKButton;
        public Button Cancel => CancelButton;
    }
}
