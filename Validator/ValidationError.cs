namespace RocketAnt.Validator
{
    public class ValidationError
    {
        public ValidationError(string error)
        {
            Error = error;
        }

        public ValidationError(string fieldName, string error)
        {
            FieldName = fieldName;
            Error = error;
        }

        public string FieldName { get; set; }
        public string Error { get; set; }
    }
}