using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SerialPortCom
{
    /// <summary>
    /// Code / décode la fin d'une trame.
    /// L'octet de Fin De Trame (EOF) définit la fin d'une trame.
    /// Dans la trame :
    ///     Les octets de valeur EOF sont remplacé par une valeure subsitue (SUBS).
    ///     Pour différencier un SUBS d'un EOF on rajouter un octet FLAG à la suite : {OEF} => {SUBS, EOF_FLAG}, {SUBS} => {SUBS, SUBS_FLAG}
    /// </summary>
    public class Coder
    {
        #region PRIVATE ATTRIBUTS
        private const byte EOF = 0xEF;
        private const byte SUBS = 0xEE;
        private const byte EOF_FLAG = 0x01;
        private const byte SUBS_FLAG = 0x00;

        private bool lastByteIsSubs;
        private List<byte> buffer;
        private CoderEvent coderEvent;
        #endregion

        #region CTOR
        /// <summary>
        /// Construcor
        /// </summary>
        /// <param name="coderEvent"> event callback </param>
        public Coder(CoderEvent coderEvent)
        {
            this.coderEvent = coderEvent;
            reset();
        }
        #endregion

        #region METHODES PUBLIC
        /// <summary>
        /// Code the input frame
        /// </summary>
        /// <param name="frame">frame tobe coded</param>
        /// <returns>coded frame</returns>
        public byte[] Write(byte[] frame)
        {
            List<byte> raw = new List<byte>();

            foreach(byte b in frame) {
                if(b == EOF)
                {
                    raw.Add(SUBS);
                    raw.Add(EOF_FLAG);
                }
                else if(b == SUBS)
                {
                    raw.Add(SUBS);
                    raw.Add(SUBS_FLAG);
                }
                else
                {
                    raw.Add(b);
                }
            }

            raw.Add(EOF);

            return raw.ToArray();
        }

        /// <summary>
        /// Decode byte after byte the input data
        /// </summary>
        /// <param name="b">input data</param>
        public void Read(byte b)
        {
            if(b == EOF)
            {
                coderEvent.NewFrame(buffer.ToArray());
                reset();
            }
            else if(b == SUBS)
            {
                lastByteIsSubs = true;
            }
            else
            {
                if(lastByteIsSubs)
                {
                    lastByteIsSubs = false;
                    if (b == EOF_FLAG)
                    {
                        buffer.Add(EOF);
                    }
                    else
                    {
                        buffer.Add(SUBS);
                    }
                }
                else
                {
                    buffer.Add(b);
                }
            }
        }
        #endregion

        #region METHODES PRIVATE
        private void reset()
        {
            lastByteIsSubs = false;
            buffer = new List<byte>();
        }
        #endregion
    }
}
