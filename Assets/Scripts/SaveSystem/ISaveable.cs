namespace SaveSystem
{
    public interface ISaveable
    {
        IMemento GetSnapshot();
        void RestoreSnapshot(IMemento memento);
    }
}
