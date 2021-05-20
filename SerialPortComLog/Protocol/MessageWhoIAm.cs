using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// Message du protocol : Who I Am.
    /// Contient le nom du périphérique en chaine de caractère.
    /// </summary>
    public class MessageWhoIAm : Message
    {
        public string name { get; }

        public MessageWhoIAm(byte[] payload) : base(MessageId.WhoIAm, payload)
        {
            name = Encoding.UTF8.GetString(_payload);
        }

        public override string ToString()
        {
            return base.ToString() + " name = " + name;
        }
    }
}
