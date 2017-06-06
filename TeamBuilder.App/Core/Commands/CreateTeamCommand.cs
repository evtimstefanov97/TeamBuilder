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
    public class CreateTeamCommand
    {
        private void AddTeam(string teamName, string acronym, string description)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team t = new Team()
                {
                  Name = teamName,
                  Acronym = acronym,
                  Description = description,               
                  CreatorId = AuthenticationManager.GetCurrentUser().Id
                };
              
                context.Teams.Add(t);
                context.Users.FirstOrDefault(u => u.Id == t.CreatorId).CreatedTeams.Add(t);
                context.SaveChanges();
            }
        }
        public string Execute(string[] inputArgs)
        {
            
            if (inputArgs.Length != 2 && inputArgs.Length != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(inputArgs));
            }
            AuthenticationManager.Authorize();
            string TeamName = inputArgs[0];
            if (CommandHelper.IsTeamExisting(TeamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, TeamName));

            }
            string Acronym = inputArgs[1];
            if (Acronym.Length != 3)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, Acronym));

            }
            string Description = inputArgs.Length == 3 ? inputArgs[2] : null;
            this.AddTeam(TeamName, Acronym, Description);
            return $"Team {TeamName} successfuly created!";
            
        }
    }
}
