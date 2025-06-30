using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Services
{
    public class Navigator : INavigator
    {
        private readonly SemaphoreSlim _singleThreadSemaphore = new SemaphoreSlim(1, 1);
        private INavigation _nav;

        public INavigation Nav
        {
            get => _nav ?? Application.Current.MainPage.Navigation;
            set => _nav = value;
        }

        public IReadOnlyList<Page> ModalStack => Nav.ModalStack;
        public IReadOnlyList<Page> NavigationStack => Nav.NavigationStack;

        /// <summary>
        /// If multiple requests are launched, then only the first one is taken into account.
        /// </summary>
        public async void ShowLogin()
        {
            try
            {
                await _singleThreadSemaphore.WaitAsync().ConfigureAwait(false);
                if (Nav.ModalStack.LastOrDefault() is LoginPage)
                {
                    return;
                }

                await Nav.PushModalAsync(new LoginPage());
            }
            finally
            {
                _singleThreadSemaphore.Release();
            }
        }

        public async Task PopModalStackAsync(bool animated = true)
        {
            try
            {
                await _singleThreadSemaphore.WaitAsync().ConfigureAwait(false);
                var modalCount = ModalStack.Count;
                for (var i = 0; i < modalCount; i++)
                {
                    await PopModalAsync(i == 0 && animated);
                }
            }
            finally
            {
                _singleThreadSemaphore.Release();
            }
        }

        public void InsertPageBefore(Page page, Page before)
        {
            Nav.InsertPageBefore(page, before);
        }

        public async Task<Page> PopAsync()
        {
            return await Nav.PopAsync();
        }

        public async Task<Page> PopAsync(bool animated)
        {
            return await Nav.PopAsync(animated);
        }

        public async Task<Page> PopModalAsync()
        {
            return await Nav.PopModalAsync();
        }

        public async Task<Page> PopModalAsync(bool animated)
        {
            return await Nav.PopModalAsync(animated);
        }

        public async Task PopToRootAsync()
        {
            await Nav.PopToRootAsync();
        }

        public async Task PopToRootAsync(bool animated)
        {
            await Nav.PopToRootAsync(animated);
        }

        public async Task PushAsync(Page page)
        {
            await Nav.PushAsync(page);
        }

        public async Task PushAsync(Page page, bool animated)
        {
            await Nav.PushAsync(page, animated);
        }

        public async Task PushModalAsync(Page page)
        {
            await Nav.PushModalAsync(page);
        }

        public async Task PushModalAsync(Page page, bool animated)
        {
            await Nav.PushModalAsync(page, animated);
        }

        public void RemovePage(Page page)
        {
            Nav.RemovePage(page);
        }
    }
}
