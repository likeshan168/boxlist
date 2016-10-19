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

namespace BoxList.CommonLib
{
    public class PrintHelper
    {
        //public static readonly PrintHelper Instance = new PrintHelper();
        //private PrintHelper() { }

        //private const string PrintServerName = "192.168.1.99";
        //private const string PrintName = "HP LaserJet M1522 series PCL6 Class Driver";

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
            SetPrintProperty(printDialog);
            //var printQueue = SelectedPrintServer(PrintServerName);
            var printQueue = LocalPrintServer.GetDefaultPrintQueue();

            if (printQueue != null)
            {
                printDialog.PrintQueue = printQueue;
                printDialog.PrintVisual(element, "");
            }
        }

        /// <summary>
        /// 查找并获取打印机
        /// </summary>
        /// <param name="printerServerName">服务器名字</param>
        /// <param name="printerName">打印机名字</param>
        /// <returns></returns>
        public PrintQueue SelectedPrintServer(string printerServerName)
        {
            try
            {

                PrintDocument print = new PrintDocument();
                string printerName = print.PrinterSettings.PrinterName;

                var printers = PrinterSettings.InstalledPrinters;//获取本机上的所有打印机
                
                PrintServer printServer = null;

                foreach (string printer in printers)
                {
                    if (printer.Contains(printerName))
                        printServer = new PrintServer("\\\\" + printerServerName);
                }

                if (printServer == null) return null;//没有找到打印机服务器

                var printQueue = printServer.GetPrintQueue(printerName);
                return printQueue;
                

            }
            catch (Exception)
            {
                return null;//没有找到打印机
            }
        }

        /// <summary>
        /// 设置打印格式
        /// </summary>
        /// <param name="printDialog">打印文档</param>
        /// <param name="pageSize">打印纸张大小 a4</param>
        /// <param name="pageOrientation">打印方向 竖向</param>
        public void SetPrintProperty(PrintDialog printDialog, PageMediaSizeName pageSize = PageMediaSizeName.ISOA4, PageOrientation pageOrientation = PageOrientation.Portrait)
        {
            var printTicket = printDialog.PrintTicket;
            printTicket.PageMediaSize = new PageMediaSize(pageSize);//A4纸
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

    class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
    }
}
