using System;
using System.Windows;
using VanyaGame;
using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    public class Moveable : Component
    {
        public double x { get; set; }
        public double y { get; set; }

        public Moveable(string name, ComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<HaveBody>() == null)
                throw new Exception("Have no component HaveBody in contaier");
            
            if (Container.GetComponent<HaveBox>() == null)
                throw new Exception("Have no component HaveBox in contaier");
        }

        public void MoveRight()
        {
            x = x + 10;
            if (Container.Components["HaveBody"] != null)
            {
                Thickness Margin = ((HaveBody)Container.Components["HaveBody"]).Body.Margin;
                Margin.Right = x;
                TDrawEffects.MoveTo(((HaveBody)Container.Components["HaveBody"]).Body,x,y);
            }
        }
        public void MoveTo(double x, double y)
        {
            TDrawEffects.AllAnimationNull(Container.GetComponent<HaveBody>().Body);
            TDrawEffects.MoveTo(Container.GetComponent<HaveBody>().Body, x, y);
        }
    }
}
