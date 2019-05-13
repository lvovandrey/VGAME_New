using LevelSetsEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LevelSetsEditor.View
{
    /// <summary>
    /// Логика взаимодействия для WindowProgress.xaml
    /// </summary>
    public partial class WindowProgress : Window, IInfoUI
    {
        public WindowProgress()
        {
            InitializeComponent();
        }
               
        int IInfoUI.Progress
        {
            get
            {
                int progress=0;
                Dispatcher.Invoke(() => {progress = (int)ProgressBar.Value; });
                return progress;
            }
            set { Dispatcher.Invoke(() => { ProgressBar.Value = value; }); }
        }

        void IInfoUI.Close()
        {
            Dispatcher.Invoke(() => { this.Close(); });
        }

        Action Command;
        void IInfoUI.Command(Action command, Action completed, string name)
        {
            Dispatcher.Invoke(() =>
            {
                Command = command;
                ButtonCommand.Content = name;
            });
        }

        void IInfoUI.InvokeCommand()
        {
            Dispatcher.Invoke(() =>
            {
                Command.Invoke();
            });
        }

        public void Clear()
        {
           
        }

        string IInfoUI.Title
        {
            get
            {
                string title="";
                Dispatcher.Invoke(() => { title = this.Title; });
                return title;
            }
            set => Dispatcher.Invoke(() => { this.Title = value; });
        }
        string IInfoUI.Message
        {
            get
            {
                string message = "";
                Dispatcher.Invoke(() => { message = (string)LabelMessage.Content; });
                return message;
            }
            set => Dispatcher.Invoke(() => { LabelMessage.Content = value; });
        }
    }
}
