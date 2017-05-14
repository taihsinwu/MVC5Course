using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MVC5Course.Models.ViewModels
{
    public class ProductList_SearchVM : IValidatableObject
    {
        public ProductList_SearchVM()
        {
            this.queryStockFrom = 0;
            this.queryStockTo = 9999;
        }

        public string queryName { get; set; }
        // public int queryPrice { get; set; }
        public int queryStockFrom { get; set; }
        public int queryStockTo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.queryStockTo < this.queryStockFrom)
            {
                yield return new ValidationResult("庫存數查詢條件錯誤", new string[] { "queryStockFrom","queryStockTo" });
            }

        }
    }


}