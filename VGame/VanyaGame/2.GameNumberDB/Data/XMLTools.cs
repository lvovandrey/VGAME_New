using System.Collections.Generic;
using System.Windows;
using System.Xml;

namespace VanyaGame.GameNumberDB.Data
{
    /// <summary>
    /// Класс для загрузки даных из xml
    /// </summary>
    public static class XMLTools
    {
        public static void LoadKeyLocationsFromXML(Dictionary<string, Point> keyLocations, string filename = "Numbers/KeysLocations.xml")
        {
            XmlDocument xDoc = new XmlDocument();
            string path = System.IO.Path.Combine(Game.Sets.InterfaceDir, filename);
            xDoc.Load(path);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            int i = -1;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {

                i++;
                // получаем атрибут name
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                    {
                        Point point = new Point(0, 0);
                        
                        Game.Msg(attr.Value);
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            // если узел - company
                            if (childnode.Name == "x")
                            {
                                Game.Msg("x: " + childnode.InnerText);
                                double x=0;
                                double.TryParse(childnode.InnerText, out x);
                                point.X = x;
                                
                            }
                            if (childnode.Name == "y")
                            {
                                Game.Msg("y: " + childnode.InnerText);
                                double y = 0;
                                double.TryParse(childnode.InnerText, out y);
                                point.Y = y;

                            }

                        }
                        keyLocations.Add(attr.Value.ToString(), point);
                    }
                }
            }
        }
    }
}
