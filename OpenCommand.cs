using System.Collections;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class OpenCommand : Command
    {

        public OpenCommand() : base()
        {
            this.Name = "open";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Open(this.SecondWord);
            }
            else
            {
                player.WarningMessage("\nOpen what?");
            }
            return false;
        }
    }
}
