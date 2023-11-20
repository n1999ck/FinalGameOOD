namespace StarterGame
{
    public interface ITrigger
    {

    }

    public interface IGameEvent
    {
        public void Execute(Player player);
    }
    
    public interface IRoomDelegate
    {

        public Room ContainingRoom{ set; get; }
        public Room RoomDelegate { get; set; }
        public void RoomDidSetExit(string exitName, Door door);
        public Door RoomDidGetExit(string exitName, Door door);
        public string RoomDidGetExits(string exits);
        public string RoomDidGetDescription(string description);    
    }

    public interface ICloseable
    {
        bool IsClosed{get;}
        bool IsOpen{get;}
        bool Close();
        bool Open();
        bool CanClose{get;}
        bool CanOpen{get;}   
    }

    public interface ILockable : IKeyed
    {
        bool IsLocked { get; }
        bool IsUnlocked { get; }
        bool Lock();
        bool Unlock();
        bool CanLock { get; }
        bool CanUnlock { get; }
        bool CanClose { get; }
        bool CanOpen { get; }
        IKeyed Keyed { set; get; }
    }

    public interface IKeyed 
    {
        bool HasKey { get; }
        IItem Insert(IItem key);
        IItem Remove();
    }

    public interface IItem
    {
        string Name { get; }
        float Weight { get; }
        string LongName {get;}
        string Description { get; }

        bool IsContainer { get; }
        void AddDecorator(IItem decorator);
    }

    public interface IItemContainer : IItem
    {
        void Add(IItem item);
        IItem Remove(string itemName);
    }
}