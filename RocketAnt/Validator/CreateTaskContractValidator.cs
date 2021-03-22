
using System;

namespace RocketAnt.Validator
{
    public class CreateTaskContractValidator : ContractBaseValidator<CreateTaskContract>
    {
        override protected void ValidateInternal(CreateTaskContract contract)
        {
            IsNotNull(contract);
            IsNotNull(contract, o => o.NumOfSteps);
        }
    }
}