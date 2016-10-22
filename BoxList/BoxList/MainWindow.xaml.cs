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

        private bool isValid = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(code.Text))
            {

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
                    string barCode = code.Text.Trim();
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
                       new PrintHelper().PrintVisual(isCheckIn ? boxImage1 : boxImage2);
                       Console.WriteLine("打印完成");
                       Msg.Text = "打印完成";
                       code.Text = "";
                   }
                   catch (Exception ex)
                   {
                       System.Windows.MessageBox.Show(ex.Message);
                   }
               }), DispatcherPriority.Background);


        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //Uri uri = new Uri(@"C:\Users\hiand\Desktop\test\one.png");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xps");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Uri uri = new Uri(Path.Combine(path, (bool)rbtnCheckIn.IsChecked ? ConfigEntry.Instance.XpsForBaseLabel : ConfigEntry.Instance.XpsForJingDongLabel));
            Export(uri, (bool)rbtnCheckIn.IsChecked ? boxImage1 : boxImage2);


            using (XpsDocument doc = new XpsDocument(uri.LocalPath, FileAccess.Read))
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
            surface.LayoutTransform = null;

            Size size = new Size(surface.Width, surface.Height);
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            RenderTargetBitmap renderBitmap =
            new RenderTargetBitmap(
            (int)size.Width,
            (int)size.Height,
            96d,
            96d,
            PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.OpenOrCreate))
            {
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
    }
}
