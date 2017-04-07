using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace IoTClient
{
    /// <summary>
    /// Класс подключается к выбранному брокеру
    /// </summary>
   public class ConnectToBroker
    {
       public MqttClient client;
        Thread clientThread;
        string BrokerNameOfHost;
        int brokerPort;
        string iDClient;
        string UserName;
        string pass;
        public bool IsConnect = false;

        /// <summary>
        /// Конструктор класса <see cref="ConnectToBroker" /> class.
        /// </summary>
        /// <param name="NameBroker">Имя брокера.</param>
        /// <param name="User">Имя пользователя.</param>
        /// <param name="portBroker">Порт подключеения к брокеру.</param>
        /// <param name="ID">ID польвателя.</param>
        /// <param name="passwordConnect">Пароль для подключению к брокеру.</param>
        public ConnectToBroker(string NameBroker, string User,
                    int portBroker, string ID, string passwordConnect)
        {
            //clientThread=new Thread(); допилить ммногопоточность
            BrokerNameOfHost = NameBroker;
            brokerPort = portBroker;
            UserName = User;
            iDClient = ID;
            pass = passwordConnect;
            client = new MqttClient(BrokerNameOfHost, brokerPort, false, null, null, MqttSslProtocols.None);
        }


        /// <summary>
        /// Подключается к брокеру.
        /// </summary>
        /// <returns>Если подлючился возвращает логическое true,
        /// Если нет то возвращается false.</returns>
        public void StartConnect()
        {
            try
            {
                client.Connect(iDClient, UserName, pass);
            }
            catch (uPLibrary.Networking.M2Mqtt.Exceptions.MqttCommunicationException ex)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось подключиться." +
                    "Причина:");
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Фукция отправляет публикацию сообщения на Topic брокеру.
        /// </summary>
        /// <param name="Topic">Topic</param>
        /// <param name="Published">Публикация.</param>
        public void SendMessage(string Topic,string Published)
        {
            if (client.IsConnected == true)
            {
                byte[] message = new byte[Published.Length];
                message = Encoding.UTF8.GetBytes(Published);
                client.Publish(Topic, message);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Вы не подлкючены к брокеру. Подключитесь.");
                this.StartConnect();
            }
        }       
    }
           
}
