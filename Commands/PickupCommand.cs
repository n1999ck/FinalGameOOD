namespace StarterGame
{
    public class PickupCommand : Command
    {
        public PickupCommand() : base()
        {
            this.Name = "pickup";
        }

        //Dispatch
        override
            public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Pickup(this.SecondWord);
            }
            else if(this.HasThirdWord())
            {
                player.Pickup(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.WarningMessage("\nPick up what?");
            }
            return false;
        }
    }
}