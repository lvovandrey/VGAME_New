namespace VanyaGame.Units
{
    public interface IComponent
    {
        string Name { get; set; }
        IComponentContainer Container { get; set; }
    }
}