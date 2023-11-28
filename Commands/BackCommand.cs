namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class BackCommand : Command
    {

        public BackCommand() : base()
        {
            this.Name = "back";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.ErrorMessage("I can't go back to " + this.SecondWord);
                player.ErrorMessage("(Hint: 'back' is a single word command!)");
            }
            else if (this.HasThirdWord())
            {
                player.ErrorMessage("I can't go back to " + this.ThirdWord);
                player.ErrorMessage("(Hint: 'back' is a single word command!)");
            }
            else
            {
                player.Back();
            }
            return false;
        }
    }
}
