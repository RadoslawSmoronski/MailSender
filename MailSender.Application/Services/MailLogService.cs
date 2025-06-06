using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.Models;
using MailSender.Infrastructure.Database.Repository;

namespace MailSender.Application.Services
{
    /// <summary>
    /// Service responsible for managing mail logs.
    /// </summary>
    public class MailLogService : IMailLogService
    {
        private readonly IRepository<MailLog> _repository;

        /// <summary>
        /// Initializes a new instance of <see cref="MailLogService"/>.
        /// </summary>
        /// <param name="repository">Generic repository for <see cref="MailLog"/> entities.</param>
        public MailLogService(IRepository<MailLog> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all mail logs associated with a specific application ID.
        /// </summary>
        /// <param name="AppId">The application ID to filter logs by.</param>
        /// <returns>A <see cref="Result{List{MailLog}}"/> containing the list of matching mail logs,
        /// or an error result if retrieval fails.</returns>
        public async Task<Result<List<MailLog>>> GetLogs(string AppId)
        {
            try
            {
                var result = await _repository.WhereAsync(x => x.AppId == AppId);
                return result.ToList();
            }
            catch (Exception ex)
            {
                return Error.Unknown(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new mail log entry asynchronously.
        /// </summary>
        /// <param name="mailLog">The mail log entity to add.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the operation.</returns>
        public async Task<Result> LogAsync(MailLog mailLog)
        {
            try
            {
                await _repository.AddAsync(mailLog);
                await _repository.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Unknown(ex.Message);
            }
        }
    }
}
