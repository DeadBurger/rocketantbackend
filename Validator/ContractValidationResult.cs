
using System.Collections.Generic;
using System.Linq;

namespace RocketAnt.Function
{
    public class ContractValidationResult
    {
        public List<ValidationError> Errors { get; set; }

        public bool IsValid => !Errors.Any();
    }
}