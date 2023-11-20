namespace StarterGame
{
    public class LockableFacade
    {
        //Design decision
        //Could just make everything static class methods, nothing stored in here
        public static ILockable MakeLockable(string lockableName, string keyName) //Design decision - what to pass here?
        {
            ILockable lockable = null;
            {
                //Could check for key but whatever
                switch (lockableName)
                {
                    case "RegularLock":
                        lockable = new RegularLock();
                        break;
                    default:
                        lockable = new RegularLock();
                        break;
                }

                if (!keyName.Equals(""))
                {
                    lockable.Keyed = new Keyed(keyName);
                }

                return lockable;
            }
        }
    }
}
