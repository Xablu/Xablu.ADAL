using System.Threading.Tasks;

namespace Plugin.Xablu.Adal.Abstractions
{
    /// <summary>
    /// Interface for Adal
    /// </summary>
    public interface IAdal
    {
        /// <summary>
        /// 
        /// </summary>
        IAdalPersistence Persistence { get; set; }

        /// <summary>
        /// Configures the ADAL settings to use for all future AD requests.
        /// </summary>
        /// <param name="configuration">A valid ADAL configuration</param>
        void Configure(AdalConfiguration configuration);

        /// <summary>
        /// Returns the currently logged in user or null if no valid login exists.
        /// </summary>
        /// <returns>The logged in user, or null if no active login exists</returns>
        Task<ActiveDirectoryUser> GetLoggedInUserAsync();

        /// <summary>
        /// Returns the currently logged in user and shows the login page
        /// if no user was logged in yet.
        /// </summary>
        /// <returns>The logged in user</returns>
        Task<ActiveDirectoryUser> EnsureUserLoggedIn();

        /// <summary>
        /// This task can be awaited in an synchronous thread to ensure that
        /// the login page is not being shown anymore. Note that this will not
        /// ensure that a valid user is logged in.
        /// </summary>
        /// <returns></returns>
        Task WaitForLogin();

        Task Logout();
    }
}
