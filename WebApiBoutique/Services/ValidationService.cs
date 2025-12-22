using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Services
{
    public class ValidationService
    {
        public static (bool IsValid, List<string> Errors) ValidateModel<T>(T model) where T : class
        {
            var context = new ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);
            
            var errors = results.Select(r => r.ErrorMessage ?? "Validation error").ToList();
            return (isValid, errors);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && 
                   phone.Length >= 10 && 
                   phone.All(char.IsDigit);
        }

        public static bool IsValidMeasurement(decimal value, decimal min = 0, decimal max = 200)
        {
            return value >= min && value <= max;
        }

        public static bool IsValidAmount(decimal amount)
        {
            return amount > 0 && amount <= 999999.99m;
        }
    }
}