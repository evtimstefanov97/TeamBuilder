using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class LogInCommand
    {
        private User GetUserByCredentials(string username, string password)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return
                    context.Users.FirstOrDefault(
                        u => u.Username == username && u.Password == password && u.IsDeleted == false);
            }
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);
            string username = inputArgs[0];
            string password = inputArgs[1];
            if (AuthenticationManager.IsAuthenticated(username,password))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }
            User user = this.GetUserByCredentials(username, password);
            AuthenticationManager.Login(user);
            return $"User {user.Username} successfully logged in!";
        }
    }
}
