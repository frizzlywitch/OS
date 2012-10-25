using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor.Vol2
{
    class IndReg : IWirePart
    {
        public byte Chist { get; private set; }
        public byte Zam2 { get; private set; }
        public byte Value { get; private set; }

        public bool IsOkChist { get; private set; }
        public bool IsOkZam2 { get; private set; }

        public IndReg(Dictionary<string,int> config)
        {
            IsOkChist = (config["Chist"] == 1);
            IsOkZam2 = (config["Zam2"] == 1);
        }

        public void SetWire(Command command, byte prznk)
        {
            if(IsOkChist)
                Chist = (byte) ((command.P != 2 && command.P!=3) ? 1 : 0);
            if (IsOkZam2)
                Zam2 = (byte) (command.P != 3 ? 1 : 0);
        }

        public void SetValue(byte value)
        {
            if(Zam2==1)
            {
                if (Chist == 0)
                    Value = value;
                else
                    Value = 0;
            }
        }
    }
}
