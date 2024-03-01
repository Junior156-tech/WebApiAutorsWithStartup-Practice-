using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresConStartup.Validations
{
    public class FirstLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) {

                return ValidationResult.Success;
            
            }

            var firstLetter = value.ToString()[0].ToString();
            if(firstLetter != firstLetter.ToUpper())
            {
                return  new ValidationResult("Ta malo");
            }

            return ValidationResult.Success;

        }
    }
}
