using System;
using System.Collections.Generic;

namespace Processor.Vol2
{
    internal class ALU : IWirePart
    {
        public byte Op { get; private set; }
        public byte Vyb { get; private set; }
        public bool IsOkOp { get; private set; }
        public bool IsOkVyb { get; private set; }

        public ALU(Dictionary<string,int> config)
        {
            IsOkOp = (config["Op"] == 1);
            IsOkVyb = (config["Vyb"] == 1);
        }

        #region IWirePart Members

        public void SetWire(Command command, byte prznk)
        {
            if (IsOkVyb)
                Vyb = command.I;
            if(IsOkOp)
                Op = command.Op;
        }

        #endregion

        public byte Calc(byte memVal, byte Ia, byte ronVal)
        {
            byte arg1 = ronVal;
            byte arg2 = (Vyb == 0 ? memVal : Ia);
            switch (Op)
            {
                case 0:
                    return arg1;
                case 1:
                    return arg2;
                case 2:
                    return (byte) (arg1 + arg2);
                case 3:
                    return (byte) (arg2 - arg1);
                case 15:
                    return arg1;
            }
            throw new Exception(string.Format("Unknown operation {0}", Op));
        }
    }
}