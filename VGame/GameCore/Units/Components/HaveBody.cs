using System.Windows;
using VanyaGame.Abstract;

namespace VanyaGame.Units.Components
{
    public class HaveBody: Component
    {
        public FrameworkElement Body { get; set; }
        public HaveBody(string name , IComponentContainer container, FrameworkElement body) : base(name, container)
        {
           Body = body;
        }
    }

}
