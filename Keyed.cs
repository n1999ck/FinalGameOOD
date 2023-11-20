namespace StarterGame;

public class Keyed : IKeyed
{
    private IItem originalKey;
    private IItem insertedKey;

    public Keyed()
    {
        originalKey = new Item("key", 0.1f);
        insertedKey = originalKey;
    }
    public Keyed(string name)
    {
        originalKey = new Item(name, 0.1f);
        insertedKey = originalKey;
    }
    public bool HasKey
    {
        get { return insertedKey == originalKey; }
    }

    public IItem Insert(IItem key)
    {
        IItem oldKey = insertedKey;
        insertedKey = key;
        return oldKey;
    }

    public IItem Remove()
    {
        IItem oldKey = insertedKey;
        insertedKey = null;
        return oldKey;
    }
}