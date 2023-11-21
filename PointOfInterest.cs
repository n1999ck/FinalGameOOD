namespace StarterGame
{
    public class PointOfInterest : IPointOfInterest
    {
        private Room _location;
        private bool _investigated;
        private string _name;
        public string Name { get { return _name; }}
        private string _description;
        public string Description { get {return _description; }}

        private IItem _item;
        public IItem Pickup(string itemName)
        {
            IItem itemToReturn = null;
            if (_item != null)
            {
                if (_item.Name.Equals(itemName))
                {
                    itemToReturn = _item;
                    _item = null;   
                }
            }
            return itemToReturn;
        }
    }
}