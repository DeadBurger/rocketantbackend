
namespace RocketAnt.Validator
{
    public interface IContractValidator<T>
    {
        ContractValidationResult Validate(T contract);
    }
}