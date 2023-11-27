using System;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class Character
    {
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        public Character(Room room)
        {
            _currentRoom = room;
        }

        //Only thing Character can do besides messages
        public void WalkTo(string direction)
        {
            Door nextDoor = this.CurrentRoom.GetExit(direction);
            if (nextDoor.IsOpen)
            {
                Notification notification = new Notification("CharacterWillEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                //Remember we don't have direct access to Any Part of the room
                // Like reaching into someones pants to get their wallet to borrow a dollar
                CurrentRoom = nextDoor.RoomOnTheOtherSide(CurrentRoom);
                notification = new Notification("CharacterDidEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                //NormalMessage("\n" + this.CurrentRoom.ToString());
            }
            else
            {
                //ErrorMessage("\nThe door in " + direction + " is not open.");
            }
        }

        

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
        

        public string getShown(IItem item)
        {
            //Item should have dictionary of character names and their responses to the item
            //otherwise, use a default string

        }


        
    }

}
