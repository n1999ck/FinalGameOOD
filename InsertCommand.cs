namespace StarterGame
{
    public class InsertCommand : Command
    {
        public InsertCommand() : base()
        {
            this.Name = "insert";
        }

        //Dispatch
        override
            public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Insert(this.SecondWord);
            }
            else
            {
                player.WarningMessage("\nInsert what?");
            }
            return false;
        }
    }
}