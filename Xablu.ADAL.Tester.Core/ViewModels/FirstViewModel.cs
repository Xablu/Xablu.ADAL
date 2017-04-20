using Acr.UserDialogs;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xablu.ADAL.Exceptions;

namespace Xablu.ADAL.Tester.Core.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {
        private IActiveDirectoryHelper adHelper { get; }
        
        public FirstViewModel(IActiveDirectoryHelper adHelper)
        {
            this.adHelper = adHelper;
            Title = "Login";
        }
        
        public bool ValidatingLogin { get; private set; }

        public async Task<bool> ValidateLogin()
        {
            
            if (!CrossConnectivity.Current.IsConnected)
            {
                await UserDialogs.Instance.AlertAsync("Connection error");
                Close(this);
                return false;
            }
            
            IsLoading = !IsRefreshing;
            
            try
            {
                ValidatingLogin = true;
                var user = await adHelper.EnsureUserLoggedIn();
                Hello = $"Hello {user.UserInfo.GivenName}";
                return user != null;
            }
            catch (LoginCanceledException)
            {
                ValidatingLogin = false;
                IsLoading = false;
                await UserDialogs.Instance.AlertAsync("Login was canceled");
                Close(this);
                return false;
            }
            catch (ConnectivityException e)
            {
                ValidatingLogin = false;
                IsLoading = false;
                await UserDialogs.Instance.AlertAsync(e.Message);
                return false;
            }
            catch (LoginFailedException e)
            {
                var inner = e.InnerException as Microsoft.IdentityModel.Clients.ActiveDirectory.AdalServiceException;
                await HandleUnexpectedException(new Exception($"AD login error:\n\nMessage: {e.Message}\nErrorCode: {inner?.ErrorCode}\nServiceErrorCodes: {inner?.ServiceErrorCodes}\nStatusCode: {inner?.StatusCode}\n\n", e));
                return false;
            }
            catch (Exception e)
            {
                ValidatingLogin = false;
                IsLoading = false;
                await HandleUnexpectedException(e);
                return false;
            }
            finally
            {
                ValidatingLogin = false;
                IsLoading = false;
            }
        }

        public async Task Init()
        {
            IsLoading = !IsRefreshing;

            try
            {
                if (!await ValidateLogin())
                {
                    NeedsRefresh = true;
                    return;
                }
                Title = "FirstViewModel";
            }
            catch (Exception e)
            {
                IsLoading = false;
                await HandleUnexpectedException(e);
            }
            finally
            {
                IsLoading = false;
            }
        }


        private string _hello = "Hello MvvmCross";
        public string Hello
        {
            get { return _hello; }
            set { SetProperty(ref _hello, value); }
        }
    }
}
