using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// Génère les exceptions de la class Message
    /// </summary>
    public class ExceptionMessage : Exception
    {
        public MessageId Id { get; }
        public byte[] Payload { get; }
        public DateTime DateTime { get; }

        public ExceptionMessage(MessageId id, byte[] payload)
        {
            this.Id = id;
            this.Payload = payload;
            this.DateTime = DateTime.Now;
        }
    }
}
