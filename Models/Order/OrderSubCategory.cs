namespace WebAPI.Models
{   
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderSubCategory  
    {  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderSubCategoryID { get; set; }
        
        public string SubCategoryName {get; set;} 

        public OrderCategory OrderCategory {get; set;} 
    }  
}