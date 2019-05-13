using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.Tools
{
    public interface IInfoUI
    {
        void Close();
        void Hide();
        void Clear();
     
        int Progress { get; set; }
        string Title { get; set; }
        string Message { get; set; }

        void Command(Action command, Action completed, string name);
        void InvokeCommand();
    }

    public class EmptyInfoUi : IInfoUI
    {
        public int Progress { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public void Clear()
        {

        }

        public void Close()
        {
            
        }

        public void Command(Action command, Action completed, string name)
        {
           
        }

        public void Hide()
        {

        }

        public void InvokeCommand()
        {
           
        }
    }

}
