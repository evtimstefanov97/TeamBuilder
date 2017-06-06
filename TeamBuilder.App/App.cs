using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Core;
using TeamBuilder.Data;

namespace TeamBuilder.App
{
    

    class App
    {
        static void Main(string[] args)
        {
            var context=new TeamBuilderContext();
           
            context.Database.Initialize(true);
            Engine engine=new Engine(new CommandDispatcher());
            engine.Run();
        }
    }
}
