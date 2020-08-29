using System.Windows;
using System.Windows.Controls;

namespace PhotosCategorier.Main
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
            get { return (string)GetValue(LabelContentProperty); }
            set
            {
                SetValue(LabelContentProperty, value);
                this.SettingLabel.Content = value;
            }
        }

        // Using a DependencyProperty as the backing store for LabelContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(string), typeof(Window));

        public ComboBox ComboBox
        {
            get => this.Combo;
        }

        public Button OK { get => this.OK; }
        public Button Cancel { get => this.Cancel; }
    }
}
