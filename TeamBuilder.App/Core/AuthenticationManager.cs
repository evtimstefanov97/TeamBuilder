using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core
{
    class AuthenticationManager
    {
        private static User currentUser;
        public static void Authorize()
        {
            if (currentUser == null)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.LoginFirst));
            }
        }

        public static void Login(User user)
        {
            currentUser = user;
        }

        public static void Logout()
        {
            if (currentUser == null)
            {

                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);

            }
            else
            {
                currentUser = null;
            }
            
        }

        public static bool IsAuthenticated(string username,string password)
        {
            if (currentUser != null)
            {
                return true;
            }
            else if (CommandHelper.IsUserExisting(username))
            {
                return false;
            }
            else
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound,username));
            }
        }

        public static User GetCurrentUser()
        {
            if (currentUser == null )
            {
                throw new InvalidOperationException(Constants.ErrorMessages.UserNotFound);
            }
            else
            {
                return currentUser;
            }
        }
    }
}
