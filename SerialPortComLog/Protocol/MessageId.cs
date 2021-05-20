using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// ID des messages.
    /// </summary>
    public enum MessageId
    {
        Getter,
        Error,  // TODO not implemented
        WhoIAm,
        Rssi,
        Ttf,
    }
}
