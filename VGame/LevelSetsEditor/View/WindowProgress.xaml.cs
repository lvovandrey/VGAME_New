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
                int progress = 0;
                Dispatcher.Invoke(() => { progress = (int)ProgressBar.Value; });
                return progress;
            }
            set { Dispatcher.Invoke(() => { ProgressBar.Value = value; }); }
        }

        void IInfoUI.Close()
        {
            Dispatcher.Invoke(() => { MessageBox.Show("Это окошко может закрыть только пользователь или программа"); });
        }

        Action Command;
        bool IInfoUI.IsHaveCommand
        {
            get
            {
                bool res = false;
                Dispatcher.Invoke(() => { res = (Command != null); });
                return res;
            }
            set { }
        }

        void IInfoUI.Command(Action command, Action completed, string name)
        {
            Dispatcher.Invoke(() =>
            {
                Command = command;
                ButtonCommand.Content = name;
                ButtonCommand.Visibility = Visibility.Visible;
            });
        }

        void IInfoUI.InvokeCommand()
        {
            Dispatcher.Invoke(() =>
            {
                Command.Invoke();
            });
        }

        void IInfoUI.Hide()
        {
            Dispatcher.Invoke(() =>
            {
                this.Topmost = false;
                this.Hide();

            });
        }

        void IInfoUI.Clear()
        {
            Dispatcher.Invoke(() =>
            {
                this.Title = "";
                this.LabelMessage.Content = "";
                this.ButtonCommand.Content = "";
                ButtonCommand.Visibility = Visibility.Collapsed;
                Command = null;
            });
        }

        void IInfoUI.Show()
        {
            Dispatcher.Invoke(() =>
            {
                this.Topmost = true;
                // this.Hide();
                this.Show();
                // this.Focus();
            });
        }

        string IInfoUI.Title
        {
            get
            {
                string title = "";
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

        class closeflag
        {

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is closeflag) e.Cancel = false;
            else e.Cancel = true;
        }

    }
}
