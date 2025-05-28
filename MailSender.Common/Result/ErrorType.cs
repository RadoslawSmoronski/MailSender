namespace MailSender.Common.Result
{
    /// <summary>
    /// Enumeration of possible error types for categorizing domain and application errors.
    /// </summary>
    public enum ErrorType
    {
        Unknown,
        Validation,
        Unauthorized,
        NotFound,
        Conflict,
        Failure
    }
}
