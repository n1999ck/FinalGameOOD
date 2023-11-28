using System.Collections;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace StarterGame
{
    /*
     * Spring 2023
     * Component that makes up the world
     tag describes room internally
     exits
     */
    public class Room : ITrigger
    {
        private Dictionary<string, Door> _exits;
        
        //Things to investigate
        private Dictionary<string, PointOfInterest> _pointsOfInterest;
        private string _tag;
        public string Tag { get { return _tag; } set { _tag = value; } }
        private string _description;
        public string Description { get {return _description; } set { _description = value; } }

        private List<ICharacter> _characters;

        private ItemContainer _items;

        private IRoomDelegate _roomDelegate;
        public IRoomDelegate RoomDelegate 
        {
            get 
            {
                return _roomDelegate; 
            } 
            set 
            {
                if (value != null)
                {
                    Room oldRoom = value.ContainingRoom;
                    if(value.ContainingRoom != null)
                    {
                        value.RoomDelegate = null;
                    }
                }
                _roomDelegate = value;
                if (_roomDelegate != null)
                {
                   _roomDelegate.ContainingRoom = this;
                }
            }
            }
        
        public Room() : this("No Tag"){}

        // Designated Constructor
        public Room(string tag)
        {
            _exits = new Dictionary<string, Door>();
            this.Tag = tag;
            this._roomDelegate = null; // if you want a delegate you must set
            this._items = new ItemContainer("Floor", 0f);
            this._pointsOfInterest = new Dictionary<string, PointOfInterest>();
        }

        public void SetExit(string exitName, Door door)
        {
            _exits[exitName] = door;
            if(_roomDelegate != null)
            {
                _roomDelegate.RoomDidSetExit(exitName, door);
            }
        }

        public Door GetExit(string exitName)
        {
            Door door = null;
            _exits.TryGetValue(exitName, out door);
            door = _roomDelegate==null ? door :
                _roomDelegate.RoomDidGetExit(exitName, door);
            return door;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            Dictionary<string, Door>.KeyCollection keys = _exits.Keys;
            foreach (string exitName in keys)
            {
                exitNames += " " + exitName;
            }

            exitNames = _roomDelegate == null ? exitNames :
                _roomDelegate.RoomDidGetExits(exitNames); //Pass it to the delegate who can choose what to do with it


            return exitNames;
        }

        override
        public string ToString()
        {
            string desc = "You are " + this.Tag + ".\n *** " + this.GetExits() +  "\nItems: " + _items.Description;
            return _roomDelegate == null ? desc :
                _roomDelegate.RoomDidGetDescription(desc);
        }

        public void Drop(IItem item)
        {
            _items.Add(item);
        }
        
        public IItem Pickup(String itemName)
        {
            return _items.GetItem(itemName);
        }

        public void AddPointofInterest(string name, PointOfInterest pointOfInterest)
        {
            _pointsOfInterest[name] = pointOfInterest;
        }

        public PointOfInterest GetPointOfInterest(string name){
            PointOfInterest PoIToReturn = null;
            _pointsOfInterest.TryGetValue(name, out PoIToReturn);           
            return PoIToReturn;
        }

        public string Investigate(){
            string returnString = "";
            if (_pointsOfInterest.Count > 0)
            {
                returnString += "Points of Interest in " + Tag + ":\n";
                foreach (PointOfInterest pointOfInterest in _pointsOfInterest.Values)
                {
                    returnString += pointOfInterest.Name + "\n";
                }
                returnString += "Use the command Investigate <Point of Interest Name> to look closer.";
            }
            else
            {
                returnString += "There is nothing of interest in " + Tag + ".";
            }
            return returnString;
        }
    }

    public class TrapRoom : IRoomDelegate
    {
        private string _password;
        private bool _disarmed;
        public Room ContainingRoom{ set; get;}
        public Room RoomDelegate{set; get;}

        public TrapRoom(string password)
        {
            _password = password;
            _disarmed = false;
            NotificationCenter.Instance.AddObserver("PlayerDidShoutAWord", PlayerDidShoutAWord);
        }

        public void PlayerDidShoutAWord(Notification notification)
        {
            Player player = (Player) notification.Object;
            if(player != null)
            {
                if (player.CurrentRoom == ContainingRoom)
                {
                     Dictionary<string, Object> userInfo = notification.UserInfo;
                string word = (string)userInfo["word"];
                if (word != null)
                {
                    if (word.Equals(_password))
                    {
                        _disarmed = true;
                        player.InfoMessage("You have disarmed the trap.");
                        player.InfoMessage(player.CurrentRoom.ToString());
                    }
                    else
                    {

                        player.WarningMessage("You didn't say the magic word.");
                    }
                    }
               
                }
               
            }
        }
        public void RoomDidSetExit(string exitName, Door door)
        {

        }
        public Door RoomDidGetExit(string exitName, Door door)
        {
            if(_disarmed)
            {
                return door;
            }
            else
            {
                return null;
            }
        }
        public string RoomDidGetExits(string exits)
        {
            if(_disarmed)
            {
                return exits;
            }
            else
            {
                return "There is no escape :)";
            }
        }
        public string RoomDidGetDescription(string description)
        {   
            if(_disarmed)
            {
                return description;
            }
            else
            {
                return "This is a trap room.";
            }
        }
    }
}
