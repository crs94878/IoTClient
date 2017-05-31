using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IoTClient.ConWind;

namespace IoTClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ConnectWindow ConnectWindowToBroker;

       

        public MainWindow()
        {
            InitializeComponent();
            
        }
#region Обновление и отображение статуса пользователля
        /// <summary>
        /// Обновляет информацию о подключении пользователя к брокеру.
        /// </summary>
        /// <param name="State">Статус подключения</param>
        private void UpdateStatus(string State,SolidColorBrush color)
            {
            StatusRichTextBox.Document.Blocks.Clear();
            StatusRichTextBox.Document.ContentStart.InsertTextInRun(State);
            StatusRichTextBox.Foreground = color;
        }
        /// <summary>
        /// Функция обновляет статус Подключенного клиента, если не подлючен то будет надпись No Connected.
        /// </summary>
        private void PersonStatus()
        {
            PersonTextBox.Text = ConnectWindow.NewConnect.IDClient;

            if (ConnectWindow.NewConnect.RetClient.IsConnected)
            {
                UpdateStatus("Connect",Brushes.Green);
            }
            else
            {
                UpdateStatus("No Connected",Brushes.Red);
            }
        }
#endregion
        /// <summary>
        /// Функция подписывает на событие получения сообщения от брокера и выводит его в окно TextBox
        /// </summary>
        private void UpdateControlForm()
        {

           Analize.ReciveMessageReWriter.MessageFromBroker += (str) => Dispatcher.Invoke(new Action(()=> TalkToIoTtextBox.Text +="["+DateTime.Now.ToString()+"]"+": " + str+"\n" ));
            ArrayIoTConnection.MonitorStatus += (iot, stat) => Dispatcher.Invoke(new Action(() => IoTRichTextbox.Document.ContentStart.InsertTextInRun(iot+"|"+stat+"\n")));// TEST
            ConnectWindow.IsTrueClose += () => Dispatcher.Invoke(new Action(() => PersonStatus()));
        }

        /// <summary>
        ///При загрузке окна сразу загружает окно авторизации на брокере.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateStatus("No Connected",Brushes.Red);
            ConnectWindowToBroker = new ConnectWindow();
            ConnectWindowToBroker.ShowDialog();
            Task.Delay(15000);
            if (ConnectWindowToBroker.IsTrueAndConnectToBroker == true)
            {
                PersonStatus();
                UpdateControlForm();
                
            }
        }

        /// <summary>
        /// Отправляет сообщение Брокеру, для управления Интернета Вещей.
        /// </summary
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectWindowToBroker.IsTrueAndConnectToBroker == true)
            {
                string topic = "home.LedArduino";
                ConnectWindow.NewConnect.SendMessage(topic, SendMessageTextBox.Text);
                TalkToIoTtextBox.Text += "[" + DateTime.Now.ToString() + "]" + "  " + ConnectWindow.NewConnect.IDClient + ": " + SendMessageTextBox.Text + "\n";
                SendMessageTextBox.Text = null;
                PersonStatus();
            }
            else
            {
                ConnectWindowToBroker.Dispose();
              var result=  MessageBox.Show("Вы не подключены." + "\n" + "Подключитесь","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                if(result==MessageBoxResult.OK)
                {
                    ConnectWindowToBroker = new ConnectWindow();
                    ConnectWindowToBroker.ShowDialog();
                }
            }
        }

        
    }
}
