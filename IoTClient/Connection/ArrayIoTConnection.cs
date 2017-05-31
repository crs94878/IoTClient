using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTClient
{
    class ArrayIoTConnection: ConnectToBroker
    {
        
        /// <summary>
        /// Делегат который указывает на сообщение о статусе подключения Интернета Вещей
        /// </summary>
        /// <param name="IoT">Модуль Интернета Вещей</param>
        /// <param name="status">Статус подключения</param>
        public delegate void StatusConnected(string IoT, string status);

        /// <summary>
        /// Событие статуса подключения.
        /// </summary>
        public static event StatusConnected MonitorStatus = delegate { };
        public static void IoTStatusRecive(string IoT, string Status)
        {
            MonitorStatus(IoT, Status);
        }
        /// <summary>
        /// Функция получает всех список всех подключенных модулей к брокеру, и удаляет информацию о самом
        /// пользователе из списка.
        /// </summary>
        /// <param name="IoT">Модуль.</param>
        /// <returns>Список модулей</returns>
        public static string [] AllIoTFromBroker(string  IoT)
        {
          string []  AllIoT= IoT.Split(',');
            for (int i=0;i<IoT.Length;i++)
            {
               if(AllIoT[i]==ConWind.ConnectWindow.NewConnect.IDClient)
                {
                    AllIoT[i] = "";
                }
            }
            return AllIoT;
        }
    }
}
