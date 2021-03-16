
namespace RocketAnt.Function
{
    public interface IContractValidator<T>
    {
        ContractValidationResult Validate(T contract);
    }
}