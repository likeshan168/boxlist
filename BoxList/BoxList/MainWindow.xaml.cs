using BoxList.CommonLib;
using BoxList.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
                //isCheckIn = (bool)rbtnCheckIn.IsChecked;
                //barCode = code.Text;

                Task.Factory.StartNew(() => { return SetDataContext(); })
                    .ContinueWith((rst) =>
                    {
                        if (rst.Result)
                        {
                            //PrintLabel();
                        }
                    });

                Console.WriteLine("complete!");
            }
        }

        private bool SetDataContext()
        {
            Dispatcher.BeginInvoke(new Func<bool>(() =>
            {
                Console.WriteLine("获取数据...!");
                Msg.Text = "获取数据...";
                //Thread.Sleep(4000);
                bool isCheckIn = (bool)rbtnCheckIn.IsChecked;
                string barCode = code.Text;
                var printLabel = new DbOperation().GetPrintLabel(isCheckIn, barCode);

                if (printLabel == null)
                {
                    return false;
                }

                if (isCheckIn)
                {
                    boxImage1.DataContext = printLabel;
                }
                else
                {
                    boxImage2.DataContext = printLabel;
                }
                Console.WriteLine("数据获取完成");
                Msg.Text = "数据获取完成";

                return true;
            }));

            return true;
        }

        private void PrintLabel()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("正在打印...");
                Msg.Text = "正在打印...";
                bool isCheckIn = (bool)rbtnCheckIn.IsChecked;
                new PrintHelper().PrintVisual(isCheckIn ? boxImage1 : boxImage2);
                Console.WriteLine("打印完成");
                Msg.Text = "打印完成";
                Console.WriteLine("扫描打印");
                Msg.Text = "扫描打印";
            }));
        }
    }
}
