
using System.Collections.Generic;
using System.Linq;

namespace RocketAnt.Validator
{
    public class ContractValidationResult
    {
        public List<ValidationError> Errors { get; set; }

        public bool IsValid => !Errors.Any();
    }
}