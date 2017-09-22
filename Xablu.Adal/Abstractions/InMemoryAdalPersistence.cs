using System.Threading.Tasks;

namespace Plugin.Xablu.Adal.Abstractions
{
    internal class InMemoryAdalPersistence : IAdalPersistence
    {
        private string _userId;
        private byte[] _data;

        public Task PersistTokenCache(byte[] data)
        {
            _data = data;
            return Task.FromResult(0);
        }

        public Task PersistUserId(string userId)
        {
            _userId = userId;
            return Task.FromResult(0);
        }

        public Task<byte[]> RetrieveTokenCache()
        {
            return Task.FromResult(_data);
        }

        public Task<string> RetrieveUserId()
        {
            return Task.FromResult(_userId);
        }

        public Task RemoveTokenCache()
        {
            _data = null;
            return Task.FromResult(0);
        }

        public Task RemoveUserId()
        {
            _userId = null;
            return Task.FromResult(0);
        }
    }
}
