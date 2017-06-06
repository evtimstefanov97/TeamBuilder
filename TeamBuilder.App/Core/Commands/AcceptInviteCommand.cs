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
    
    public class AcceptInviteCommand
    {
        private void AcceptInvite(string teamName)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                User currentUser = AuthenticationManager.GetCurrentUser();
                Team team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                context.Users.Attach(currentUser);
                currentUser.Teams.Add(team);
                Invitation invitation =
                    context.Invitations.FirstOrDefault(
                        i => i.TeamId == team.Id && i.InvitedUserId == currentUser.Id && i.IsActive);
                invitation.IsActive = false;
                context.SaveChanges();
            }
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);
            AuthenticationManager.Authorize();
            string teamName = inputArgs[0];
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound,teamName));
            }
            if (!CommandHelper.IsInviteExisting(teamName,AuthenticationManager.GetCurrentUser()))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));

            }
            this.AcceptInvite(teamName);
            return $"User {AuthenticationManager.GetCurrentUser().Username} joined team {teamName}!";
        }
    }
}
