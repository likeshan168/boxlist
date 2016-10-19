using BoxList.CommonLib;
using BoxList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoxList
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(code.Text))
            {
                bool isCheckIn = (bool)rbtnCheckIn.IsChecked;
                var printLabel = new DbOperation().GetPrintLabel(isCheckIn,code.Text);

                //new PrintHelper().PrintVisual(isCheckIn ? boxImage1 : boxImage2);
            }
        }

    }
}
