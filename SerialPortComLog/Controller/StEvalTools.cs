using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPortCom
{
    public class StEvalTools
    {
        public const int TIMEOUT = 1000;
        public static List<StEvalPeriph> Scanner()
        {
            List<StEvalPeriph> devices = new List<StEvalPeriph>();
            List<StEvalPeriph> stEvalPeriph = new List<StEvalPeriph>();
            string[] devicesKeys;

            // refresh
            SerialDevices.getInstance().Refresh();

            // Récupérer toutes les clés
            devicesKeys = SerialDevices.getInstance().GetKeys();

            // Créer des StEvalPeriph via les clés
            foreach(string k in devicesKeys)
            {
                devices.Add(new StEvalPeriph(k));
            }

            // Dans la reception, sauvegarder les périphs qui répondent
            void Reception(StEvalPeriph p, Message m)
            {
                if (m is MessageWhoIAm whoIAmMessage)
                {
                    stEvalPeriph.Add(p);
                }
            }

            devices.ForEach(p => p.OnMessageReceived += Reception);

            // Envoyer à tout le monde une trame
            MessageGetter message = new MessageGetter(MessageId.WhoIAm);
            devices.ForEach(sp => sp.Send(message));

            Thread.Sleep(TIMEOUT);

            return stEvalPeriph;
        }
    }
}
