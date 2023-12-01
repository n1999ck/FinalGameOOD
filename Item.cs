using System.Collections.Generic;
using System.Reflection.Metadata;

namespace StarterGame
{
    public class Item : IItem
    {
        private string _name;
        private float _weight;
        private IItem _decorator;
        private string _description;
        private bool _canPickUp;
        public string Name {get{return _name;}}
        public float Weight {get{return _weight +  (_decorator == null ? 0 : _decorator.Weight);}}
        public string FullDescription {get {return LongName + ", " + Weight + Description;}}
        public string Description {get {return Name + ": " + _description;} set {_description = value;}}
        public bool IsContainer {get{return false;}}
        public string LongName {
            get
            {
                return Name + ( _decorator ==null ? "" : " with " + _decorator.LongName);
            }
        }
        public bool CanPickUp { get { return _canPickUp; } }
        public Item() : this("Nameless", 0, true, ""){}
        public Item(string Name) : this(Name, 1f, true, ""){}
        public Item(string Name, float Weight) : this(Name, 1f, true, ""){}

        public Item(string Name, float Weight, string Description) : this(Name, Weight, true, Description){}

        //Designated constructor
        public Item(string Name, float Weight, bool CanPickUp, string Description)
        {
            _name = Name;
            _weight = Weight;
            _canPickUp = CanPickUp;
            _decorator = null;
            _description = Description;
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
        private bool _isHandheld;
        private bool _canPickUp;
        public bool CanPickUp { get { return _canPickUp; } }
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
        
        public string Description 
        {
            get 
            {
                string returnString = "";
                if (_items.Count == 0)
                {
                    returnString = "None";
                }
                else
                {
                    foreach (IItem item in _items.Values)
                    {
                        returnString += item.Description + "\n";
                    }
                }
                
                return returnString;
            }
        }

        public bool IsContainer {get{return true;}}
        public bool IsHandheld {get{return _isHandheld;}}
        public string LongName {
            get
            {
                return Name + ( _decorator ==null ? "" : " with " + _decorator.LongName);
            }
        }

        public IItem GetItem(string itemName) {
            IItem itemToReturn = null;
            if (_items.Count != 0)
            {
                _items.TryGetValue(itemName, out itemToReturn);
                if (itemToReturn != null)
                {
                    _items.Remove(itemName);
                }   
            }
            return itemToReturn;
        }
        public ItemContainer() : this("Nameless", 0, true, true){}
        public ItemContainer(string Name, float Weight) : this(Name, 1f, true, true){}

        //Designated constructor
        public ItemContainer(string Name, float Weight, bool IsHandheld, bool CanPickUp)
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