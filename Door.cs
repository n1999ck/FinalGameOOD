namespace StarterGame
{
    public class Door : ICloseable, ILockable
    {
        private Room _room1;
        private Room _room2;
        private bool _closed;
        private ILockable _lockable;
        public ILockable Lockable {get {return _lockable;}
            set {_lockable = value; }}
        
        public bool IsClosed{get{return _closed;}}
        public bool IsOpen{get {return !_closed;}}
        public IKeyed Keyed { 
            get { return (_lockable == null?null:Lockable.Keyed); } 
            set 
            {
                if (_lockable != null)
                {
                    _lockable.Keyed = value;
                }
            } 
        }

        public Door(Room room1, Room room2){
            _room1 = room1;
            _room2 = room2;
            _closed = false;
            _lockable = null; //lets us know if we can open/close door
        }

        public bool Close()
        {
            bool result = false;
            if (!_closed)
            {
                if (CanClose)
                {
                    _closed = true;
                    result = true;
                }
            }
            return result;
        }
        
        public bool Open()
        {
            bool result = false;
            if (_closed)
            {
                if (CanOpen)
                {
                    _closed = false;
                    result = true;
                }
            }
            return result;
        }
        public bool CanClose{get {return  _lockable == null? true : _lockable.CanClose;}}
        public bool CanOpen{get {return _lockable == null? true : _lockable.CanOpen;}}   

        private bool _locked;
        public bool IsLocked { get{return _lockable==null ? false : _lockable.IsLocked;} }
        public bool IsUnlocked {get {return _lockable==null ? false : _lockable.IsUnlocked;}}
        public bool Lock()
        {
            return _lockable ==null? false:_lockable.Lock();
        }
        public bool Unlock()
        {
            return _lockable == null ? false : _lockable.Unlock();
        }
        public bool CanLock{get {return _lockable == null? false : _lockable.CanLock;}}
        public bool CanUnlock{get{return _lockable == null? false: _lockable.CanUnlock;}}
        
        public Room RoomOnTheOtherSide(Room myRoom)
        {
            if(myRoom == _room1)
            {
                return _room2;
            }
            else
            {
                return _room1;
            }
        }
        public bool HasKey { get{return Lockable==null?true:Lockable.HasKey;} }

        public IItem Insert(IItem key)
        {
            return Lockable == null ? key : Lockable.Insert(key);
        }

        public IItem Remove()
        {
            return Lockable == null ? null : Lockable.Remove();
        }

        public static Door Connect(Room roomA, Room roomB, string labelToRoomA, string labelToRoomB)
        {
            Door door = new Door(roomA, roomB);
            roomA.SetExit(labelToRoomA, door);
            roomB.SetExit(labelToRoomB, door);
            return door;
        }
    }
}