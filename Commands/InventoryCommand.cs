namespace StarterGame
{
    public class InventoryCommand : Command
    {
        public InventoryCommand() : base()
        {
            this.Name = "inventory";
        }

        //Dispatch
        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Inventory();
            }
            return false;
        }
    }
}