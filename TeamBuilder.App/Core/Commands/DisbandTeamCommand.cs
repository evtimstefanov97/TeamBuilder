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
    public class DisbandTeamCommand
    {
        private void DisbandTeam(string teamName)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                Team team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                context.Teams.Remove(team);
                context.SaveChanges();
            }
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);
            AuthenticationManager.Authorize();
            string teamName = inputArgs[0];
            User currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsUserExisting(currentUser.Username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound,currentUser.Username));
            }
            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }
            this.DisbandTeam(teamName);
            return $"{teamName} was disbanded!";
        }
    }
}
