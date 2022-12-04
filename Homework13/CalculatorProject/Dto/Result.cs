namespace CalculatorProject.Dto
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public T Result { get; set; }

        protected ResultDto(bool success, T value, string errorMsg)
        {
            IsSuccess = success;
            ErrorMessage = errorMsg;
            Result = value;
        }

        protected ResultDto() { }
    }
}
