using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LevelSetsEditor.Model
{
    public class WebBrowserModel
    {
        public WebBrowserModel()
        {
            CurURL = @"https://www.youtube.com/";

        }
        public string CurURL { get; set; }
        public WebBrowser Body { get; set; }
    }
}
