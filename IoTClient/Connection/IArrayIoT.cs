using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTClient.Connection
{
    /// <summary>
    /// Интерфейс описывает методы и свойства класса ArrayIoT
    /// </summary>
    interface IArrayIoT
    {
        string State { get; set; }
        string Name { get; set; }
        DateTime TimeUpdate { get; set; }
    }
}
