namespace VGameCore.Abstract
{
    public interface IComponent
    {
        string Name { get; set; }
        IComponentContainer Container { get; set; }
    }
}