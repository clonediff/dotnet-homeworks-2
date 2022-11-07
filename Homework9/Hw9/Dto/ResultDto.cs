namespace Hw9.Dto
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

        public static ResultDto<T> Ok(T value) 
            => new ResultDto<T>(true, value, default);
        public static ResultDto<T> Error(string errorMsg) 
            => new ResultDto<T>(false, default, errorMsg);
    }
}
