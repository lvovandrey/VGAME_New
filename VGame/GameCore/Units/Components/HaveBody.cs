using System.Windows;
using VGameCore.Abstract;

namespace VGameCore.Units.Components
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
