using System.Collections.Generic;

namespace StarterGame {
    public class AssistantCharacter : ICharacter
        {
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        
        private IItemContainer _inventory;
        
        //I guess we can have pointers to items here?
        public Dictionary<string, string> _itemResponses = null;
        
        //Might as well have some default here
        private string _defaultResponse = "I don't know anything about that.";

        //Here we can have either a string name of point of interest or a pointer to a POI
        private Dictionary<string, string> _talkResponses = null;

        public AssistantCharacter(Room room, string defaultResponse)
        {
            _currentRoom = room;
            _itemResponses = new Dictionary<string, string>();
            _talkResponses = new Dictionary<string, string>();
            _defaultResponse = defaultResponse;

        }
        public void WalkTo(string direction)
        {
            Door nextDoor = this.CurrentRoom.GetExit(direction);
            if (nextDoor.IsOpen)
            {
                Notification notification = new Notification("NPCharacterWillEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                //Remember we don't have direct access to Any Part of the room
                // Like reaching into someones pants to get their wallet to borrow a dollar
                CurrentRoom = nextDoor.RoomOnTheOtherSide(CurrentRoom);
                notification = new Notification("NPCharacterDidEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                //NormalMessage("\n" + this.CurrentRoom.ToString());
            }
            else
            {
                //ErrorMessage("\nThe door in " + direction + " is not open.");
            }
        }

        public void setItemResponse(IItem item, string response)
        {
            _itemResponses.Add(item.Name, response);
        }

        public void setPOIResponse(PointOfInterest POI, string response)
        {
            _talkResponses.Add(POI.Name, response);
        }

        
        public void Give(IItem item)
        {
            if(item != null)
            {
                _inventory.Add(item);
            }
        }

        public IItem Take(string itemName)
        {
            return _inventory.Remove(itemName);
        }

        //Get npcharacter's response for an item
        public string LookAt(IItem item)
        {
            //Item should have dictionary of npcharacter names and their responses to the item
            //otherwise, use a default string
            string response = _defaultResponse;
            _itemResponses.TryGetValue(item.Name, out response);
            return response;
           
        }

        public void Drop(string itemName)
        {
            IItem item = Take(itemName);
            if (item != null)
            {
                _currentRoom.Drop(item);
                //TODO: Decide if a string should be output, implement
            }   
        }
    }
}