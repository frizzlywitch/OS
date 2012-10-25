using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor.Vol2
{
    class IP : IWirePart
    {
        public IP(Dictionary<string,int> config)
        {
            IsOkPereh = (config["Pereh"] == 1);
            IsOkPusk = (config["Pusk"] == 1);
            if (IsOkPusk)
                Pusk = 1;
        }

        public byte Pusk { get; private set; }
        public byte Pereh { get; private set; }
        public byte Pointer { get; private set; }

        public bool IsOkPusk { get; private set; }
        public bool IsOkPereh { get; private set; }

        public void IncOrSetPointer(byte value)
        {
            if(Pereh==1)
            {
                Pointer = value;
                return;
            }

            Pointer += 2;
        }

        public void SetWire(Command command, byte prznk)
        {
            if(IsOkPusk)
                Pusk = (byte) (command.OpCode != 255 ? 1 : 0);
            if(IsOkPereh)
                Pereh = command.Pereh(prznk);
        }
    }
}
