using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Utilities;

namespace TeamBuilder.App.Core.Commands
{
    class ExitCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(0, inputArgs);
            Console.WriteLine("Bye!");
            Environment.Exit(0);
            return "Bye!";

        }
    }
}
