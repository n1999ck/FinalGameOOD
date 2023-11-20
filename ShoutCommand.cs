namespace StarterGame 
{
    public class ShoutCommand : Command
    {
        public ShoutCommand() : base()
        {
            this.Name = "shout";
        }

        override
        public bool Execute(Player player)
        {
            if(this.HasSecondWord())
            {
                player.Shout(this.SecondWord);
            }
            else
            {
                player.WarningMessage("Shout what?");
            }
            return false;
        }
    }
}