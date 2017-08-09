using BoxList.BusinessLib;
using BoxList.CommonLib;
using BoxList.ViewModels;
using System;
using System.IO;
using System.IO.Packaging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace BoxList
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string barCode = string.Empty;
        private bool isValid = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            int codeLength;
            if (!int.TryParse(CodeLength.Text, out codeLength))
            {
                MessageBox.Show("条形码的位数只能为数字！");
                return;
            }
            
            if (!string.IsNullOrWhiteSpace(code.Text) && (code.Text.Length == codeLength))
            {
                barCode = code.Text;
                Task.Factory.StartNew(() => { SetDataContext(); })
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
                    
                    var printLabel = new DbOperation().GetPrintLabel(isCheckIn, barCode);

                    if (printLabel == null)
                    {
                        Msg.Text = "没有找到对应的数据";
                        isValid = false;
                        code.SelectAll();
                        return false;
                    }

                    if (isCheckIn)
                    {
                        boxImage1.DataContext = printLabel;
                        bigImage.DataContext = printLabel;
                    }
                    else
                    {
                        bigImage.DataContext = printLabel;
                        boxImage2.DataContext = printLabel;
                    }

                    Msg.Text = "数据获取完成";
                    isValid = true;
                    return true;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
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

                       //讲表格保存为xps文件已供打印和预览
                       string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xps");
                       if (!Directory.Exists(path))
                       {
                           Directory.CreateDirectory(path);
                       }

                       Uri uri = new Uri(Path.Combine(path, (bool)rbtnCheckIn.IsChecked ? ConfigEntry.Instance.XpsForBaseLabel : ConfigEntry.Instance.XpsForJingDongLabel));
                       //Export(uri, isCheckIn ? boxImage1 : boxImage2);
                       //ExportToPng(new Uri(Path.Combine(path, isCheckIn ? "one.png" : "two.png")), isCheckIn ? boxImage1 : boxImage2);

                       //第一种方式
                       new PrintHelper().PrintVisual(isCheckIn ? boxImage1 : boxImage2);
                       //第二种方式
                       //new PrintHelper().PrintDoc(isCheckIn ? boxImage1 : boxImage2, isCheckIn);

                       Console.WriteLine("打印完成");
                       Msg.Text = "打印完成";
                       code.SelectAll();
                   }
                   catch (Exception ex)
                   {
                       System.Windows.MessageBox.Show(ex.Message);
                   }
               }), DispatcherPriority.Background);


        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xps", (bool)rbtnCheckIn.IsChecked ? ConfigEntry.Instance.XpsForBaseLabel : ConfigEntry.Instance.XpsForJingDongLabel);

            using (XpsDocument doc = new XpsDocument(path, FileAccess.Read))
            {
                DocumentViewer docV = new DocumentViewer();
                docV.Document = doc.GetFixedDocumentSequence();

                Window page = new Window();
                page.Content = docV;
                page.Show();

                page = null;

                docV = null;
            }
        }

        public void ExportToPng(Uri path, Grid surface)
        {
            if (path == null) return;

            Transform transform = surface.LayoutTransform;
            surface.UseLayoutRounding = true;
            surface.LayoutTransform = null;

            Size size = new Size(surface.Width, surface.Height);
            surface.Measure(size);
            surface.Arrange(new Rect(size));


            //PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.Transform.x


            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.OpenOrCreate))
            {
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
                RenderTargetBitmap renderBitmap =
                   new RenderTargetBitmap(
                   (int)size.Width,
                   (int)size.Height,
                   graphics.DpiX,
                   graphics.DpiY,
                   PixelFormats.Pbgra32);
                renderBitmap.Render(surface);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
            surface.LayoutTransform = transform;

        }

        public void Export(Uri path, Grid surface)
        {
            if (path == null) return;

            Transform transform = surface.LayoutTransform;
            surface.LayoutTransform = null;

            Size size = new Size(surface.Width, surface.Height);
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            if (File.Exists(path.LocalPath))
                File.Delete(path.LocalPath);
            using (Package package = Package.Open(path.LocalPath, FileMode.OpenOrCreate))
            {
                XpsDocument doc = new XpsDocument(package);

                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                writer.Write(surface);
                doc.Close();
                package.Close();
                surface.LayoutTransform = transform;
            }
        }

        private void code_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                barCode = code.Text;
                Task.Factory.StartNew(() => { SetDataContext(); })
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
    }
}
