using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortCom
{
    /// <summary>
    /// Event callback methode for the coder class
    /// </summary>
    public interface CoderEvent
    {
        /// <summary>
        /// Callback new frame are ready
        /// </summary>
        /// <param name="frame"></param>
        void NewFrame(byte[] frame);
    }
}
