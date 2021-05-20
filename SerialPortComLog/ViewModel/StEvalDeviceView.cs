using SerialPortCom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SerialPortComLog.ViewModel
{
    public class StEvalDeviceView
    {
        private StEvalPeriph stEvalPeriph;
        public ObservableCollection<Message> messages { get; private set; } = new ObservableCollection<Message>();

        public StEvalDeviceView(StEvalPeriph periph)
        {
            stEvalPeriph = periph;

            stEvalPeriph.OnMessageReceived += NewMessage;
            MessageGetter getterName = new MessageGetter(MessageId.WhoIAm);
            stEvalPeriph.Send(getterName);
        }

        public void GetWhoIAm()
        {
            MessageGetter message = new MessageGetter(MessageId.WhoIAm);
            stEvalPeriph.Send(message);
        }

        public void GetTtf()
        {
            MessageGetter message = new MessageGetter(MessageId.Ttf);
            stEvalPeriph.Send(message);
        }

        private void NewMessage(StEvalPeriph periph, Message message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                messages.Add(message);
            });
        }
    }
}
