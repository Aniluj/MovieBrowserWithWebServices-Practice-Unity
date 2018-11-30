using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageData : MonoBehaviour {

    [System.Serializable]
    public class Data
    {
        public string FileType;
        public string FileTypeExtension;
        public string MIMEType;
        public double JFIFVersion;
        public string ResolutionUnit;
        public int XResolution;
        public int YResolution;
        public string ProfileCMMType;
        public string ProfileVersion;
        public string ProfileClass;
        public string ColorSpaceData;
        public string ProfileConnectionSpace;
        public string ProfileDateTime;
        public string ProfileFileSignature;
        public string PrimaryPlatform;
        public string CMMFlags;
        public string DeviceManufacturer;
        public string DeviceModel;
        public string DeviceAttributes;
        public string RenderingIntent;
        public string ConnectionSpaceIlluminant;
        public string ProfileCreator;
        public int ProfileID;
        public string ProfileDescription;
        public string ProfileCopyright;
        public string MediaWhitePoint ;
        public string MediaBlackPoint ;
        public string RedMatrixColumn;
        public string GreenMatrixColumn;
        public string BlueMatrixColumn;
        public int ImageWidth;
        public int ImageHeight;
        public string EncodingProcess;
        public int BitsPerSample;
        public int ColorComponents;
        public string YCbCrSubSampling;
        public string ImageSize;
        public double Megapixels;
    }
}
