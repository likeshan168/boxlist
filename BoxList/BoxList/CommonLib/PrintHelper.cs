using BoxList.Models;
using System;
using System.Collections.Generic;
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
            var printDialog = new PrintDialog();
            if ((bool)printDialog.ShowDialog())
            {
                printDialog.PrintVisual(element, "");
                return;
            }

            //SetPrintProperty(printDialog, element.ActualWidth, element.ActualHeight);
            
          
            var printQueue = LocalPrintServer.GetDefaultPrintQueue();

            if (printQueue != null)
            {
                printDialog.PrintQueue = printQueue;
                
                printDialog.PrintVisual(element, "");
            }
        }

        public void PrintDoc(FrameworkElement element,bool isCheckedIn)
        {
            this.isCheckedIn = isCheckedIn;

            PrintDocument printDoc = new PrintDocument();
            PaperSize paperSize = new PaperSize();

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
            paperSize.Width = (int)element.ActualWidth;
            paperSize.Height = (int)element.ActualHeight;
            printDoc.DefaultPageSettings.PaperSize = paperSize;
            printDoc.PrintPage += PrintDoc_PrintPage;
            printDoc.Print();
            
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // e.Graphics.
            e.Graphics.DrawImage(System.Drawing.Image.FromFile(isCheckedIn? @"./xps/one.png":@"./xps/two.png"),e.Graphics.VisibleClipBounds);
            e.HasMorePages = false;
        }

        /// <summary>
        /// 设置打印格式
        /// </summary>
        /// <param name="printDialog">打印文档</param>
        /// <param name="pageSize">打印纸张大小 a4</param>
        /// <param name="pageOrientation">打印方向 竖向</param>
        public void SetPrintProperty(PrintDialog printDialog, double width, double height, PageMediaSizeName pageSize = PageMediaSizeName.CreditCard, PageOrientation pageOrientation = PageOrientation.Landscape)
        {
            //PageMediaSizeName page = new PageMediaSizeName();
         
            var printTicket = printDialog.PrintTicket;
            printTicket.PageMediaType = PageMediaType.Label;
            printTicket.PageMediaSize = new PageMediaSize(width, height);
            //printTicket.PageMediaSize = new PageMediaSize(pageSize);//A4纸
            //printTicket.PageOrientation = pageOrientation;//默认竖向打印
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
}
