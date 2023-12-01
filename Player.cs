using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class Player : ICharacter
    {
        private Room _currentRoom = null;
        private IItem _hand = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        private IItemContainer _inventory;
        private float _maximumWeight;
        private Stack<Door> _doorHistory;

        public Player(Room room)
        {
            _currentRoom = room;
            _hand = null;
            _inventory = new ItemContainer("Inventory", 0f);
            _maximumWeight = 10f;
            _doorHistory = new Stack<Door>();
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
        
        public void WalkTo(string direction)
        {
            Door nextDoor = this.CurrentRoom.GetExit(direction);
            if (nextDoor.IsOpen)
            {
                Notification notification = new Notification("PlayerWillEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                //Remember we don't have direct access to Any Part of the room
                // Like reaching into someones pants to get their wallet to borrow a dollar
                _doorHistory.Push(nextDoor);
                CurrentRoom = nextDoor.RoomOnTheOtherSide(CurrentRoom);
                notification = new Notification("PlayerDidEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                NormalMessage("\n" + this.CurrentRoom.ToString());
            }
            else
            {
                ErrorMessage("\nThe door in " + direction + " is not open.");
            }
        }
        public void Back()
        {
            if (_doorHistory.Count > 0)
            {
                if (_doorHistory.Peek() != null)
                {
                    Door lastDoor = _doorHistory.Pop();
                    if (lastDoor.IsOpen)
                    {
                        Notification notification = new Notification("PlayerWillEnterRoom", this);
                        NotificationCenter.Instance.PostNotification(notification);
                        CurrentRoom = lastDoor.RoomOnTheOtherSide(CurrentRoom);       
                        notification = new Notification("PlayerDidEnterRoom", this);
                        NotificationCenter.Instance.PostNotification(notification);
                        NormalMessage("\n" + this.CurrentRoom.ToString());
                    }
                    else
                    {
                        ErrorMessage("\nThe door is not open.");   
                    }
                }
                else
                {
                    WarningMessage("I have nowhere to return to!");
                }
            }
            else
            {
                WarningMessage("I have nowhere to return to!");
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
                        InfoMessage("The door on " + direction + " is now unlocked.");
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

        public void Investigate(string pointOfInterestName)
        {
            Notification notification = new Notification("PlayerWillInvestigate", this);
            NotificationCenter.Instance.PostNotification(notification);
            PointOfInterest pointOfInterest = CurrentRoom.GetPointOfInterest(pointOfInterestName);
            if (pointOfInterest != null)
            {
                InfoMessage("You take a closer look at " + pointOfInterestName + ".\nIt seems to be " + pointOfInterest.Description);
                InfoMessage("Items:\n" + pointOfInterest.ItemsList);
                pointOfInterest.Investigated = true;
                notification = new Notification("PlayerDidInvestigate", this);
                NotificationCenter.Instance.PostNotification(notification);
            }
            else
            {
                InfoMessage(CurrentRoom.Investigate());
            }
        }

        //This way Investigate can also be used with no second word to look around the room
        public void Investigate()
        {
            Notification notification = new Notification("PlayerWillInvestigate", this);
            NotificationCenter.Instance.PostNotification(notification);
            InfoMessage(CurrentRoom.Investigate());
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

        public void Drop(string itemName)
        {
            IItem item = Take(itemName);
            if (item != null)
            {
                CurrentRoom.Drop(item);
                InfoMessage("You dropped " + itemName);
            }
            else
            {
                WarningMessage("There is no item named " + itemName + " in your inventory.");
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
                if(item.CanPickUp && _inventory.Weight + item.Weight <= _maximumWeight)
                {
                    Give(item);
                    InfoMessage("You picked up " + _hand.Name);
                }
                else
                {
                    WarningMessage("You don't have enough room in your bag.");
                    CurrentRoom.Drop(item);
                }
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

        public void Show(NPCharacter character, string itemName)
        {
            
        }
    }

}
