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

namespace IoTClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Функция обновляет статус Подключенного клиента, если не подлючен то будет надпись No Connected.
        /// </summary>
        private void PersonStatus()
        {
            PersonTextBox.Text = ConWind.ConnectWindow.NewConnect.iDClient;
            StatusRichTextBox.Document.Blocks.Clear();
            if (IoTClient.ConWind.ConnectWindow.NewConnect.client.IsConnected) {
                StatusRichTextBox.Document.ContentStart.InsertTextInRun("Connected");
                StatusRichTextBox.Foreground = Brushes.Green;
                    }
            else
            {
                StatusRichTextBox.Document.ContentStart.InsertTextInRun("No Connected");
                StatusRichTextBox.Foreground = Brushes.Red;
            }
        }
        /// <summary>
        /// Функция подписывает на событие получения сообщения от брокера и выводит его в окно TextBox
        /// </summary>
        private void UpDateTextBoxForm()
        {

           ConWind.ConnectWindow.NewConnect.MessageFromBroker += (str) => Dispatcher.Invoke(new Action(()=> TalkToIoTtextBox.Text +="["+DateTime.Now.ToString()+"]"+": " + str+"\n" ));
        }
        /// <summary>
        ///При загрузке окна сразу загружает окно авторизации на брокере.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConWind.ConnectWindow ConnectWindowToBroker = new ConWind.ConnectWindow();
            ConnectWindowToBroker.ShowDialog();
            Task.Delay(10000);
            PersonStatus();
            UpDateTextBoxForm();
        }

        /// <summary>
        /// Отправляет сообщение Брокеру, для управления Интернета Вещей.
        /// </summary
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string topic="home.LedArduino";
            ConWind.ConnectWindow.NewConnect.SendMessage(topic,SendMessageTextBox.Text);
            TalkToIoTtextBox.Text +="[" +DateTime.Now.ToString()+"]" + "  "+ConWind.ConnectWindow.NewConnect.iDClient +": " + SendMessageTextBox.Text+"\n";
            SendMessageTextBox.Text = null;
            PersonStatus();
        }
        
    }
}
