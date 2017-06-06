using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Utilities
{
    public static class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Users.Any(u => u.Username == username && u.IsDeleted == false);
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            using (TeamBuilderContext context=new TeamBuilderContext()) 
                {
                    return
                        context.Invitations.Any(
                            i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
                }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                return context.Teams.Any(t => t.Name == teamName && t.Members.Any(m => m.Username == username));
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                return context.Events.Any(e => e.Name == eventName && e.Creator.IsDeleted==false);
            }
        }

        public static bool IsUserCreatorOfEvent(string eventname, User user)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return
                    context.Users.Any(
                        u =>
                            u.Username == user.Username && u.IsDeleted == false &&
                            u.CreatedEvents.Any(e => e.Name == eventname && e.CreatorId == user.Id));
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return
                    context.Users.Any(
                        u =>
                            u.Username == user.Username && u.IsDeleted == false &&
                            u.CreatedTeams.Any(e => e.Name == teamName && e.CreatorId == user.Id));
            }
        }
    }
}
