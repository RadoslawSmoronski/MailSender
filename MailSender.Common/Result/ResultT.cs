namespace MailSender.Common.Result
{
    /// <summary>
    /// Represents the result of an operation that returns a value on success or an error on failure.
    /// </summary>
    /// <typeparam name="T">Type of the value returned on success.</typeparam>
    public class Result<T> : Result
    {
        private readonly T? _value;

        private Result(T value)
            : base(true, Error.None)
        {
            _value = value;
        }

        private Result(Error error)
            : base(false, error)
        {
        }

        /// <summary>
        /// The value of the result if successful; throws if result is a failure.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when accessing value on a failure result.</exception>
        public T Value =>
            IsSuccess
                ? _value!
                : throw new InvalidOperationException("No value for failure result.");

        public static implicit operator Result<T>(Error error) => new(error);

        public static implicit operator Result<T>(T value) => new(value);

        /// <summary>Creates a successful result with a return value.</summary>
        public static Result<T> Success(T value) => new(value);

        /// <summary>Creates a failed result with the specified error.</summary>
        public static new Result<T> Failure(Error error) => new(error);
    }
}
