using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TeamBuilder.App.Core.Commands;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = string.Empty;
            string[] inputArgs = input.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            string commandName = inputArgs.Length > 0 ? inputArgs[0] : String.Empty;
            inputArgs = inputArgs.Skip(1).ToArray();

            Type commandType = Type.GetType("TeamBuilder.App.Core.Commands." + commandName + "Command");
            if (commandType == null)
            {
                throw new NotSupportedException($"Command {commandName} not supported!");
            }
            object command = Activator.CreateInstance(commandType);
            MethodInfo executeMethod = command.GetType().GetMethod("Execute");
            result = executeMethod.Invoke(command, new object[] {inputArgs}) as string;
            
            return result;
        }
    }
}
