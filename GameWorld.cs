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
            Room parkingLot = new Room("in the ACE Embalming parking lot");
            Room lobby = new Room("in the ACE Embalming lobby");
            Room deliveryBay = new Room("in the delivery bay");
            Room frontEmbalmingRoom = new Room("in the front embalming room");
            Room backEmbalmingRoom = new Room("in the back embalming room");
            Room refrigerator = new Room("in the walk-in refrigerator");
            Room breakRoom = new Room("in the break room");
            Room office = new Room("in the office");
            Room lockerRoom = new Room("in the locker room");

            Door door = Door.Connect(parkingLot, lobby, "north", "south");
            door = Door.Connect(parkingLot, deliveryBay, "west", "east");
            door = Door.Connect(deliveryBay, frontEmbalmingRoom, "north", "south");
            door = Door.Connect(frontEmbalmingRoom, backEmbalmingRoom, "north", "south");
            door = Door.Connect(backEmbalmingRoom, lockerRoom, "east", "west");
            door = Door.Connect(backEmbalmingRoom, refrigerator, "north", "south");
            door = Door.Connect(lockerRoom, breakRoom, "east", "west");
            door = Door.Connect(lockerRoom, lobby, "south", "north");
            door = Door.Connect(lobby, office, "east", "west");


            /*
            RegularLock rl = new RegularLock();
            door.Lockable = rl;
            Keyed keyed = new Keyed("Key1");
            rl.Keyed = keyed;
            */
            //Locking the office
            ILockable rl = LockableFacade.MakeLockable("RegularLock", "officeKey"); //Making as ILockable instead of RegularLock- minimizes potential for error
            door.Lockable = rl;
            door.Close();
            rl.Lock();
            IItem key = rl.Remove();
            lockerRoom.Drop(key);

            
            _exit = parkingLot;
            
            TrapRoom tr = new TrapRoom("shazam");
            backEmbalmingRoom.RoomDelegate = tr;
            refrigerator.RoomDelegate = tr;

            //Create items, place them
            IItem item = new Item("IPad", 0.5f);
            IItem decorator = new Item("cover", 0.2f);
            item.AddDecorator(decorator);
            lobby.Drop(item);

            return parkingLot;
        }
    }

    
}