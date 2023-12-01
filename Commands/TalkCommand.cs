namespace StarterGame
{
    public class TalkCommand : Command
    {
        public TalkCommand() : base()
        {
            this.Name = "talk";
        }

        //Dispatch
        override
            public bool Execute(Player player)
        {
            if (this.HasSecondWord() && this.HasThirdWord())
            {
                player.Talk(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.WarningMessage("\nTalk to who? " + player.CurrentRoom.roomCharacters());
            }
            return false;
        }
    }
}