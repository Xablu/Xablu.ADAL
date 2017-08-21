using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Akavache;
using Plugin.Xablu.Adal.Abstractions;

namespace Sample
{
    public class LoginPersistence : IAdalPersistence
    {
        private const string UserKey = "userId", TokenKey = "tokenCache";

        public async Task PersistTokenCache(byte[] data)
        {
            await BlobCache.Secure.InsertObject<byte[]>(TokenKey, data).ToTask().ConfigureAwait(false);
        }

        public async Task PersistUserId(string userId)
        {
            await BlobCache.Secure.InsertObject(UserKey, userId).ToTask().ConfigureAwait(false);
        }

        public async Task RemoveTokenCache()
        {
            await BlobCache.Secure.Invalidate(TokenKey).ToTask().ConfigureAwait(false);
        }

        public async Task RemoveUserId()
        {
            await BlobCache.Secure.Invalidate(UserKey).ToTask().ConfigureAwait(false);
        }

        public async Task<byte[]> RetrieveTokenCache()
        {
            return await BlobCache.Secure.GetObject<byte[]>(TokenKey).Catch(Observable.Return(new byte[0]));
        }

        public async Task<string> RetrieveUserId()
        {
            return await BlobCache.Secure.GetObject<string>(UserKey).Catch(Observable.Return(string.Empty));
        }
    }
}
