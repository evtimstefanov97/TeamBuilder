using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateEventCommand
    {
        private void CreateEvent(string eventName, string description, DateTime startDateTime, DateTime endDateTime)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                Event e=new Event()
                {
                    Name = eventName,
                    Description = description,
                    StartDate = startDateTime,
                    EndDate = endDateTime,
                    CreatorId = AuthenticationManager.GetCurrentUser().Id,
                   
                };
                context.Events.AddOrUpdate(e);
                context.Users.FirstOrDefault(u => u.Id == e.CreatorId).CreatedEvents.Add(e);
                context.SaveChanges();
            }
        }
        public string Execute(string[] inputArgs)
        {
            
            Check.CheckLength(6, inputArgs);
            AuthenticationManager.Authorize();
            string EventName = inputArgs[0];
            string Description = inputArgs[1];
            DateTime startDateTime;
            bool isStartDateTime = DateTime.TryParseExact(inputArgs[2] + " " + inputArgs[3], Constants.DateTimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime);
            DateTime endDateTime;
            bool isEndDateTime = DateTime.TryParseExact(inputArgs[4] + " " + inputArgs[5], Constants.DateTimeFormat,
              CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime);
            if (!isEndDateTime || !isStartDateTime)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);

            }
            if (startDateTime > endDateTime)
            {
                throw new ArgumentException("The start date cannot be after the end date!");
            }
            else
            {
                this.CreateEvent(EventName, Description, startDateTime, endDateTime);
                return $"Event {EventName} was created successfully!";
            }
        }
        
    }
}
