using System;
using System.Windows;
using System.Windows.Input;
using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    public class DragAndDrop : Component
    {
        #region constructors
        public DragAndDrop(string name, IComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<HaveBody>() == null)
                throw new Exception("Have no component HaveBody in contaier");

            if (Container.GetComponent<HaveBox>() == null)
                throw new Exception("Have no component HaveBox in contaier");

            if (Container.GetComponent<Moveable>() == null)
                throw new Exception("Have no component Moveable in contaier");


            Container.GetComponent<HaveBody>().Body.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
            CanDragging = true;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public bool CanDragging { get; set; }

        public double DragXShift { get; set; }
        public double DragYShift { get; set; }
        #endregion

        #region methods
        protected virtual void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement Body = Container.GetComponent<HaveBody>().Body;

            if (!CanDragging) return;

            Body.PreviewMouseMove += PreviewMouseMove;
            Body.LostMouseCapture += LostMouseCapture;
            Body.PreviewMouseUp += PreviewMouseUp;
            Mouse.Capture(Body);



            DragXShift = e.GetPosition(Body).X;
            DragYShift = e.GetPosition(Body).Y;

            UpdatePosition(e);

        }

        protected virtual void PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }

        protected virtual void LostMouseCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        protected virtual void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        protected virtual void UpdatePosition(MouseEventArgs e)
        {
            FrameworkElement Body = Container.GetComponent<HaveBody>().Body;
            Point point = e.GetPosition(Container.GetComponent<HaveBox>().InnerBox);
            Container.GetComponent<Moveable>().MoveTo(point.X - DragXShift, point.Y - DragYShift);
        }

        protected virtual void FinishDrag(object sender, MouseEventArgs e)
        {
            FrameworkElement Body = Container.GetComponent<HaveBody>().Body;
            Body.PreviewMouseMove -= PreviewMouseMove;
            Body.LostMouseCapture -= LostMouseCapture;
            Body.PreviewMouseUp -= PreviewMouseUp;
            UpdatePosition(e);
        }



        #endregion

    }
}
