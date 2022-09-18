using System.ComponentModel.DataAnnotations;

namespace WEBUI.Models
{
    [AttributeUsage(
        validOn: AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public class SizeValidator : ValidationAttribute
    {
        public SizeValidator(long sizeInBytes)
        {
            this.SizeInBytes = sizeInBytes;
        }

        public long SizeInBytes { get; private set; }

        public override bool IsValid(object value)
        {
            bool isValid = false;

            if (value is IFormFile file)
            {
                isValid = file.Length <= this.SizeInBytes;
            }

            return isValid;
        }
    }
}
