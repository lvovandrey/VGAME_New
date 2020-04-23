using System;
using System.Windows;
using VanyaGame.Abstract;
using VanyaGame.Units;
using VanyaGame.Units.Components;

namespace VanyaGame.GameCardsEasyDB.Units.Components
{
    /// <summary>
    /// Определяет специфичные для Number методы показывания и исчезновения элементов с экрана
    /// </summary>
    class NumberShower : Component
    {
        #region constructors
        public NumberShower(string name, IComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<HiderShower>() == null)
                throw new Exception("Have no component HiderShower in contaier");

            Complete += NumberShower_Complete;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        #endregion

        #region methods
        public event VoidDelegate Complete;

        public void ResetCompleteSubscriptions()
        {
            Complete = null;
            Complete += NumberShower_Complete;
        }

        public void Show(VoidDelegate complete)
        {
            TimeSpan t = TimeSpan.FromSeconds(0.5);
           // HiderShower H = Container.GetComponent<HiderShower>();

            Container.GetComponent<HiderShower>().Show(1, t, new Thickness(0), TimeSpan.FromSeconds(0.3), 20000);
           // H.Show(1, t, new Thickness(100), t);
            ToolsTimer.Delay(() => {
               // Complete();
                complete();
            }, t);
            Container.GetComponent<HaveBody>().Body.HorizontalAlignment = HorizontalAlignment.Center;
            Container.GetComponent<HaveBody>().Body.VerticalAlignment = VerticalAlignment.Center;

        }

        private void NumberShower_Complete()
        {
            
        }

        public void Hide(VoidDelegate complete)
        {

            TimeSpan t = TimeSpan.FromSeconds(0.5);
            HiderShower H = Container.GetComponent<HiderShower>();
            H.Show(0, t, new Thickness(0), t);
            ToolsTimer.Delay(() => 
            {
                Complete();
                complete();
            }, t);
        }
        #endregion

    }
}
