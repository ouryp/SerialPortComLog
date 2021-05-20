using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// Message du protocol : Getter.
    /// Contient l'ID du message demandé au périphérique.
    /// </summary>
    public class MessageGetter : Message
    {
        public MessageId id { get; }

        public MessageGetter( MessageId getter) : base(MessageId.Getter, new byte[] { (byte)getter })
        {
            id = getter;
        }

        public MessageGetter(byte[] payload) : base(MessageId.Getter, payload)
        {
            id = (MessageId)_payload[0];
        }

        public override string ToString()
        {
            return " - " + base.ToString() + " " + id;
        }

    }
}
