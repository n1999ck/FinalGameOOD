using System;
using System.Collections.Generic;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class NPCharacter : ICharacter
    {
        private NPCState _state = null;
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; } }
        private string _age;
        public string Age { get { return _age; } set { _name = Age; } }
        private string _description;
        public string Description { get { return _description; } set { _description = value; } }
        
        private IItemContainer _inventory;
        
        //I guess we can have pointers to items here?
        private Dictionary<string, string> _itemResponses = null;
        public Dictionary<string, string> ItemResponses{ get { return _itemResponses; }}
        
        //Might as well have some default here
        private string _defaultResponse = "I don't know anything about that.";
        public string DefaultResponse { get { return _defaultResponse; } set { _defaultResponse = value; }}

        //Here we can have either a string name of point of interest or a pointer to a POI
        private Dictionary<string, string> _talkResponses = null;
        public Dictionary<string, string> TalkResponses { get { return _talkResponses; } }

        public NPCharacter(Room room, string name, string age, string description)
        {
            _currentRoom = room;
            _name = name;
            _age = age;
            _description = description;
            _itemResponses = new Dictionary<string, string>();
            _talkResponses = new Dictionary<string, string>();
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


        public void Drop(string itemName)
        {
            IItem item = Take(itemName);
            if (item != null)
            {
                _currentRoom.Drop(item);
                //TODO: Decide if a string should be output, implement
            }   
        }
        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ColoredMessage(string message, ConsoleColor newColor)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }

        public void NormalMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.White);
        }

        public void InfoMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Cyan);
        }

        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.DarkYellow);
        }

        public void ErrorMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Red);
        }

        public void ChangeState(NPCState state)
        {
            _state = state;
        }
        public NPCState GetNPCState()
        {
            return _state;
        }
    }

    public class NPCState
    {
        virtual public void TalkAbout(string topic){}
        virtual public void LookAt(IItem item){}
    }

    class NeutralState : NPCState
    {
        private NPCharacter _character;
        public NeutralState(NPCharacter character)
        {
            _character = character;
        }
    
        //Get npcharacter's response for an item
        public void LookAt(IItem item)
        {
            //Item should have dictionary of npcharacter names and their responses to the item
            //otherwise, use a default string
            string response = _character.DefaultResponse;
            _character.ItemResponses.TryGetValue(item.Name, out response);
            _character.NormalMessage(response);
        }

        public void TalkAbout(string topic)
        {
            string response = _character.DefaultResponse;
            _character.TalkResponses.TryGetValue(topic, out response);
            _character.NormalMessage(response);
        }
    }

        class AngryState : NPCState
        {
        private NPCharacter _character;
        public AngryState(NPCharacter character)
        {
            _character = character;
        }

        public void LookAt(IItem item)
        {
            _character.NormalMessage("I don't care about that " + item.Name + "!");
        }

        public void TalkAbout(string topic)
        {
            _character.NormalMessage("I don't want to talk about " + topic + "!");
        }
    }
}
