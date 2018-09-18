#if __IOS__
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;



namespace LAPhil.User
{
    public partial class LoginService: ILoginService
    {
        object accountLock = new object{};

        public LoginService()
        {
        }

        public Task Save(LAPhil.User.Account account)
        {
            var authAccount = ConvertToAuthAccount(account);
            return AccountStore.Create().SaveAsync(authAccount, serviceId: ServiceId);
        }

        public LAPhil.User.Account CurrentAccount()
        {
            // this is intentionally not async.
            // there may be scenarios where we need to check the account
            // immediately. (deep link etc). Anywhere where we use 
            // `loginService.IsLoggedIn` will call into this method to get the 
            // current account. Which would mean, if this were async, we would 
            // be doing `await loginService.IsLoggedIn` 
            // It's fetching from the KeyChain in iOS and the KeyStore on 
            // Android so it should not be SUPER slow.
            // This is a method, and not a getter because on Andorid we 
            // need to pass the `Context` to get the `AccountStore`. So we
            // keep the API the same on Andriod and iOS
            
            lock (accountLock)
            {
                if (_CurrentAccount != null)
                    return _CurrentAccount;
                
                LAPhil.User.Account result = null;
                var account = FirstAccountFromStore();

                if (account != null)
                    result = ConvertToAccount(account);

                _CurrentAccount = result;
                return result;
            }
        }

        public Xamarin.Auth.Account FirstAccountFromStore()
        {
            var accounts = AccountsFromStore();

            if (accounts.Length == 0)
                return null;

            return accounts[0];
        }

        public Xamarin.Auth.Account[] AccountsFromStore()
        {
            var accounts = AccountStore
                .Create()
                .FindAccountsForService(serviceId: ServiceId)
                .ToArray();

            return accounts;
        }

        public async Task DeleteAccount(LAPhil.User.Account account)
        {
            var store = AccountStore.Create();
            var targets = AccountsFromStore()
                .Where(x => x.Username == account.Username);
            
            foreach(var x in targets)
            {
                await store.DeleteAsync(x, serviceId: ServiceId);
            }

        }

        public async Task DeleteCurrentAccount()
        {
            if (_CurrentAccount == null)
                return;

            var store = AccountStore.Create();
            var accounts = AccountsFromStore()
                .Where(x => x.Username == _CurrentAccount.Username);


            foreach(var x in accounts)
            {
                await store.DeleteAsync(x, serviceId: ServiceId);
            }
        }

        public async Task DeleteAllAccounts()
        {
            var store = AccountStore.Create();

            foreach (var x in AccountsFromStore())
            {
                await store.DeleteAsync(x, serviceId: ServiceId);
            }
        }

        public async Task Logout()
        {
            await DeleteAccount(_CurrentAccount);
            _CurrentAccount = null;
        }

        public bool IsLoggedIn()
        {
            var account = CurrentAccount();

            var hasAccount = account == null ? false : true;
            var result = hasAccount;

            return result;
        }
    }
}
#endif