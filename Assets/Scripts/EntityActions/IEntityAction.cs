public interface IEntityAction
{
    void Abort();
    bool IsDone();
    void Update();
}