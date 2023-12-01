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
            Room parkingLot = new Room("in the ACE Embalming parking lot", "A typical, small parking lot. The pavement is showing its age. Multiple cars are parked here.");
            Room lobby = new Room("in the ACE Embalming lobby", "The ACE lobby, a small room containing a few chairs and a front desk.");
            Room deliveryBay = new Room("in the delivery bay", "An area for receiving new bodies and sending out embalmed patients. A walk-in refrigerator sits against the western wall. ");
            Room frontEmbalmingRoom = new Room("in the front embalming room", "An embalming room. It looks like it was organized before the murder, but tools are now strewn about.");
            Room backEmbalmingRoom = new Room("in the back embalming room", "An embalming room. It's clean and organized, with various tools laid neatly on the counters. Two prep tables are in the room - no clients are on them.");
            Room refrigerator = new Room("in the walk-in refrigerator", "A walk-in refrigerator, used for storing bodies before they're prepared. Currently, no clients are waiting here.");
            Room breakRoom = new Room("in the break room", "The ACE break room, where employees eat lunches and socialize between cases.");
            Room office = new Room("in the office", "The ACE office, a small and cluttered room where Amelia took care of management and financial tasks.");
            Room lockerRoom = new Room("in the locker room", "The ACE locker room, where employees change into personal protective equipment before entering the embalming room and shower after leaving.");

            Door door = Door.Connect(parkingLot, lobby, "north", "south");
            door = Door.Connect(parkingLot, deliveryBay, "west", "east");
            door = Door.Connect(deliveryBay, frontEmbalmingRoom, "north", "south");
            door = Door.Connect(deliveryBay, refrigerator, "west", "east");
            door = Door.Connect(frontEmbalmingRoom, backEmbalmingRoom, "north", "south");
            door = Door.Connect(backEmbalmingRoom, lockerRoom, "east", "west");
            door = Door.Connect(lockerRoom, lobby, "south", "north");
            door = Door.Connect(breakRoom, lobby, "west", "east");
            door = Door.Connect(breakRoom, office, "north","south");
            
            NPCharacter sophieBalmer = new NPCharacter(office, "Sophie Balmer", "26", "Amelia's niece. A shy, reserved individual and embalmer-in-training.");
            NPCharacter angelHart = new NPCharacter(breakRoom, "Angel Hart", "37", "An ex-artist who changed careers and became an embalmer. Loves the macabre and usually wears gothic attire.");
            NPCharacter detectiveSholmes = new NPCharacter(frontEmbalmingRoom, "Detective Sholmes", "39", "A detective in my department. Has a taste for the flamboyant and dramatic.");
            NPCharacter mayaFey = new NPCharacter(parkingLot, "Maya Fey", "20", "My assistant. A spiritual person who doesn't take things too seriously.");

            sophieBalmer.ChangeState("angry");
            angelHart.ChangeState("sad");

            //Locking the office
            ILockable rl = LockableFacade.MakeLockable("RegularLock", "officeKey"); //Making as ILockable instead of RegularLock- minimizes potential for error
            door.Lockable = rl;
            door.Close();
            rl.Lock();
            IItem key = rl.Remove();
            lockerRoom.Drop(key);
            
            _exit = parkingLot;
            
            TrapRoom tr = new TrapRoom("Please");
            backEmbalmingRoom.RoomDelegate = tr;
            refrigerator.RoomDelegate = tr;

            //Create items, place them
            IItem item = new Item("IPad", 0.5f, true, "");
            IItem decorator = new Item("cover", 0.2f);
            item.AddDecorator(decorator);
            parkingLot.Drop(item);

            PointOfInterest anthonysCar = new PointOfInterest("AnthonysCar", "A white 1985 Oldsmobile Cutlass Ciera. He's been driving it since it was new. It looks well-maintained, inside and out.", parkingLot);
            parkingLot.AddPointofInterest(anthonysCar);
            item = new Item("Wallet", 0.2f, "A well-worn leather wallet. The driver's license is Anthony's.");
            decorator = new Item("FamilyPhoto", 0.01f, "A photo of Anthony and his family, held in his wallet's photo slot.");
            item.AddDecorator(decorator);
            anthonysCar.Drop(item);

            PointOfInterest sophiesCar = new PointOfInterest("SophiesCar", "A tan 2009 Toyota Prius. Its back bumper sports a sizeable dent. The inside is nearly empty.", parkingLot);
            parkingLot.AddPointofInterest(sophiesCar);
            item = new Item("Photo", 0.1f, "A family photo from a holiday party. The setting seems to be a high-income home. Amelia and Sophie are visible.");;
            sophiesCar.Drop(item);

            PointOfInterest ameliasCar = new PointOfInterest("AmeliasCar", "A red 2018 Acura MDX. It looks like it's been to the car wash within the last week or so. The inside is a little cluttered.", parkingLot);
            parkingLot.AddPointofInterest(ameliasCar);
            item = new Item("Necklace", 0.1f, "A diamond hanging on a golden chain. It's a little gaudy, but the sheer size of the rock compensates.");
            ameliasCar.Drop(item);
            sophieBalmer.ChangeState("angry");
            sophieBalmer.setDesiredItem("angry", item);

            PointOfInterest frontDesk = new PointOfInterest("FrontDesk", "A large wooden desk used as the reception desk for ACE.", lobby);
            lobby.AddPointofInterest(frontDesk);
            item = new Item("Computer", 6.0f, false, "An aging desktop PC used for email and schedules. Nothing of interest is visible on it.");
            frontDesk.Drop(item);

            PointOfInterest filingCabinet = new PointOfInterest("FilingCabinet", "A metal filing cabinet, filled with business documents. Not often used in the internet age.", office);
            office.AddPointofInterest(filingCabinet);
            item = new Item("Amelia's Will", 0.1f, "Amelia's last will and testament, dated to two months ago. Flipping through reveals that she had many assets bequeathed to Sophie.");
            filingCabinet.Drop(item);

            PointOfInterest AnthonysLocker = new PointOfInterest("AnthonysLocker", "Anthony's locker is nearly empty. A set of PPE hangs neatly inside. Family photos are taped to the door.", lockerRoom);
            lockerRoom.AddPointofInterest(AnthonysLocker);
            item = new Item("Receipt", 0.1f, "A receipt from a fast food restaurant. It shows four meals purchased around lunchtime - Anthony must have bought his coworkers a meal.");
            AnthonysLocker.Drop(item);

            PointOfInterest SophiesLocker = new PointOfInterest("SophiesLocker", "Sophie's locker is decorated with faux flowers. It's a bit cluttered with clothes and papers.", lockerRoom);
            lockerRoom.AddPointofInterest(SophiesLocker);
            item = new Item("Folder", 0.1f, "A two-pocket folder. Inside is a document describing the process of will execution.");
            SophiesLocker.Drop(item);
            item = new Item("Bracelet", 0.3f, "A gaudy bracelet consisting of a golden chain and large diamond.");
            SophiesLocker.Drop(item);

            return parkingLot;
        }
    }

    
}