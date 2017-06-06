using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ExportTeamCommand
    {
        private Team GetTeamByNameWithMembers(string teamName)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                return context.Teams.Include("Members").FirstOrDefault(t => t.Name == teamName);
            }
        }

        private void ExportTeam(Team team)
        {
            string json = JsonConvert.SerializeObject(new
            {
                Name = team.Name,
                Acronym = team.Acronym,
                Members = team.Members.Select(m => m.Username)
            }, Formatting.Indented);
            File.WriteAllText("../../Import/team.json", json);
        }     
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);
            string teamName = inputArgs[0];
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound,teamName));
            }
            Team team = this.GetTeamByNameWithMembers(teamName);
            this.ExportTeam(team);
            return $"Team {teamName} exported!";
        }
    }
}
