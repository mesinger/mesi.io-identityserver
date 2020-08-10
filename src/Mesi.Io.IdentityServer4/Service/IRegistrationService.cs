using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mesi.Io.IdentityServer4.Service
{
    /// <summary>
    /// Performs registration operations for new users
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Trys to register a new user and returns a result object
        /// </summary>
        /// <param name="request">holds data about a user</param>
        /// <returns></returns>
        Task<RegistrationResult> RegisterUser(RegistrationRequest request);
    }

    /// <summary>
    /// Request model that holds information about a user
    /// </summary>
    public class RegistrationRequest
    {
        public RegistrationRequest(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
    }

    /// <summary>
    /// Result object that holds information about a registration attempt
    /// </summary>
    public class RegistrationResult
    {
        protected RegistrationResult(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }
        
        public static RegistrationResult SuccessFul() => new RegistrationResult(true, Enumerable.Empty<string>());
        
        public static RegistrationResult Failure(IEnumerable<string> errors) => new RegistrationResult(false, errors);

        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }
    }
}