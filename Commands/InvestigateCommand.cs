namespace StarterGame
{
    public class InvestigateCommand : Command
    {
        public InvestigateCommand() : base()
        {
            this.Name = "investigate";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Investigate(this.SecondWord);
            }
            else
            {
                player.WarningMessage("\nInvestigate what?");
            }
            return false;
        }
    }
}