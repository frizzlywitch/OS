using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor.Vol2
{
    class RON : IWirePart
    {
        public byte Zam1 { get; private set; }
        public bool IsOkZam1 { get; private set; }
        public byte Value { get; private set; }
        public byte Prznk { get; private set; }

        public RON(Dictionary<string,int> config )
        {
            IsOkZam1 = (config["Zam1"] == 1);
        }

        public void SetWire(Command command, byte prznk)
        {
            if(IsOkZam1)
                Zam1 = (byte) (command.P == 1 ? 1 : 0);
        }

        public void SetRon(byte value, byte prznk)
        {
            if(Zam1==1)
            {
                Value = value;
                Prznk = prznk;
            }
        }
    }
}
