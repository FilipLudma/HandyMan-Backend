using System;
using System.Reflection.Metadata;
using HandyManAPI.Models;

namespace HandyManAPI.Inputs
{
    public class ImageAttachment
    {

        #region Public Constructor
        public ImageAttachment() { }

        #endregion Public Constructor

        #region Public properties

        public String FileName { get; set; }

        public String ImgBase64 { get; set; }

        #endregion Public properties
    }
}   