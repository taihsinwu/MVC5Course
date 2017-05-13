using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ValidAttributes
{
    public class 商品名稱需包含安安Attribute : DataTypeAttribute
    {
        public 商品名稱需包含安安Attribute() : base(DataType.Text)
        {

        }

        public override bool IsValid(object value)
        {
            var str = (string)value;
            if (str.Contains("安安")) return true;

            else return false;
        }
    }
}