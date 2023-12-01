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
            door = Door.Connect(deliveryBay, refrigerator, "west", "east");
            door = Door.Connect(frontEmbalmingRoom, backEmbalmingRoom, "north", "south");
            door = Door.Connect(backEmbalmingRoom, lockerRoom, "east", "west");
            door = Door.Connect(lockerRoom, lobby, "south", "north");
            door = Door.Connect(breakRoom, lobby, "west", "east");
            door = Door.Connect(breakRoom, office, "north","south");
            

            //Locking the office
            ILockable rl = LockableFacade.MakeLockable("RegularLock", "officeKey"); //Making as ILockable instead of RegularLock- minimizes potential for error
            door.Lockable = rl;
            door.Close();
            rl.Lock();
            IItem key = rl.Remove();
            lockerRoom.Drop(key);
            
            _exit = parkingLot;
            
            TrapRoom tr = new TrapRoom("");
            backEmbalmingRoom.RoomDelegate = tr;
            refrigerator.RoomDelegate = tr;

            //Create items, place them
            IItem item = new Item("IPad", 0.5f, true);
            IItem decorator = new Item("cover", 0.2f);
            item.AddDecorator(decorator);
            parkingLot.Drop(item);
            item = new Item("Notebook", 0.2f);
            parkingLot.Drop(item);
            item = new Item("Embalming fluid", 1.3f);
            parkingLot.Drop(item);

            PointOfInterest anthonysCar = new PointOfInterest("AnthonysCar", "A white 1985 Oldsmobile Cutlass Ciera. He's been driving it since it was new. It looks well-maintained, inside and out.", parkingLot);
            parkingLot.AddPointofInterest("AnthonysCar", anthonysCar);
            item = new Item("Wallet", 0.1f);
            anthonysCar.Drop(item);

            NPCharacter sophieBalmer = new NPCharacter(office, "Sophie Balmer", "26", "Amelia's niece. A shy, reserved individual and embalmer-in-training.");
            NPCharacter angelHart = new NPCharacter(breakRoom, "Angel Hart", "37", "An ex-artist who changed careers and became an embalmer. Loves the macabre and usually wears gothic attire.");
            NPCharacter detectiveSholmes = new NPCharacter(frontEmbalmingRoom, "Detective Sholmes", "39", "A detective in my department. Has a taste for the flamboyant and dramatic.");
            NPCharacter mayaFey = new NPCharacter(parkingLot, "Maya Fey", "20", "My assistant. A spiritual person who doesn't take things too seriously.");

            



            return parkingLot;
        }
    }

    
}