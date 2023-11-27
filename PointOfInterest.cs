using System.Collections.Generic;

namespace StarterGame
{
    public class PointOfInterest : IPointOfInterest
    {
        private bool _investigated;
        public bool Investigated { get { return _investigated; } set { _investigated = value; }}
        private string _name;
        public string Name { get { return _name; }}
        private string _description;
        
        public string Description { get {return _description; }}

        public string ItemsList {get {return _items.Description;}}

        public PointOfInterest():this("nameless", "No description"){}

        public PointOfInterest(string name, string description){
            _name = name;
            _description = description;
            _investigated = false;
            _items = new ItemContainer();
        }

        private ItemContainer _items;

        public IItem Pickup(string itemName)
        {
            return _items.GetItem(itemName);
        }

    }
}