using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace SerialPortCom
{
    public class SerialDevices
    {
        static private SerialDevices instance;
        private Dictionary<string, SerialPortExt> devices;

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            SerialPortExt spExt;
            if(devices.TryGetValue(sp.PortName,out spExt))
            {
                while (spExt.BytesToRead > 0)
                {
                    spExt.OnNewByte((byte)sp.ReadByte());
                }
            }
        }

        private SerialDevices()
        {
            devices = new Dictionary<string, SerialPortExt>();
        }

        public void Refresh()
        {
            string[] ports = SerialPort.GetPortNames();

            lock (devices)
            {
                // Add connected port
                foreach (string port in ports)
                {
                    if (!devices.ContainsKey(port))
                    {
                        devices.Add(port, new SerialPortExt(port));
                    }
                }

                // Remove disconnected port
                foreach (var k in devices.Keys)
                {
                    if (!Array.Exists(ports, p => p == k))
                    {
                        devices.Remove(k);
                    }
                }
            }
        }

        public string[] GetKeys()
        {
            return devices.Keys.ToArray();
        }

        public void Open(string key)
        {
            SerialPortExt spExt;
            if(devices.TryGetValue(key,out spExt))
            {
                if (!spExt.IsOpen)
                {
                    spExt.Open();
                    spExt.DataReceived += DataReceived;
                }
            }
        }

        public void Close(string key)
        {
            SerialPortExt spExt;
            if (devices.TryGetValue(key, out spExt))
            {
                spExt.Close();
                spExt.DataReceived -= DataReceived;
            }
        }

        public void Bind(string key, SerialPortExt.NewByte newByte)
        {
            SerialPortExt spExt;
            if(devices.TryGetValue(key,out spExt))
            {
                spExt.OnNewByteReceived += newByte;
            }
        }

        public void UnBind(string key, SerialPortExt.NewByte newByte)
        {
            SerialPortExt spExt;
            if (devices.TryGetValue(key, out spExt))
            {
                spExt.OnNewByteReceived -= newByte;
            }
        }

        public void Send(string key, byte[] data)
        {
            SerialPortExt spExt;
            lock (devices)
            {
                if (devices.TryGetValue(key, out spExt))
                {
                    spExt.Write(data, 0, data.Length);
                }
            }
        }

        public string GetName(string key)
        {
            SerialPortExt spExt;
            if (devices.TryGetValue(key, out spExt))
            {
                return spExt.PortName;
            }
            else
            {
                return null;
            }
        }

        public static SerialDevices getInstance()
        {
            if(instance == null)
            {
                instance = new SerialDevices();
            }

            return instance;
        }

        /// <summary>
        /// This class able to mask the SerialPort instance to user when the SerialDataReceivedEventHandler is trigged 
        /// </summary>
        public class SerialPortExt : SerialPort
        {
            public delegate void NewByte(byte data);
            public event NewByte OnNewByteReceived;

            public SerialPortExt(string port) : base(port)
            {
            }

            public void OnNewByte(byte b)
            {
                OnNewByteReceived.Invoke(b);
            }
        }
    }
}
