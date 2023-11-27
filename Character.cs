using System;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class Character
    {
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        //I guess we can have pointers to items here?
        public Dictionary<string, string> _itemResponses = null;
        
        //Might as well have some default here
        private string _defaultResponse = "I don't know anything about that.";

        //Here we can have either a string name of point of interest or a pointer to a POI
        private Dictionary<string, string> _talkResponses = null;

        public Character(Room room, string defaultResponse)
        {
            _currentRoom = room;
            _itemResponses = new Dictionary<string, string>();
            _talkResponses = new Dictionary<string, string>();
            _defaultResponse = defaultResponse;

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

        public void setItemResponse(IItem item, string response)
        {
            _itemResponses.Add(item.Name, response);
        }

        public void setPOIResponse(PointOfInterest POI, string response)
        {
            _talkResponses.Add(POI.Name, response);
        }

        

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
        

        //Get character's response for an item
        public string LookAt(IItem item)
        {
            //Item should have dictionary of character names and their responses to the item
            //otherwise, use a default string
            string response = _defaultResponse;
            _itemResponses.TryGetValue(item.Name, out response);
            return response;
           
        }


        
    }

}
