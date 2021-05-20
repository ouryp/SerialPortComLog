using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    public class MessageTTF : Message
    {
        public float ttf { get; }

        public MessageTTF(byte[] payload) : base(MessageId.Ttf, payload)
        {
            ttf = BitConverter.ToSingle(_payload, 0);
        }

        public override string ToString()
        {
            return base.ToString() + " TTF = " + ttf;
        }
    }
}
