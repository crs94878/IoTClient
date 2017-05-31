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
    public class ConnectToBroker : Connection.IConnectToBroker
    {
       
        protected MqttClient client;
        string BrokerNameOfHost;
        int brokerPort;
        string iDClient;
        string UserName;
        string pass;
        string[] TopicsSubscribe = { "/home.StatusIoTControl" };
        /// <summary>
        /// Свойсвто возвращает объект клиент класса MQTT.
        /// </summary>
        /// <value>
        /// Клиент MQTT.
        /// </value>
        public MqttClient RetClient { get => client; set { } }
        /// <summary>
        /// Возвращает ID подключенного пользователя.
        /// </summary>
        public string IDClient { get => iDClient; set { } }

        #region Конструктор
        public ConnectToBroker()
        {

        }
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
            try
            {
                client = new MqttClient(BrokerNameOfHost, brokerPort, false, null, null, MqttSslProtocols.None);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
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
                        ListenMessage(TopicsSubscribe);
                
               
                    }
                    catch (uPLibrary.Networking.M2Mqtt.Exceptions.MqttCommunicationException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Не удалось подключиться." +
                            "Причина:" + ex.Message);
                    }
                    catch(Exception ex)
                    {
                     System.Windows.Forms.MessageBox.Show(ex.Message);
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
                StartConnect();
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
            try
            {
                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                client.Subscribe(Topics, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            } catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Функция формирует событие когда на подписанном топике появляется сообщение .
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MqttMsgPublishEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
         void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                if(Encoding.ASCII.GetString(e.Message)!="null")
                Analize.ReciveMessageReWriter.MessageReWriter(e.Topic, Encoding.ASCII.GetString(e.Message));
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
        }
    }
#endregion    
}

