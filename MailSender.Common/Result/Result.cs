namespace MailSender.Common.Result
{
    /// <summary>
    /// Represents the result of an operation that can either succeed or fail with an error.
    /// </summary>
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>Indicates whether the result represents a successful operation.</summary>
        public bool IsSuccess { get; }

        /// <summary>Indicates whether the result represents a failure.</summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>Error associated with the failure. If successful, returns <see cref="Error.None"/>.</summary>
        public Error Error { get; }

        public static implicit operator Result(Error error) => new(false, error);

        /// <summary>Creates a successful result without a return value.</summary>
        public static Result Success() => new(true, Error.None);

        /// <summary>Creates a failed result with the specified error.</summary>
        public static Result Failure(Error error) => new(false, error);
    }
}
