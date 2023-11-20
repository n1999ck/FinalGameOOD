namespace StarterGame
{
    public class InspectCommand : Command
    {
        public InspectCommand() : base()
        {
            this.Name = "inspect";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Inspect(this.SecondWord);
            }
            else
            {
                player.WarningMessage("\nInspect what?");
            }
            return false;
        }
    }
}