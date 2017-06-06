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
    public class ShowTeamCommand
    {
        private void ShowTeamData(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team teamtoshow = context.Teams.FirstOrDefault(e => e.Name == teamName);
                Console.WriteLine("{0} {1}", teamName, teamtoshow.Acronym);
                Console.WriteLine("Members: ");
                foreach (var member in teamtoshow.Members)
                {
                    Console.WriteLine("- " + member.Username);
                }
            }
        }
        public void Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);
            string teamName = inputArgs[0];
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            this.ShowTeamData(teamName);
        }
    }
}
