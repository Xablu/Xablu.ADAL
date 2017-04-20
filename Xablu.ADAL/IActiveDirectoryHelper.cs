using System.Threading.Tasks;

namespace Xablu.ADAL
{
    public interface IActiveDirectoryHelper
    {
        /// <summary>
        /// Returns the currently logged in user.
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
    }

}
