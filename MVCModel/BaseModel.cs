using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCModel
{
    public abstract class BaseModel : IValidatableObject
    {
        #region Implementation of IValidatableObject

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (false) yield return new ValidationResult("", new[] { "" });
        }

        #endregion
    }
}
