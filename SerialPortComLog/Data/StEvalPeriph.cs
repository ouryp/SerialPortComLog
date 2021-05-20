using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// Périphérique USB de type PORT COM.
    /// </summary>
    public class StEvalPeriph : CoderEvent
    {
        public delegate void NewMessage(StEvalPeriph periph, Message message);
        public event NewMessage OnMessageReceived;

        private string key;
        private Coder coder;

        public StEvalPeriph(string deviceKey)
        {
            key = deviceKey;
            coder = new Coder(this);
            SerialDevices.getInstance().Open(key);
            SerialDevices.getInstance().Bind(key, DataReceived);
        }

        public void Send(Message message)
        {
            byte[] frame = message.ToArray();
            byte[] raw = coder.Write(frame);
            SerialDevices.getInstance().Send(key, raw);
        }

        public override string ToString()
        {
            return SerialDevices.getInstance().GetName(key);
        }

        private void DataReceived(byte b)
        {
            coder.Read(b);
        }

        void CoderEvent.NewFrame(byte[] frame)
        {
            OnMessageReceived.Invoke(this, Message.Cast(frame));
        }
    }
}
