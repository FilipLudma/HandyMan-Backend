namespace HandyManAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderRecord
    {

        #region Public Constructor
        public OrderRecord()
        {
            this.ImgAttachments = new List<ImageModel>();
        }

        #endregion Public Constructor

        #region Public properties
        public Guid OrderGuid { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactOption { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public IList<ImageModel> ImgAttachments { get; set; }

        #endregion Public properties
    }
}