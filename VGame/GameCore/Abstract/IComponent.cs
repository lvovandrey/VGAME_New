namespace VanyaGame.Abstract
{
    public interface IComponent
    {
        string Name { get; set; }
        IComponentContainer Container { get; set; }
    }
}