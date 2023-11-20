using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class Player
    {
        private Room _currentRoom = null;
        private IItem _hand = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        public Player(Room room)
        {
            _currentRoom = room;
        }

        //Only thing player can do besides messages
        public void WaltTo(string direction)
        {
            Door nextDoor = this.CurrentRoom.GetExit(direction);
            if (nextDoor.IsOpen)
            {
                Notification notification = new Notification("PlayerWillEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                //Remember we don't have direct access to Any Part of the room
                // Like reaching into someones pants to get their wallet to borrow a dollar
                CurrentRoom = nextDoor.RoomOnTheOtherSide(CurrentRoom);
                notification = new Notification("PlayerDidEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                NormalMessage("\n" + this.CurrentRoom.Description());
            }
            else
            {
                ErrorMessage("\nThe door in " + direction + " is not open.");
            }
        }

        public void Open(string direction)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if(door != null)
            {
                if(door.IsClosed)
                {
                    if(door.Open())
                    {
                        InfoMessage("The door on " + direction + " is now open.");
                    }
                    else
                    {
                        WarningMessage("The door on " + direction + " did not open.");
                    }
                }
                else
                {
                    InfoMessage("The door on " + direction + " is already open");
                }
            }
            else
            {
                ErrorMessage("\nThere is no door on " + direction);
            }
        }

        public void Unlock(string direction)
        {
            Door door = this.CurrentRoom.GetExit(direction);
            if(door != null)
            {
                if (door.IsLocked)
                {
                    if (door.Unlock())
                    {
                        InfoMessage("The door on " +direction + " is now unlocked.");
                    }
                    else
                    {
                        WarningMessage("The door on " + direction + " did not unlock.");
                    }
                }
                else
                {
                    InfoMessage("The door on " + direction + " is already unlocked.");
                }
            }
            else
            {
                ErrorMessage("\nThere is no door on " + direction);
            }

        }
        public void Shout(string word)
        {
            NormalMessage("<<<" + word + ">>>");
            Dictionary<string, Object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word; 
            Notification notification = new Notification
                ("PlayerDidShoutAWord", this, userInfo); //designated constructor: can add userInfo to carry the info
            NotificationCenter.Instance.PostNotification(notification);
        }

        public void Inspect(string itemName){
            IItem item = CurrentRoom.Pickup(itemName);
            if (item != null)
            {
                InfoMessage("The item is: " + item.Description);
                CurrentRoom.Drop(item);
            }
            else
            {
                WarningMessage("There is no item named " + itemName + " in the room.");
            }
        }

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Pickup(String itemName)
        {
            IItem item = CurrentRoom.Pickup(itemName);
            if (item != null)
            {
                _hand = item;
                InfoMessage("You picked up " + _hand.Name);
            }
            else
            {
                WarningMessage("There is no item called " + itemName);
            }
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

        public void Insert(string exitName)
        {
            Door door = _currentRoom.GetExit(exitName);
            if (door != null)
            {
                if (_hand != null)
                {
                    door.Insert(_hand);
                    InfoMessage("You inserted " + _hand.Name + " in the door to tho " + exitName);
                    _hand = null;
                }
                else
                {
                    WarningMessage("You don't have anything to insert.");
                }
            }
            else
            {
                WarningMessage("There is no door to the " + exitName);
            }
        }
    }

}
