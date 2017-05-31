using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTClient.Analize
{
    /// <summary>
    /// Класс в котором будут обрабатываться Полученные сообщения, эти сообщения будут перенаправляться для изменения 
    /// контроллов интерфейса программы.
    /// </summary>
    class ReciveMessageReWriter 
    { 
        /// <summary>
        /// Делегать который указывает на событие получения сообщение от Интернета Вещей
        /// </summary>
        /// <param name="message">сообщение</param>
        public  delegate void ReciveMessageFromBroker(string message);
        /// <summary>
        /// Событие получения сообщения
        /// </summary>
        public static event ReciveMessageFromBroker MessageFromBroker = delegate { };

    /// <summary>
    /// Функция котороя обрабатывает полученные сообщения
    /// </summary>
    /// <param name="MessFromTop">Топик</param>
    /// <param name="MessageRecived">Сообщение</param>
        public static void MessageReWriter(string MessFromTop, string MessageRecived)
        {
            if (MessFromTop == "/home.StatusIoT")
            {
                MonitorStatusConnect(MessageRecived);
            }
            else if (MessFromTop == "/info.connection")
            {
                ArrayIoTConnection.AllIoTFromBroker(MessageRecived);
            }
            else
            {
                MessageFromBroker(MessageRecived);
            }
        }
        /// <summary>
        /// Обрабатывает сообщение о статусе подключения модуля Интернета Вещей
        /// </summary>
        /// <param name="StatusConnect">Полученный статус</param>
        private static void MonitorStatusConnect(string StatusConnect)
        {
            string Mess;
            string IoThings;
            IoThings = StatusConnect.Substring(0,StatusConnect.IndexOf(":"));
            Mess = StatusConnect.Substring(StatusConnect.IndexOf(":"));
            ArrayIoTConnection.IoTStatusRecive(IoThings,Mess);
        }
      
    }
}
