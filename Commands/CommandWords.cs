using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2023
     * basically a dictionary with reduced functionality
     */
    public class CommandWords
    {
        private Dictionary<string, Command> _commands;
        private static Command[] _commandArray = { new GoCommand(), 
            new QuitCommand(), new ShoutCommand(), new OpenCommand(), 
            new UnlockCommand(), new InspectCommand(), new InsertCommand(), 
            new PickupCommand(), new InvestigateCommand() };

        public CommandWords() : this(_commandArray) {}

        // Designated Constructor
        public CommandWords(Command[] commandList)
        {
            _commands = new Dictionary<string, Command>();
            foreach (Command command in commandList)
            {
                _commands[command.Name] = command;
            }
            // New addition
            Command help = new HelpCommand(this);
            _commands[help.Name] = help;
        }

        public Command Get(string word)
        {
            Command command = null;
            _commands.TryGetValue(word, out command);
            return command;
        }

        public string Description()
        {
            string commandNames = "";
            Dictionary<string, Command>.KeyCollection keys = _commands.Keys;
            foreach (string commandName in keys)
            {
                commandNames += " " + commandName;
            }
            return commandNames;
        }
    }
}
