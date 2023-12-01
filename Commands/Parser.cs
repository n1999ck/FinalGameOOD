namespace StarterGame
{
    /*
     * Spring 2023
     * Similar to Vending Machine parser but commands list is external
     * This way we can have different sets of commands, parser for each one
     * This Is Encapsulation
     */
    public class Parser
    {
        private CommandWords _commands;

        public Parser() : this(new CommandWords()){}

        // Designated Constructor
        // All constructors go through designated constructor
        public Parser(CommandWords newCommands)
        {
            _commands = newCommands;
        }

        public Command ParseCommand(string commandString)
        {
            Command command = null;
            string[] words = commandString.Split(' ');
            if (words.Length > 0)
            {
                command = _commands.Get(words[0]);
                if (command != null)
                {
                    if (words.Length >= 2)
                    {
                        command.SecondWord = words[1];
                    }                    
                    else
                    {
                        command.SecondWord = null;
                    }
                    if (words.Length == 3)
                    {
                        command.ThirdWord = words[2];
                    }
                    else
                    {
                        command.ThirdWord = null;
                    }
                }
                else
                {
                }
            }
            else
            {
            }
            return command;
        }

        public string Description()
        {
            return _commands.Description();
        }
    }
}
