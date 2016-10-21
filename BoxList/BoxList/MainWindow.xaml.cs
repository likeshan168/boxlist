using BoxList.CommonLib;
using BoxList.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BoxList
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isValid = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(code.Text))
            {

                Task.Factory.StartNew(() => {  SetDataContext(); })
                           .ContinueWith((rst) =>
                           {
                               if (isValid)
                               {
                                   PrintLabel();
                                   Console.WriteLine("打印");
                               }
                           });

                Console.WriteLine("complete!");
            }
        }

        private void SetDataContext()
        {

            Dispatcher.Invoke(new Func<bool>(() =>
            {
                try
                {
                    Console.WriteLine("获取数据...!");
                    Msg.Text = "获取数据...";
                    bool isCheckIn = (bool)rbtnCheckIn.IsChecked;
                    string barCode = code.Text;
                    var printLabel = new DbOperation().GetPrintLabel(isCheckIn, barCode);

                    if (printLabel == null)
                    {
                        Msg.Text = "没有找到对应的数据";
                        isValid = false;
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
                   
                    Msg.Text = "数据获取完成";
                    isValid = true;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    isValid = false;
                    return false;
                }
            }), DispatcherPriority.Background);
        }

        private void PrintLabel()
        {

            Dispatcher.Invoke(new Action(() =>
               {
                   try
                   {
                       Console.WriteLine("正在打印...");
                       Msg.Text = "正在打印...";
                       bool isCheckIn = (bool)rbtnCheckIn.IsChecked;
                       new PrintHelper().PrintVisual(isCheckIn ? boxImage1 : boxImage2);
                       Console.WriteLine("打印完成");
                       Msg.Text = "打印完成";
                       code.Text = "";
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show(ex.Message);
                   }
               }), DispatcherPriority.Background);


        }
    }
}
