using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BoxList.BusinessLib
{
    class TextChangeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            string para = (string)parameter;
            if(string.IsNullOrWhiteSpace(para))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            //调用打印机的功能
        }
    }
}
