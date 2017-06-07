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
        Image imageStatus;
        string OnlineImage= @"C:\Users\Shel\Documents\Visual Studio 2017\Projects\IoTClient\IoTClient\OkConnect.png";
        string OffLineImage= @"C:\Users\Shel\Documents\Visual Studio 2017\Projects\IoTClient\IoTClient\OFFConnect.png";
       

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
        /// так же подписываает на события подключенных к брокеру клиентов.
        /// </summary>
        private void UpdateControlForm()
        {
        
           Analize.ReciveMessageReWriter.MessageFromBroker += (str) => Dispatcher.Invoke(new Action(() => TalkToIoTtextBox.Text += "[" + DateTime.Now.ToString() + "]" + " " + str + "\n" ));
            ArrayIoTConnection.MonitorStatus += (iot) => Dispatcher.Invoke(new Action(() =>TheAllIoTConnected(iot) ));// TEST
            ConnectWindow.IsTrueClose += () => Dispatcher.Invoke(new Action(() => PersonStatus()));
        }


        /// <summary>
        /// Thes all io t connected.
        /// </summary>
        /// <param name="Iot">The iot.</param>
       private void TheAllIoTConnected(List<Connection.ArrayIoT>  AllIot)
        {
            ListItemIoT.Items.Clear();

            foreach (var f in AllIot)
            {
                if (f.State == "Connected")
                {


                    ListItemIoT.Items.Add(new Label() { Content = "Name:  " + f.Name + "\nStatus: " + f.State, Foreground = Brushes.Green });                  
                }
                else 
                {
                    ListItemIoT.Items.Add(new Label() { Content = "Name:  " + f.Name + "\nStatus: " +f.State,Foreground=Brushes.Red });
                }
            }
  


        }
        /// <summary>
        ///При загрузке окна сразу загружает окно авторизации на брокере
        ///Все настройки и запускает поток таймера для проверки подключения к брокеру.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListItemIoT.SelectionMode = SelectionMode.Multiple;
            UpdateStatus("No Connected",Brushes.Red);
            ConnectWindowToBroker = new ConnectWindow();
            ConnectWindowToBroker.ShowDialog();
            Task.Delay(15000);
            if (ConnectWindowToBroker.IsTrueAndConnectToBroker == true)
            {
               
                PersonStatus();
                UpdateControlForm();
                ArrayIoTConnection.TimerStartToUpdate();
                

            }
        }

        /// <summary>
        /// Отправляет сообщение Брокеру, для управления Интернета Вещей.
        /// Если подключение было выполенено то можно продолжать управление Интернета Вещей, если нет-то 
        /// появится окно предупреждения, и будет повторно открыто окно повторного подключения.
        /// </summary
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectWindowToBroker.IsTrueAndConnectToBroker == true)
            {
               
                TalkToIoTtextBox.Text += "[" + DateTime.Now.ToString() + "]" + "  " + ConnectWindow.NewConnect.IDClient + ": " + SendMessageTextBox.Text + "\n";
                Analize.TrueWrite.RegularExForSendMessage(SendMessageTextBox.Text);
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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectWindow.NewConnect.RetClient.Disconnect();
            ArrayIoTConnection.Timer.Stop();
            ConnectWindowToBroker.Dispose();
            this.Close();
        }

        private void TalkToIoTtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TalkToIoTtextBox.SelectionStart = TalkToIoTtextBox.Text.Length;
            TalkToIoTtextBox.ScrollToEnd();
        }
    }
}
