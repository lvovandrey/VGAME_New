using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameCore
{
    public delegate void VoidDelegate();
    public delegate bool BoolDelegate();
    public delegate object FullFreeDelegate(params object[] vs);
    public delegate void TUserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key);
    public delegate bool BoolUserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key);


    public delegate void SenderDelegate(object sender);
    public enum GameType
    {
        Fish,
        Number,
        NumberDB,
        CardsEasy,
        CardsNewDB,
        None
    }

    class Core
    {
    }
}
