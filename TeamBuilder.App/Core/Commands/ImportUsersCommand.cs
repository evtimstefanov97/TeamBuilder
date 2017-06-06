using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ImportUsersCommand
    {
        private List<User> GetUsersFromXml(string filepath)
        {
            XDocument xmlDoc = XDocument.Load(filepath);
            var users = xmlDoc.Root.Elements().Select(u => new User()
            {
              Username = (string)u.Element("username"),
              Age = (int)u.Element("age"),
              CreatedEvents = new List<Event>(),
              CreatedTeams = new List<Team>(),
              FirstName = (string)u.Element("first-name"),
              LastName = (string)u.Element("last-name"),
              Password = (string)u.Element("password"),
              Teams = new List<Team>(),
              Gender = (Gender)Enum.Parse(typeof(Gender),CultureInfo.CurrentCulture.TextInfo.ToTitleCase((string)u.Element("gender")))
            });
            return users.ToList();
        }

        private void AddUsers(List<User> users)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                context.Users.AddRange(users);
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
            List<User> users;
            try
            {
                users = this.GetUsersFromXml(filePath);
            }
            catch (Exception e)
            {

                throw new FormatException(Constants.ErrorMessages.InvalidXmlFormat);
            }
            this.AddUsers(users);
            return $"You have successfully imported {users.Count} users!";
        }
    }
}
