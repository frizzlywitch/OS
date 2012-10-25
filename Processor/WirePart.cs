using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor.Vol2
{
    interface IWirePart
    {
        void SetWire(Command command, byte prznk);
    }
}
