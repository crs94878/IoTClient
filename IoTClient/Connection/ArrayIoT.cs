using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTClient.Connection
{
   public class ArrayIoT:IArrayIoT
    {
        DateTime TimeToUpdate;
        string status;       
        /// <summary>
        ///  Принимает и возвращает статус подключенного клиента.
        /// </summary>
        /// <value>
        /// статус клиента.
        /// </value>
        public string State { get => status; set => status = value; }
        public string Name { get => NameIoT;set { } }
        public DateTime TimeUpdate { get => TimeToUpdate; set => TimeToUpdate = value; }
        string NameIoT;
        
        public ArrayIoT(string Things,string ConnectStatus,DateTime timeUpdate)
        {
            NameIoT = Things;
            status = ConnectStatus;
            TimeToUpdate = timeUpdate;
        }
    }
}
