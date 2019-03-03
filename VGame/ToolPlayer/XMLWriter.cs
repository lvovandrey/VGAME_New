using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ToolPlayer
{
    public class XMLWriter
    {

  
        public static void WriteTest2(Tlevel lev1, string DirName)
        {
            // объект для сериализации уже есть
           

            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(Tlevel));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(DirName + @"\LevelSets.xml", FileMode.Create))
            {
                formatter.Serialize(fs, lev1);

            }
        }

    }

    [XmlRoot("level"), Serializable]
    public class Tlevel
    {
        [XmlAttribute]
        public string number;

        [XmlAttribute]
        public int sublevels_count;

        
        [XmlElement]
        public Tsublevel[] sublevel;
        
        public Tlevel() { }
    }

    [Serializable]
    public class Tsublevel
    {
        [XmlAttribute]
        public string number;

        public string filename;
        public string filenumber;
        public string timeBegin;
        public string timeEnd;
    }

}
