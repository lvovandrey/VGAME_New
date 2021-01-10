using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InstallationTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Настройка конфигурационных файлов...");
//            if (args.Length > 1)
                ConfigurateFiles(@"c:\1.config", "");
        }

        static void ConfigurateFiles(string filename, string appDir)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filename);
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                // получаем атрибут name
                if (xnode.Name== "appSettings")
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Attributes.Count > 0)
                        {
                            XmlNode attrkey = childnode.Attributes.GetNamedItem("key");
                            if (attrkey != null && attrkey.Value == "BackgroundFilename")
                            {
                                XmlNode attrval = childnode.Attributes.GetNamedItem("value");
                                if (attrval != null)
                                    attrval.Value = @"C:\Users\Professional\Pictures\Юниты\vrayr-vapovao-oapoaplpr-uchebnik.jpg";
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            
            Console.Read();
            xDoc.Save(filename);
        }
    }
}
