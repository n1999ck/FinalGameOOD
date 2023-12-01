namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class ProfilesCommand : Command
    {

        public ProfilesCommand() : base()
        {
            this.Name = "profiles";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.ErrorMessage("(Hint: 'profiles' is a single word command!)");
            }
            else if (this.HasThirdWord())
            {
                player.ErrorMessage("(Hint: 'profiles' is a single word command!)");
            }
            else
            {
                player.Profiles();
            }
            return false;
        }
    }
}
