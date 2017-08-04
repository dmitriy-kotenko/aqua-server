using System;
using System.ComponentModel.DataAnnotations;

namespace AquaServer.Core.Attributes
{
	public class ValidEnumValueAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			Type enumType = value.GetType();

			bool valid = Enum.IsDefined(enumType, value);
			if (!valid)
			{
				return new ValidationResult($"{value} is not a valid value for type {enumType.Name}");
			}

			return ValidationResult.Success;
		}
	}
}
