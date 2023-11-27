namespace StarterGame
{
    public class UnlockCommand : Command
    {
        public UnlockCommand() : base()
        {
            this.Name = "unlock";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Unlock(this.SecondWord);
            }
            else
            {
                player.WarningMessage("\nUnlock what?");
            }
            return false;
        }
    }
}
