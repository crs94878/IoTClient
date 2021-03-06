﻿using System;
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
using System.Windows.Shapes;
using System.ComponentModel;

namespace IoTClient.ConWind
{
    /// <summary>
    /// Окно ввода параметров подключения к брокеру ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window, IDisposable
    {
        public delegate void IsCloseDelegat();
        public static event IsCloseDelegat IsTrueClose = delegate { };
        public static ConnectToBroker NewConnect;
        bool IsTrueAndConnect = false;
        Random randId=new Random((int)DateTime.Now.Ticks);
        private bool dispos = false;
        private Component component;
        public bool IsTrueAndConnectToBroker { get => IsTrueAndConnect; set { } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectWindow"/> class.
        /// </summary>
        public ConnectWindow()
        {
            InitializeComponent();
            IDtextBox.Text = randId.Next(0, 9999).ToString();
            component = new Component();
    }

        private void StartConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Analize.TrueWrite.IsTrueWriteParamToConnect(BrokerURITextBox.Text, UserNameTextBox.Text, BrokerPortTextBox.Text,
                IDtextBox.Text, PasswordTextBox.Text))
            {
                try
                {
                    NewConnect = new ConnectToBroker(BrokerURITextBox.Text, UserNameTextBox.Text, Convert.ToInt32(BrokerPortTextBox.Text),
                    IDtextBox.Text, PasswordTextBox.Text);
                    NewConnect.StartConnect();
                   

                    if (NewConnect.RetClient.IsConnected==true)
                {
                        
                        IsTrueAndConnect = true;
                        this.Close();
                    
                }
                else
                    {
                        IsTrueAndConnect = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка в заполнении полей при подключении:" + "\n" + ex.Message,"ОШИБКА", MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// Сохраняет параметры подключения к брокеру в XML-файл.
        /// 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveAutorizationPropertiseButton_Click(object sender, RoutedEventArgs e)
        {
            OutDate.AutorizationDate SettingsConnect = new OutDate.AutorizationDate();
            SettingsConnect.brokerURI = BrokerURITextBox.Text;
            SettingsConnect.clientName = UserNameTextBox.Text;
            SettingsConnect.clientID = IDtextBox.Text;
            SettingsConnect.port = BrokerPortTextBox.Text;
            SettingsConnect.password = PasswordTextBox.Text;
            OutDate.XmlSaveDate SaveSettingsConnect = new OutDate.XmlSaveDate(SettingsConnect);
            SaveSettingsConnect.StartSerialize();
        }
        /// <summary>
        /// Дессериализуует объект подключения из XML-файла и заподняет поля подключения к брокеру, из XML файла.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Seved_Selected(object sender, RoutedEventArgs e)
        {

            OutDate.AutorizationDate SettingsConnect = new OutDate.AutorizationDate();
            OutDate.XmlSaveDate SaveSettingsConnect = new OutDate.XmlSaveDate(SettingsConnect);
             SettingsConnect= SaveSettingsConnect.StartDeserialize();
            BrokerURITextBox.Text = SettingsConnect.brokerURI;
            UserNameTextBox.Text = SettingsConnect.clientName;
            IDtextBox.Text= SettingsConnect.clientID;
            BrokerPortTextBox.Text = SettingsConnect.port;
            PasswordTextBox.Text = SettingsConnect.password;
            this.UpdateLayout();
        }

        /// <summary>
        /// Очищает все поля TextBox в текущем окне, для ввода данных вручную.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandWrite_Selected(object sender, RoutedEventArgs e)
        {
            BrokerURITextBox.Text = null;
            UserNameTextBox.Text = null;
            TextBox[] textBox= { BrokerURITextBox, UserNameTextBox, UserNameTextBox, BrokerPortTextBox, PasswordTextBox};
            foreach(var _box in textBox)
            {
                _box.Text = null;
            }
            this.UpdateLayout();
           
        }
        public void Dispose()
        {
            this.Dispose(true);
                  GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if(!this.dispos)
            {
                if(disposing)
                {
                    component.Dispose();
                }
            }
        }
    }
}
