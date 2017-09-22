namespace WebAPI.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    
        public class OrderContactOption
    {
        #region Public Constructor
        #endregion Public Constructor

        #region Public properties

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderContactOptionID { get; set; }
        public int OrdersRecordID {get; set;}
        public string ContactOption { get; set; }
        public OrdersRecord OrdersRecord {get; set;}
        
        #endregion Public properties
    }
}