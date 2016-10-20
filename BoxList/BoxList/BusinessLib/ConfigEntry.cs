using BoxList.CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxList.BusinessLib
{
    public class ConfigEntry
    {
        public readonly static ConfigEntry Instance = new ConfigEntry();
        private ConfigEntry()
        {
            ProductImagePath = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.PRODUCTIMAGEPATH);
            ProductImageExt = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.PRODUCTIMAGEEXT);
            BaseBarCodeImagePath = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.BASEBARCODEIMAGEPATH);
            BaseBarCodeImageExt = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.BASEBARCODEIMAGEEXT);
            JingDongBarCodeImagePath = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.JINGDONGBARCODEIMAGEPATH);
            JingDongBarCodeImageExt = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.JINGDONGBARCODEIMAGEEXT);
            Db = ConfigHelper.Instance.GetConnectionStringsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.DB);
            LabelSql = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.LABELSQL);
            JingDongLabelSql = ConfigHelper.Instance.GetAppSettingsConfigValue(Constants.BOXLISTCONFIGFILENAME, Constants.JINGDONGLABELSQL);
        }

        public string ProductImagePath { get; }
        public string ProductImageExt { get; }
        public string BaseBarCodeImagePath { get; }
        public string BaseBarCodeImageExt { get; }
        public string JingDongBarCodeImagePath { get; }
        public string JingDongBarCodeImageExt { get; }
        public string Db { get; }
        public string LabelSql { get; }
        public string JingDongLabelSql { get; }

    }
}
