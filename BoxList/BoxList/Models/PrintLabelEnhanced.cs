using System.ComponentModel;

namespace BoxList.Models
{
    public class PrintLabelEnhanced : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string productImagePath;
        public string 鞋图名称
        {
            get { return productImagePath; }
            set
            {
                productImagePath = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("鞋图名称"));
                }
            }
        }

        private string chineseName;
        public string 中文名称
        {
            get { return chineseName; }
            set
            {
                chineseName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("中文名称"));
                }
            }
        }

        private string englishName;
        public string 英文名称
        {
            get { return englishName; }
            set
            {
                englishName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("英文名称"));
                }
            }
        }

        private string productNo;
        public string 货号
        {
            get { return productNo; }
            set
            {
                productNo = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("货号"));
                }
            }
        }


        private string chineseColrName;
        public string 颜色
        {
            get { return chineseColrName; }
            set
            {
                chineseColrName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("颜色"));
                }
            }
        }

        private string englishColorName;
        public string 英文颜色
        {
            get { return englishColorName; }
            set
            {
                englishColorName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("英文颜色"));
                }
            }
        }

        private string aus;
        public string AUS
        {
            get { return aus; }
            set
            {
                aus = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("AUS"));
                }
            }
        }
        private string eu;
        public string EU
        {
            get { return eu; }
            set
            {
                eu = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("EU"));
                }
            }
        }
        private string usa;
        public string USA
        {
            get { return usa; }
            set
            {
                usa = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("USA"));
                }
            }
        }

        private string cm;
        public string CM
        {
            get { return cm; }
            set
            {
                cm = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CM"));
                }
            }
        }
        private string inches;
        public string Inches
        {
            get { return inches; }
            set
            {
                inches = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Inches"));
                }
            }
        }

        private string barCode;
        public string 条形码
        {
            get { return barCode; }
            set
            {
                barCode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("条形码"));
                }
            }
        }

        private string jingDongCode;
        public string 京东码
        {
            get { return jingDongCode; }
            set
            {
                jingDongCode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("京东码"));
                }
            }
        }
    }
}
