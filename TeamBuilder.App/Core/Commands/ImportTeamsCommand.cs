using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ImportTeamsCommand
    {
        private List<Team> GetTeamsFromXml(string filepath)
        {
            XDocument xmlDoc = XDocument.Load(filepath);
            var teams = xmlDoc.Root.Elements().Select(u => new Team()
            {
                Name = (string)u.Element("name"),
                Acronym = (string)u.Element("acronym"),
                Description = (string)u.Element("description"),
                CreatorId = (int)u.Element("creator-id"),
                InvitationsSent = new List<Invitation>(),
                ParticipatingEvents = new List<Event>()             
            });
            return teams.ToList();
        }

        private void AddTeams(List<Team> teams)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                context.Teams.AddRange(teams);
                context.SaveChanges();
            }
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);
            string filePath = inputArgs[0];
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format(Constants.ErrorMessages.FileNotFound, filePath));
            }
            List<Team> teams;
            try
            {
                teams = this.GetTeamsFromXml(filePath);
            }
            catch (Exception e)
            {

                throw new FormatException(Constants.ErrorMessages.InvalidXmlFormat);
            }
            this.AddTeams(teams);
            return $"You have successfully imported {teams.Count} teams!";
        }
    }
}
