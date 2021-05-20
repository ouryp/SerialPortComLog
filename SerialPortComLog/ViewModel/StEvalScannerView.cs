using SerialPortCom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SerialPortComLog.ViewModel
{
    public class StEvalScannerView
    {
        public delegate void ScannerUpdate(string name);
        public event ScannerUpdate OnScannerUpdate;

        public ObservableCollection<string> names { get; private set; } = new ObservableCollection<string>();
        private Dictionary<string, StEvalPeriph> nameDictionnary;

        public StEvalScannerView(List<StEvalPeriph> stEvalPeriphs)
        {
            nameDictionnary = new Dictionary<string, StEvalPeriph>();

            MessageGetter getterName = new MessageGetter(MessageId.WhoIAm);
            stEvalPeriphs.ForEach(p =>
            {
                p.OnMessageReceived += NewMessage;
                p.Send(getterName);
            });
        }

        public bool GetPeriph(string selected, out StEvalPeriph periph)
        {
            return nameDictionnary.TryGetValue(selected, out periph);
        }

        private void NewMessage(StEvalPeriph periph, Message message)
        {
            periph.OnMessageReceived -= NewMessage;
            if (message is MessageWhoIAm iAm)
            {
                if (nameDictionnary.TryAdd(iAm.name, periph))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        names.Add(iAm.name);
                    });
                }
            }
        }
    }
}
