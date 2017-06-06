using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class InviteToTeamCommand
    {
        private bool IsInvitePending(string teamName, string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return
                    context.Invitations.Include("Team")
                        .Include("InvitedUser")
                        .Any(t => t.Team.Name == teamName && t.InvitedUser.Username == username && t.IsActive);

            }
        }

        private bool IsCreatorOrPartOfTeam(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                User currentUser = AuthenticationManager.GetCurrentUser();
                return context.Teams.Include("Members").Any(t => t.Name == teamName && (t.CreatorId == currentUser.Id || t.Members.Any(e => e.Username == currentUser.Username)));

            }
        }

        private void SendInvite(string teamName, string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                User user = context.Users.FirstOrDefault(u => u.Username == username);
                Invitation invitation=new Invitation()
                {
                    InvitedUser = user,
                     Team = team
                };
                if (team.CreatorId == user.Id)
                {
                    team.Members.Add(user);
                    invitation.IsActive = false;
                }
                context.Invitations.Add(invitation);
                context.SaveChanges();
            }
           
            
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);
            AuthenticationManager.Authorize();                
            string teamName = inputArgs[0];
            string userName = inputArgs[1];
            if (!CommandHelper.IsUserExisting(userName) || !CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }
            if (this.IsInvitePending(teamName, userName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }
            if (!this.IsCreatorOrPartOfTeam(teamName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }
            this.SendInvite(teamName, userName);
            return $"Team {teamName} invited {userName}!";
        }
    }
}
