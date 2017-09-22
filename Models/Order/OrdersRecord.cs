namespace WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrdersRecord
    {

        #region Public Constructor
        public OrdersRecord()
        {
            // this.OrderCategory = new List<OrderCategory>();
            // this.OrderContactOptions =new List<OrderContactOption>();
        }

        #endregion Public Constructor

        #region Public properties
        public Guid OrderGuid { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }


        public string OrderAddress { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }

        #endregion Public properties
    }
}