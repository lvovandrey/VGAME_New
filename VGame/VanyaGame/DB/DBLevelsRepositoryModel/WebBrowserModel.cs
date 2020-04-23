using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.DB.DBLevelsRepositoryModel
{
    public class WebBrowserModel
    {
        public WebBrowserModel()
        { CurURL = @"https://www.youtube.com/"; }

        public int Id { get; set; }

        public string CurURL { get; set; }
    }
}
