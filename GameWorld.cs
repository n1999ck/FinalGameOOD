using System;
using System.Collections.Generic;

namespace StarterGame
{
    public class GameWorld
    {
        private static GameWorld _instance = null;

        public static GameWorld Instance
        {
            get 
            {
                if(_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }

        private Room _entrance;

        public Room Entrance { get { return _entrance; } }
        private Room _exit;
        private Dictionary<ITrigger, IGameEvent> _worldChanges;
       
        private GameWorld()
        {
            _worldChanges = new Dictionary<ITrigger, IGameEvent>();
            _entrance = CreateWorld();
            NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);

            
        }
        public void PlayerWillEnterRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                //player.WarningMessage("Player will leave " + player.CurrentRoom.Tag);
            }
        }

        public void PlayerDidEnterRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player != null)
            {
                if(player.CurrentRoom == _exit)
                {
                    player.InfoMessage("Player did arrive at the exit");
                }
                IGameEvent wc = null;
                _worldChanges.TryGetValue(player.CurrentRoom, out wc);
                if (wc != null)
                {
                    wc.Execute(player);
                }
            }
            Console.WriteLine("Player did enter room.");
        }

        private Room CreateWorld()
        {
            Room outside = new Room("outside the main entrance of the university");
            Room scctparking = new Room("in the parking lot at SCCT");
            Room boulevard = new Room("on the boulevard");
            Room universityParking = new Room("in the parking lot at University Hall");
            Room parkingDeck = new Room("in the parking deck");
            Room scct = new Room("in the SCCT building");
            Room theGreen = new Room("in the green in from of Schuster Center");
            Room universityHall = new Room("in University Hall");
            Room schuster = new Room("in the Schuster Center");
            Room davidson = new Room("in the Davidson lounge");
            Room clockTower = new Room("at the Clock Tower");
            Room greekCenter = new Room("in the Greek Center");

            //definitely not all connected correctly
            Door door = Door.Connect(outside, boulevard, "west", "east");
            door = Door.Connect(theGreen, boulevard, "east", "west");
            door = Door.Connect(scct, scctparking, "east", "west");
            door = Door.Connect(scctparking, boulevard, "north", "south");
            door = Door.Connect(scct, schuster, "north", "south");
            door = Door.Connect(schuster,theGreen, "east", "west");
            door = Door.Connect(universityHall,universityParking, "east", "west");
            door = Door.Connect(universityParking, parkingDeck, "north", "south");
            door = Door.Connect(davidson, clockTower, "west", "east");
            door = Door.Connect(clockTower, greekCenter, "north", "south");
            door = Door.Connect(schuster, universityHall, "north", "south");
            door = Door.Connect(universityParking, boulevard, "north", "south");

            /*
            RegularLock rl = new RegularLock();
            door.Lockable = rl;
            Keyed keyed = new Keyed("Key1");
            rl.Keyed = keyed;
            */
            ILockable rl = LockableFacade.MakeLockable("RegularLock", "key1"); //Making as ILockable instead of RegularLock- minimizes potential for error
            door.Lockable = rl;
            door.Close();
            rl.Lock();
            IItem key = rl.Remove();
            clockTower.Drop(key);

            //keeping track of where player goes


            WorldChange wc = new WorldChange(universityHall, schuster, davidson, "west", "east");
            _worldChanges[universityHall] = wc;
            
            _exit = parkingDeck;
            
            TrapRoom tr = new TrapRoom("shazam");
            scct.RoomDelegate = tr;
            parkingDeck.RoomDelegate = tr;

            //Create items, place them
            IItem item = new Item("IPad", 0.5f);
            IItem decorator = new Item("cover", 0.2f);
            item.AddDecorator(decorator);
            schuster.Drop(item);

            return outside;
        }
    }

    
}