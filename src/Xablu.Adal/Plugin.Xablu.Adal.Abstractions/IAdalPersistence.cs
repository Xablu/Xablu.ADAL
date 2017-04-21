using System.Threading.Tasks;

namespace Plugin.Xablu.Adal.Abstractions
{
    public interface IAdalPersistence
    {
        Task<byte[]> RetrieveTokenCache();
        Task PersistTokenCache(byte[] data);
        Task RemoveTokenCache();

        Task<string> RetrieveUserId();
        Task PersistUserId(string userId);
        Task RemoveUserId();

    }
}
