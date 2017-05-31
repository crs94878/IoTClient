using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace IoTClient.Connection
{
    /// <summary>
    /// Описывает методы Класса для подключения к брокеру
    /// </summary>
    interface IConnectToBroker
    {
        MqttClient RetClient { get; set; }
        string IDClient { get; set; }
        /// <summary>
        /// Начать подключение.
        /// </summary>
        void StartConnect();
        /// <summary>
        ///Отправить сообщение.
        /// </summary>
        /// <param name="Topic">Топик.</param>
        /// <param name="Published">Сообщение.</param>
        void SendMessage(string Topic, string Published);
        /// <summary>
        /// Получать сообщения от брокера.
        /// </summary>
        /// <param name="Topics">Топики.</param>
        void ListenMessage(string[] Topics);
    }
}
