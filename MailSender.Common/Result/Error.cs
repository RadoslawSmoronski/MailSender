namespace MailSender.Common.Result
{
    /// <summary>
    /// Represents a domain-specific error with code, description, and type.
    /// </summary>
    /// <param name="Code">Short error code identifier (e.g., NotFound, Validation).</param>
    /// <param name="Description">Human-readable error description.</param>
    /// <param name="errorType">Categorization of the error type.</param>
    public sealed record Error(string Code, string Description, ErrorType? errorType)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);

        public static Error NotFound(string description) =>
            new("NotFound", description, ErrorType.NotFound);

        public static Error Validation(string description) =>
            new("Validation", description, ErrorType.Validation);

        public static Error Conflict(string description) =>
            new("Conflict", description, ErrorType.Conflict);

        public static Error Unauthorized(string description) =>
            new("Unauthorized", description, ErrorType.Unauthorized);

        public static Error Unauthentication(string description) =>
            new("Unauthentication", description, ErrorType.Unauthentication);

        public static Error Unknown(string description) =>
            new("Unknown", description, ErrorType.Unknown);

        public static Error Failure(string description) =>
            new("Failure", description, ErrorType.Failure);
    }
}
