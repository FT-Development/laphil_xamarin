#if NETSTANDARD2_0
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;


namespace LAPhil.User
{
    public partial class LoginService: ILoginService
    {
        public LoginService()
        {
        }

        public Task Save(LAPhil.User.Account account) 
        { 
            return Task.CompletedTask;
        }

        public LAPhil.User.Account CurrentAccount()
        {
            return new LAPhil.User.Account();
        }

        public Task DeleteAccount(LAPhil.User.Account account) 
        {
            return Task.CompletedTask;
        }
        public Task DeleteCurrentAccount() 
        {
            return Task.CompletedTask;
        }
        public Task DeleteAllAccounts() 
        {
            return Task.CompletedTask;
        }
        public Task Logout() 
        { 
            return Task.CompletedTask;
        }

        public bool IsLoggedIn()
        {
            return true;
        }
    }
}
#endif
