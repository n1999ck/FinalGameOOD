using System.Collections.Generic;

namespace StarterGame
{
    public class CharactersProfile
    {
        private Dictionary<string, NPCharacter> _profiles = null;
        
        public CharactersProfile()
        {
            _profiles = new Dictionary<string, NPCharacter>();
        }
        
        public void AddCharacter(NPCharacter npc)
        {
            
        }
    }
}