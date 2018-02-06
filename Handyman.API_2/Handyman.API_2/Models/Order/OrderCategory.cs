namespace WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderCategory
    {
        #region Public Constructor

        public OrderCategory()
        {
            // List<OrderSubCategory> orderSubCategory = new List<OrderSubCategory>();
        }

        #endregion Public Constructor

        #region Public properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderCategoryID { get; set; }
        public string CategoryName { get; set; }
        public int CategoryType { get; set; }

        public ICollection<OrderSubCategory> OrderSubCategories { get; set; }

        #endregion Public properties
    }
}