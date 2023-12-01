namespace StarterGame
{
    public class ShowCommand : Command
    {
        public ShowCommand() : base()
        {
            this.Name = "show";
        }

        //Dispatch
        override
            public bool Execute(Player player)
        {
            if (this.HasSecondWord() && this.HasThirdWord())
            {
                player.Show(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.WarningMessage("\nShow who what?");
            }
            return false;
        }
    }
}