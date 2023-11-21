using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace StarterGame
{
    /*
     * Spring 2023
     */
    public class Character
    {
        private Room _currentRoom = null;
        private IItem _hand = null;
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
                NormalMessage("\n" + this.CurrentRoom.ToString());
            }
            else
            {
                ErrorMessage("\nThe door in " + direction + " is not open.");
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

        
    }

}
