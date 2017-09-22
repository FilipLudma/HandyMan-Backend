namespace WebAPI.Inputs
{ 
    public class OrderDto
    {

        #region Public Constructor
        public OrderDto() { }

        #endregion Public Constructor

        #region Public properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        
        public string Address { get; set; }        
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ContactOption { get; set; }

        #endregion Public properties
    }
}