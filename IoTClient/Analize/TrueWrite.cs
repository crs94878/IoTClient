﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTClient.Analize
{
    /// <summary>
    /// Класс для проверки правильности ввода
    /// </summary>
    class TrueWrite
    {
        /// <summary>
        /// Функция проверяет заполнение формы для подклюению.
        /// </summary>
        /// <param name="Uri">Адрес брокера.</param>
        /// <param name="User">Пользователь.</param>
        /// <param name="port">Порт.</param>
        /// <param name="ID">ID Пользователя.</param>
        /// <param name="pass">Пароль.</param>
        /// <returns>
        ///   <c>true</c> Если все поля заполнены; иначе, <c>false</c>.
        /// </returns>
        public static bool IsTrueWriteParamToConnect(string Uri, string User, string port, string ID, string pass)
        {
            if (Uri == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле IP адресс - не может быть пустым");
                return false;
            }
            else if (User == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле Пользователь - не может быть пустым");
                return false;
            }
            else if (port == "")
            {
                System.Windows.Forms.MessageBox.Show("Неверно указан порт подключения");
                return false;
            }
            else if (ID == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле ID клиента должно быть заполнено");
                return false;
            }
            else if (pass == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле пароль должно быть заполнено");
                return false;
            }
            return true;
        }
    }
}