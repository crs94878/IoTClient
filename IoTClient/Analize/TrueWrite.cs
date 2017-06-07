using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IoTClient.Analize
{
    /// <summary>
    /// Класс для проверки правильности ввода при подключении к брокеру и перед отправкой сообщения.
    /// </summary>
    class TrueWrite 
    {
        
        #region Проверка полей подключенения к брокеру, На факт заполнения и на наличие определенных символов
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
                System.Windows.Forms.MessageBox.Show("Поле IP адресс - не может быть пустым", "Ошибка ввода", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (User == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле Пользователь - не может быть пустым", "Ошибка ввода", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (port == "")
            {
                System.Windows.Forms.MessageBox.Show("Строка ввода потра подключения не может быть пустым", "Ошибка ввода", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (ID == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле ID клиента должно быть заполнено", "Ошибка ввода", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (pass == "")
            {
                System.Windows.Forms.MessageBox.Show("Поле пароль должно быть заполнено", "Ошибка ввода", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            return IsRegularExParamConn(Uri, User, port, ID, pass, true);
        }

        /// <summary>
        /// Функция проверяет регулярными выражениями строки на соответсвие правилам.
        /// </summary>
        /// <param name="Uris">Адрес.</param>
        /// <param name="Users">Пользователь.</param>
        /// <param name="ports">Порт.</param>
        /// <param name="IDs">ID пользователя.</param>
        /// <param name="passs">Пароль.</param>
        /// <param name="IsNotNull">были ли поля не пустые <c>true</c> [is not null].</param>
        /// <returns>
        ///   <c>true</c> Если все корректно заполнено; в остальных случаях, <c>false</c>.
        /// </returns>
        static bool IsRegularExParamConn(string Uris, string Users, string ports, string IDs, string passs, bool IsNotNull)
        {
            if (!(Regex.IsMatch(Uris, @"^(?!\..*\..*\..*\..*$)([0-9]{3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})$")))
            {
                System.Windows.Forms.MessageBox.Show("Строка ввода IP-адрес брокера ввведена не корректно, введите повторно.", "Ошибка ввода адреса", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (!(Regex.IsMatch(Users, @"^(?!\.$)(?!#$)(?!\$$)(?!%$)(?!\^$)(?!&$)(?!/$)(?!\\$)(?!,$)(.+( .+)?( .+)?)$")))
            {
                System.Windows.Forms.MessageBox.Show("Строка для ввода логина авторизации имела не верный формат" + "\nВведите повторно"
                    , "Ошибка ввода логина", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (!(Regex.IsMatch(ports, @"^(?!80$)(?!8080$)(?!443$)(?!25$)(?!5025$)([0-9]{1,6})$")))
            {
                System.Windows.Forms.MessageBox.Show("Строка для ввода порта имела не верный формат" +
                    "\nВведите числа от 0 до 99999. " + "(Порт с номером: 80,8080,25,5025 - Уже зарезервированны в системе.)", "Ошибка ввода порта", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (!(Regex.IsMatch(IDs, @"^(?!\.$)(?!#$)(?!\$$)(?!%$)(?!\^$)(?!&$)(?!/$)(?!\\$)(?!,$)(.+( .+)?( .+)?)$")))
            {
                System.Windows.Forms.MessageBox.Show("Строка для ввода ID пользователя имела не верный формат" + "\nВведите повторно"
                    , "Ошибка ввода ID", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            else if (!(Regex.IsMatch(passs, @"^(.+)$")))
            {
                System.Windows.Forms.MessageBox.Show("Строка для пароля  имела не верный формат" + "\nВведите повторно"
                    , "Ошибка ввода пароля", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        #endregion 

        #region Проверка строк перед отправкой регулярными выражениями
        public static void RegularExForSendMessage(string Command)
        {
            string patternRegular = @"^(?!@$)(?!#$)(?!\$$)(?!%$)(?!\^$)(?!&$)(.{1}[A-Za-z]{1,2}([a-z]{0,3})?([,]{0,1})?( [A-Za-z]{4})?(.{1,2})?([a-z]{0,6})?( [a-z]{3,6})?( on [a-z]{6})?)$";
            string patternCommand = @"((?!p)(?!z)(?!s)(?!lease)(?!ease)(?!se)(?!ase)[A-Za-z]{2,3} (?!p)[A-Za-z]{3,6})|((?!p)(?!z)(?!s)(?!lease)(?!ease)(?!se)(?!ase)[rgy][A-Za-z]{2,5} [A-Za-z]{2,3})";
            if (Regex.IsMatch(Command, patternRegular))
            {
                foreach (Match match in Regex.Matches(Command, patternCommand))
                    TurnTipicToSend(match.Value.ToLower());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Нельзя отправить такую команду");
            }
        }
        static void TurnTipicToSend(string mess)
        {
            string topicSend;
            //foreach(Match match in Regex.Matches(mess,))
            if (Regex.IsMatch(mess, @"g[a-z]{4}"))
            {
                topicSend = "/home.GreenLedControl";
                ConWind.ConnectWindow.NewConnect.SendMessage(topicSend, mess);
            }
            else if (Regex.IsMatch(mess, @"r[a-z]{2}"))
            {
                topicSend = "/home.RedLedControl";
                ConWind.ConnectWindow.NewConnect.SendMessage(topicSend, mess);
            }
            else if (Regex.IsMatch(mess, @"y[a-z]{5}"))
            {
                topicSend = "/home.YellowLedControl";
                ConWind.ConnectWindow.NewConnect.SendMessage(topicSend, mess);
            }
        }
    }
}

#endregion