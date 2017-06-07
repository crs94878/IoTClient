using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace IoTClient
{
    /// <summary>
    /// Класс содержит в себе массив всех подключенных клиетов к брокеру и статические медоты для 
    /// обработки состояния клиентов.
    /// </summary>
    /// <seealso cref="IoTClient.ConnectToBroker" />
    class ArrayIoTConnection : ConnectToBroker
    {
        public static System.Windows.Threading.DispatcherTimer Timer;

       static List<Connection.ArrayIoT> arrIot = new List<Connection.ArrayIoT>();
        /// <summary>
        /// Делегат который указывает на сообщение о статусе подключения Интернета Вещей
        /// </summary>
        /// <param name="IoT">Модуль Интернета Вещей</param>
        public delegate void StatusConnected(List<Connection.ArrayIoT>  IoT);

        /// <summary>
        /// Событие статуса подключения.
        /// </summary>
        public static event StatusConnected MonitorStatus = delegate { };


        /// <summary>
        /// Функция получает сообщения от брокера для обработки состояния интенета вещей
        /// и заполнения списка из объектов подключенныхъ клиентов.
        /// </summary>
        /// <param name="IoT">Сам интернет вещь.</param>
        /// <param name="Status">статус.</param>
        public static void IoTStatusRecive(string IoT, string Status)         
        {
            bool IsNameFound=false;
            bool IsStateNo = false;
            
            int i = 0;
            if (arrIot.Count>0)
            for (;i<arrIot.Count;i++)
            {
               
                if (arrIot[i].Name == IoT)
                    {
                        IsNameFound = true;
                        if (arrIot[i].State != Status)
                        {
                            IsStateNo = true;
                            break;
                        }
                        else break;
                    }
                 else if (arrIot[i].Name != IoT)
                 {
                        IsStateNo = false;
                        
                 }
                }
            else
            {
                arrIot.Add(new Connection.ArrayIoT(IoT, Status,DateTime.Now));
                MonitorStatus(arrIot);
                IsNameFound = true;
            }
            if(IsNameFound==false)
            {
                arrIot.Add(new Connection.ArrayIoT(IoT, Status, DateTime.Now));
                MonitorStatus(arrIot);
            }
            else if(IsNameFound==true)
            {
                if(IsStateNo==true)
                {
                    arrIot[i].State = Status;
                    arrIot[i].TimeUpdate = DateTime.Now;
                    MonitorStatus(arrIot);
                }
            }

        }
        /// <summary>
        /// Автоматически обновляет статус подключенных интернет вещей
        /// если от интернет вещи не было сообщения 5 секунд что он онлайн, то статус меняется на оффлайн.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> События по тику таймера.</param>
        static void UpdateStatusIoT(object sender,EventArgs e)
        {
            try
            {
                if (arrIot.Count > 0)
                {
                    for (int i = 0; i < arrIot.Count; i++)
                    {
                        if( (DateTime.Now- arrIot[i].TimeUpdate) > (TimeSpan.FromSeconds(20)))
                        {
                            arrIot[i].State = "Disconnect";
                            arrIot[i].TimeUpdate = DateTime.Now;
                            MonitorStatus(arrIot);
                        }
                    }
                }
            } catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Таймер, который в запускается в отдельном потоке, и запускает проверку интернета вещей на активность.
        /// </summary>
        public static void TimerStartToUpdate()
        {
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.IsEnabled = true;
            Timer.Tick += new EventHandler(UpdateStatusIoT);
            Timer.Start();
        }
    }
}
