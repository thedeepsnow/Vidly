using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class MustNotContainHAYAsExampleCustomAtt : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            // this gives us access to the customer object (including other properties we might want to include in the validation logic.
            Customer cust = (Customer)validationContext.ObjectInstance;

            if (cust.Name == null)
            {
                return ValidationResult.Success; // the required attribute will catch this.
                // >>>
            }

            if (cust.Name.ToUpper().Contains("HAY") == true)
            {
                return new ValidationResult("Name must not contain HAY!");
                //>>>
            }

            return ValidationResult.Success;
            // >>>


            //return base.IsValid(value, validationContext);
        }
    }
}