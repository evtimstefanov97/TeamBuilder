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
    public class ShowEventCommand
    {
        private void ShowEventData(string eventname)
        {
            using (TeamBuilderContext context=new TeamBuilderContext())
            {
                Event eventtoshow=context.Events.FirstOrDefault(e=>e.Name== eventname);
                Console.WriteLine("{0} {1} {2} \n{3}", eventname, eventtoshow.StartDate, eventtoshow.EndDate,
                    eventtoshow.Description);
                Console.WriteLine("Teams: ");
                foreach (var team in eventtoshow.ParticipatingTeams)
                {
                    Console.WriteLine("- "+team.Name);
                }
            }
        }
        public void Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);
            string eventName = inputArgs[0];
            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound,eventName));
            }

             this.ShowEventData(eventName);
        }
    }
}
