using BoxList.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BoxList.CommonLib
{
    public class PrintHelper
    {
        private bool isCheckedIn = true;
        public PrintHelper()
        {
        }

        /// <summary>
        /// 打印控件
        /// </summary>
        /// <param name="element"></param>
        public void PrintVisual(FrameworkElement element)
        {
            try
            {
                var printDialog = new PrintDialog();
                //if ((bool)printDialog.ShowDialog())
                //{
                //    printDialog.PrintVisual(element, "");
                //    return;
                //}

                SetPrintProperty(printDialog, element.Width-1, element.Height);


                var printQueue = LocalPrintServer.GetDefaultPrintQueue();

                if (printQueue != null)
                {
                    printDialog.PrintQueue = printQueue;
                    //new PrintDirect(LocalPrintServer.GetDefaultPrintQueue().Name).PrintESC(0);
                    printDialog.PrintVisual(element, "");


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PrintDoc(FrameworkElement element, bool isCheckedIn)
        {
            this.isCheckedIn = isCheckedIn;

            PrintDocument printDoc = new PrintDocument();
            PaperSize paperSize = new PaperSize();
            printDoc.OriginAtMargins = true;
            //设置页边距
            printDoc.PrinterSettings.DefaultPageSettings.Margins.Left = 0;
            printDoc.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
            printDoc.PrinterSettings.DefaultPageSettings.Margins.Right = 0;
            printDoc.PrinterSettings.DefaultPageSettings.Margins.Bottom = 0;
            //设置尺寸大小，如不设置默认是A4纸
            //A4纸的尺寸是210mm×297mm，
            //当你设定的分辨率是72像素/英寸时，A4纸的尺寸的图像的像素是595×842
            //当你设定的分辨率是150像素/英寸时，A4纸的尺寸的图像的像素是1240×1754
            //当你设定的分辨率是300像素/英寸时，A4纸的尺寸的图像的像素是2479×3508，

            paperSize.RawKind = 0;
            paperSize.Width = (int)(70 / 25.4 * 100);
            paperSize.Height = (int)(40 / 25.4 * 100);

            printDoc.DefaultPageSettings.PaperSize = new PaperSize("custom", (int)(70 / 25.4 * 100), (int)(40 / 25.4 * 100));
            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.PrintPage += PrintDoc_PrintPage;
            printDoc.Print();



        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // e.Graphics.
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(isCheckedIn ? @"./xps/one.oxps" : @"./xps/two.png"))
            {
                //e.Graphics.RotateTransform(-90.0F);
                //e.Graphics.TranslateTransform(0, 0, MatrixOrder.Append);
                e.Graphics.DrawImage(img, e.Graphics.VisibleClipBounds);
                //e.Graphics.DrawString("hello",new Font("宋体",12f),System.Drawing.Brushes.Red,new PointF(0,0));
                //e.Graphics.DrawImage(img, new RectangleF(0, 0, 264, 151));
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// 设置打印格式
        /// </summary>
        /// <param name="printDialog">打印文档</param>
        /// <param name="pageSize">打印纸张大小 a4</param>
        /// <param name="pageOrientation">打印方向 竖向</param>
        public void SetPrintProperty(PrintDialog printDialog, double width, double height, PageMediaSizeName pageSize = PageMediaSizeName.CreditCard, PageOrientation pageOrientation = PageOrientation.ReverseLandscape)
        {
            //PageMediaSizeName page = new PageMediaSizeName();

            var printTicket = printDialog.PrintTicket;

            printTicket.PageMediaType = PageMediaType.AutoSelect;
            printTicket.PageMediaSize = new PageMediaSize(width, height);

            //printTicket.PageMediaSize = new PageMediaSize(pageSize);//A4纸
            printTicket.PageOrientation = pageOrientation;//默认竖向打印
        }

        public List<string> GetLocalPrinters()
        {
            var printers = PrinterSettings.InstalledPrinters;
            var printLst = new List<string>();
            foreach (string printer in printers)
            {
                if (printLst.Contains(printer))
                    continue;
                printLst.Add(printer);
            }

            return printLst;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDataType;
    }

    public class PrintDirect
    {
        private string PrintPort { get; set; }
        public PrintDirect(string port)
        {
            this.PrintPort = port;
        }

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long ClosePrinter(IntPtr hPrinter);

        public void PrintESC(int type)
        {
            System.IntPtr lhPrinter = new System.IntPtr();
            DOCINFO di = new DOCINFO();
            int pcWritten = 0;
            di.pDocName = "进退纸Document";
            di.pDataType = "RAW";

            try
            {
                PrintDirect.OpenPrinter(this.PrintPort, ref lhPrinter, 0);
                if (lhPrinter == IntPtr.Zero)
                {
                    return;
                }
                PrintDirect.StartDocPrinter(lhPrinter, 1, ref di);
                PrintDirect.StartPagePrinter(lhPrinter);
                string send = string.Empty;
                for (int j = 0; j < 3; j++)
                {
                    switch (type)
                    {
                        case 0:
                            send = "" + (char)(27) + (char)(64) + (char)(27) + 'j' + (char)(180);//退纸
                            break;
                        case 1:
                            send = "" + (char)(27) + (char)(64) + (char)(27) + 'J' + (char)(180);//进纸
                            break;
                        case 2:
                            send = "" + (char)(27) + (char)(64) + (char)(12);   //换行（未测试）
                            break;
                        default:
                            send = "" + (char)(27) + (char)(64) + (char)(12);   //换行（未测试）
                            break;
                    }
                    byte[] buf = new byte[80];
                    for (int i = 0; i < send.Length; i++)
                    {
                        buf[i] = (byte)send[i];
                    }
                    PrintDirect.WritePrinter(lhPrinter, send, send.Length, ref pcWritten);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally
            {
                if (lhPrinter != IntPtr.Zero)
                {
                    PrintDirect.EndPagePrinter(lhPrinter);
                    PrintDirect.EndDocPrinter(lhPrinter);
                    PrintDirect.ClosePrinter(lhPrinter);
                }
            }
        }
    }
}
