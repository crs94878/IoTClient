using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace IoTClient.OutDate
{
    /// <summary>
    /// Класс используя XML сериализацию, сериализует объект параметров подключения к брокеру и десериалихует обратно
    /// </summary>
    class XmlSaveDate : AutorizationDate
    {
            XmlSerializer xSerealizeDate;
       public AutorizationDate dateAutorization;
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSaveDate"/> class.
        /// </summary>
        /// <param name="autorizationDate">Приинимает параметр типа AutorizationDate.</param>
        public XmlSaveDate(AutorizationDate autorizationDate)
        {
            dateAutorization = autorizationDate;
            xSerealizeDate = new XmlSerializer(typeof(AutorizationDate));
        }
        /// <summary>
        /// Сериализует объект подключения в XML Файл.
        /// </summary>
        public void StartSerialize()
        {
            
            Directory.CreateDirectory(Path.GetFullPath(System.Environment.SpecialFolder.ApplicationData.ToString())+ @"\SaveAutorizatadeDate");
            xSerealizeDate.Serialize(XmlWriter.Create(Path.GetFullPath(System.Environment.SpecialFolder.ApplicationData.ToString())+@"\SaveAutorizatadeDate\AutorizationData.xml"), dateAutorization);
            
        }
        /// <summary>
        /// Десериализует объект подключения обратно.
        /// </summary>
        public AutorizationDate StartDeserialize()
        {
            
             FileStream fileAutorizated = new FileStream(Path.GetFullPath(System.Environment.SpecialFolder.ApplicationData.ToString()) + @"\SaveAutorizatadeDate\AutorizationData.xml",
                                                                                                            FileMode.Open);
            XmlReader deserialiseSetting= XmlReader.Create(fileAutorizated);
            dateAutorization = (AutorizationDate)xSerealizeDate.Deserialize(deserialiseSetting);
            return dateAutorization;
        }
    }
    
}
