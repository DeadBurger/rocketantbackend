using System.Net;
using System.Net.Http;
using System;
using System.Collections.Generic;

namespace RocketAnt.Function
{
    public abstract class ContractBaseValidator<T> : IContractValidator<T>
    {
        public List<ValidationError> ValidationErrors { get; } = new List<ValidationError>();

        public ContractValidationResult Validate(T contract)
        {
            ValidateInternal(contract);
            return new ContractValidationResult()
            {
                Errors = ValidationErrors
            };
        }

        protected abstract void ValidateInternal(T contract);

        protected void IsNotNull(T contract, Func<T, object> selector)
        {
            var methodName = selector.Method.Name;
            var obj = selector(contract);
            if (obj == null)
                ValidationErrors.Add(new ValidationError("fieldname", "Field is null"));
        }
        protected void IsNotNull(T contract)
        {
            if (contract == null)
                ValidationErrors.Add(new ValidationError("Field is null"));
        }
    }
}

