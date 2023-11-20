namespace StarterGame
{
    public class WorldChange : IGameEvent
    {
        private bool _triggered;
        private ITrigger _trigger;
        private Room _inWorldRoom;
        private Room _outWorldRoom;
        private string _inOutDirection;
        private string _outInDirection;

        // interface driven design / protocol driven design
        // you create interfaces , everything works from them
        public WorldChange (ITrigger trigger, Room inWorldRoom, Room outWorldRoom, string inOutDirection, string outInDirection){
            _trigger = trigger;
            _inWorldRoom = inWorldRoom;
            _outWorldRoom = outWorldRoom;
            _inOutDirection = inOutDirection;
            _outInDirection = outInDirection;
            _triggered = false;

        }

        public void Execute(Player player){
            Door door = Door.Connect(_inWorldRoom, _outWorldRoom,_outInDirection, _inOutDirection);
            player.InfoMessage("There is a new exit " + _inWorldRoom.Tag);
            _triggered = true;
        }
    }

}