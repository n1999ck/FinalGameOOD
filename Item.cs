using System.Collections.Generic;

namespace StarterGame
{
    public class Item : IItem
    {
        private string _name;
        private float _weight;
        private IItem _decorator;
        private string _longDescription;

        public string Name {get{return _name;}}
        public float Weight {get{return _weight +  (_decorator == null ? 0 : _decorator.Weight);}}
        public string Description {get {return LongName + ", " + Weight;}}
        public string LongDescription {get {return LongDescription;} set {_longDescription = value;}}
        public bool IsContainer {get{return false;}}
        public string LongName {
            get
            {
                return Name + ( _decorator ==null ? "" : "with " + _decorator.LongName);
            }
        }
        public Item() : this("Nameless", 0){}
        public Item(string Name) : this(Name, 1f){}

        //Designated constructor
        public Item(string Name, float Weight)
        {
            _name = Name;
            _weight = Weight;
            _decorator = null;
        }
        //Decorator design pattern: Making variations of objects without changing the original
        public void AddDecorator(IItem decorator)
        {
            if (_decorator == null)
            {
                _decorator = decorator; //Simple for if you only have 1 decorator
            }
            else
            {
                //For example, add a decorator to the IPad that is a cover
                //Now can add richness without implementing 100000000 items
                _decorator.AddDecorator(decorator); //Linked list moment
            }
        }
    }

    public class ItemContainer : IItemContainer
    {
        private string _name;
        private float _weight;
        private IItem _decorator;
        private Dictionary<string, IItem> _items;
        public string Name {get{return _name;}}
        public float Weight
        {
            get
            {
                Dictionary<string, IItem>.ValueCollection values = _items.Values;
                float totalContainedWeight = 0;
                foreach(IItem item in values)
                {
                    totalContainedWeight += item.Weight;
                }
                return _weight +  (_decorator == null ? 0 : _decorator.Weight);
            }
        }
        
        public string Description {get {return LongName + ", " + Weight;}}
        public bool IsContainer {get{return false;}}
        public string LongName {
            get
            {
                return Name + ( _decorator ==null ? "" : "with " + _decorator.LongName);
            }
        }

        public ItemContainer() : this("Nameless", 0){}
        public ItemContainer(string Name) : this(Name, 1f){}

        //Designated constructor
        public ItemContainer(string Name, float Weight)
        {
            _name = Name;
            _weight = Weight;
            _decorator = null;
            _items = new Dictionary<string, IItem>();
        }

        //Decorator design pattern: Making variations of objects without changing the original
        public void AddDecorator(IItem decorator)
        {
            if (_decorator == null)
            {
                _decorator = decorator; //Simple for if you only have 1 decorator
            }
            else
            {
                //For example, add a decorator to the IPad that is a cover
                //Now can add richness without implementing 100000000 items
                _decorator.AddDecorator(decorator); //Linked list moment
            }
        }
        public void Add(IItem item)
        {
            _items.Add(item.Name, item);
        }

        public IItem Remove(string itemName)
        {
            IItem itemToRemove = null;
            _items.Remove(itemName, out itemToRemove);
            return itemToRemove;
        }
    }

}