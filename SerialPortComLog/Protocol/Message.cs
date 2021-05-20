using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// Class mère des messages du protocol.
    /// </summary>
    public abstract class Message
    {
        #region ATTRIBUTS PROTECTED
        protected MessageId _id { get; }
        protected byte[] _payload;
        #endregion

        #region CTORS
        /// <summary>
        /// Initialize a message by id and payload
        /// </summary>
        /// <param name="id">Enum MessageId</param>
        /// <param name="payload">byte array paload</param>
        public Message(MessageId id, byte[] payload)
        {
            _id = id;
            _payload = payload;
        }

        /// <summary>
        /// Initialize a message by a decoded frame
        /// </summary>
        /// <param name="frame">input decoded frame</param>
        public Message(byte[] frame)
        {
            _id = (MessageId)frame[0];
            if (frame.Length > 1)
            {
                _payload = new byte[frame.Length - 1];
                Array.Copy(frame, 1, _payload, 0, _payload.Length);
            }
        }
        #endregion

        #region METHODE PUBLIC
        /// <summary>
        /// Return message as byte array
        /// </summary>
        /// <returns>frame byte array</returns>
        public byte[] ToArray()
        {
            byte[] array = new byte[_payload.Length + 1];
            array.Append((byte)_id);
            _payload.CopyTo(array, 1);

            return array;
        }
        #endregion

        public override string ToString()
        {
            return DateTime.Now.ToString() + " : " + _id;
        }

        #region METHODE STATIC
        /// <summary>
        /// Cast generic Message by id
        /// </summary>
        /// <param name="frame">decoded frame</param>
        /// <returns>Specific Message</returns>
        public static Message Cast(byte[] frame)
        {
            MessageId id = (MessageId)frame[0];
            byte[] payload = new byte[frame.Length - 1];
            Array.Copy(frame, 1, payload, 0, payload.Length);

            switch (id)
            {
                case MessageId.Getter:
                    return new MessageGetter(payload);
                case MessageId.WhoIAm:
                    return new MessageWhoIAm(payload);
                case MessageId.Error:
                    // TODO Implement that
                    throw new NotImplementedException();
                case MessageId.Rssi:
                    // TODO Implement that
                    throw new NotImplementedException();
                case MessageId.Ttf:
                    return new MessageTTF(payload);
                default:
                    throw new ExceptionMessage(id, payload);
            }
        }
        #endregion
    }
}
