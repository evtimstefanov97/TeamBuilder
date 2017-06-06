﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class AddTeamToCommand
    {
        private void AddTeamToEvent(string teamName , string eventName )
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                Event ev = context.Events.OrderByDescending(e => e.StartDate).FirstOrDefault(v => v.Name == eventName);
                if (ev.ParticipatingTeams.Any(t => t.Name == teamName))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
                }
                ev.ParticipatingTeams.Add(team);
                context.SaveChanges();
            }
        }
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);
            AuthenticationManager.Authorize();
            string eventName = inputArgs[0];
            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }
            string teamName = inputArgs[1];
           
            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }
            if (!CommandHelper.IsUserCreatorOfEvent(eventName, AuthenticationManager.GetCurrentUser()))
            {
                throw new ArgumentException(Constants.ErrorMessages.NotAllowed);
            }
            this.AddTeamToEvent(teamName, eventName);
            return $"Team {teamName} added for {eventName}!";
        }
    }
}
