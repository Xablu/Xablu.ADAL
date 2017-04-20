using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace Xablu.ADAL.Tester.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        private string title = "";
        public virtual string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool NeedsRefresh = false;

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        private bool isRefreshing;
        public virtual bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                SetProperty(ref isRefreshing, value);
            }
        }

        public MvxAsyncCommand ReloadCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await ReloadData();
                });
            }
        }

        public virtual Task ReloadData()
        {
            RaisePropertyChanged(nameof(IsRefreshing));
            return Task.FromResult(0);
        }

        public async Task HandleUnexpectedException(Exception e)
        {
            await UserDialogs.Instance.AlertAsync($"Oops.. something went wrong :\n{e.ToString()}");
        }
    }
}
