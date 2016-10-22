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
