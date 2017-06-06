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
    public class KickMemberCommand
    {
        private void KickMemberFromTeam(string teamName,string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Username == username);
                Team team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                team.Members.Remove(user);
                context.SaveChanges();

            }
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);
            AuthenticationManager.Authorize();
            string teamName = inputArgs[0];
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }
            string username = inputArgs[1];
            if (!CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            }
            if (!CommandHelper.IsMemberOfTeam(teamName, username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotPartOfTeam,username,teamName));
            }
            if (!CommandHelper.IsUserCreatorOfTeam(teamName,AuthenticationManager.GetCurrentUser()))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }
            if (AuthenticationManager.GetCurrentUser().Username == username)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.CommandNotAllowed,"DisbandTeam"));
            }
            this.KickMemberFromTeam(teamName, username);
            return $"User {username} was kicked from {teamName}!";
        }
    }
}
