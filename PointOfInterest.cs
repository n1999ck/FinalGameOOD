using System.Collections.Generic;

namespace StarterGame
{
    public class PointOfInterest : IPointOfInterest
    {
        private Room _containingRoom;
        public Room ContainingRoom { get { return _containingRoom; } set { _containingRoom = value; } }
        private bool _investigated;
        public bool Investigated { get { return _investigated; } set { _investigated = value; }}
        private string _name;
        public string Name { get { return _name; }}
        private string _description;
        
        public string Description { get {return _description; }}
        private ItemContainer _items;

        public string ItemsList {get {return _items.Description;}}

        public PointOfInterest():this("nameless", "No description", null){}

        public PointOfInterest(string name, string description, Room containingRoom){
            _name = name;
            _description = description;
            _containingRoom = containingRoom;
            _investigated = false;
            _items = new ItemContainer();
        }
        
        public void Drop(IItem item)
        {
            _items.Add(item);
        }

        public IItem Pickup(string itemName)
        {
            return _items.GetItem(itemName);
        }

    }
}