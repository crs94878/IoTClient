using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace IoTClient
{

    /// <summary>
    /// Класс подключается к выбранному брокеру
    /// </summary>
    public class ConnectToBroker
    {
        public delegate void ReciveMessageFromBroker(string message);
        public event ReciveMessageFromBroker MessageFromBroker=delegate {};
        public MqttClient client;
        string BrokerNameOfHost;
        int brokerPort;
       public string iDClient;
        string UserName;
        string pass;
        string[] TopicsSubscribe = { "/home.StatusIoTControl" };

        #region Конструктор

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
           
            BrokerNameOfHost = NameBroker;
            brokerPort = portBroker;
            UserName = User;
            iDClient = ID;
            pass = passwordConnect;
            client = new MqttClient(BrokerNameOfHost, brokerPort, false, null, null, MqttSslProtocols.None);
      
        }
#endregion
        #region Подключение к брокеру
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
                        string[] TopikSubscribe = { "/home.StatusIoTControl" };
                        this.ListenMessage(TopikSubscribe);
               
                    }
                    catch (uPLibrary.Networking.M2Mqtt.Exceptions.MqttCommunicationException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Не удалось подключиться." +
                            "Причина:" + ex.Message);
                    }
                }
                #endregion
        #region Отправка сообщения
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
                client.Unsubscribe(TopicsSubscribe);
                this.StartConnect();
            }
        }
        #endregion
        #region Получать сообщения с подписаннных топиков   
        
        /// <summary>
        /// Listens the message.
        /// </summary>
        /// <param name="Topics">The topics.</param>
        public void ListenMessage(string [] Topics)
        {
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Subscribe(Topics, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }

        /// <summary>
        /// Handles the MqttMsgPublishReceived event of the Client control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MqttMsgPublishEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
         void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
          
            MessageFromBroker(Encoding.ASCII.GetString(e.Message));
        
        }
    }
#endregion    
}

