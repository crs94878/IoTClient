using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using uPLibrary.Networking.M2Mqtt;

namespace IoTClient
{
    /// <summary>
    /// Класс подключается к выбранному брокеру
    /// </summary>
    class ConnectToBroker
    {
        MqttClient client;
        Thread clientThread;
        string  BrokerNameOfHost;
        int brokerPort;
        string iDClient;
        string UserName;
        string pass;
        /// <summary>
        /// Конструктор класса <see cref="ConnectToBroker" /> class.
        /// </summary>
        /// <param name="NameBroker">Имя брокера.</param>
        /// <param name="User">Имя пользователя.</param>
        /// <param name="portBroker">Порт подключеения к брокеру.</param>
        /// <param name="ID">ID польвателя.</param>
        /// <param name="passwordConnect">Пароль для подключению к брокеру.</param>
        public ConnectToBroker(string NameBroker, string User,
                    int portBroker, string ID,string passwordConnect)
        {
            //clientThread=new Thread(); допилить ммногопоточность
            BrokerNameOfHost = NameBroker;
            brokerPort = portBroker;
            UserName = User;
            iDClient = ID;
            pass = passwordConnect;
            client = new MqttClient(BrokerNameOfHost);
        }
        /// <summary>
        /// Подключается к брокеру.
        /// </summary>
        /// <returns>Если подлючился возвращает логическое true,
        /// Если нет то возвращается false.</returns>
        public bool StartConnect()
        {
            client.Connect(iDClient, UserName, pass);
            if (client.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
