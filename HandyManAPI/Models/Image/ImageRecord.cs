namespace HandyManAPI.Models
{   
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ImageModel 
    {  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageModelID { get; set; }
        
        public byte[] ImageBlob {get; set;} 

        public string FileName {get; set;} 

        public OrderRecord ordersRecord {get; set;}
    }  
}