using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor.Vol2
{
    class Command
    {
        public byte OpCode;
        public byte I;
        public byte P;
        public byte Op;

        public virtual byte Pereh(byte prznk)
        {
            return 0;
        }
    }

    class Branch : Command
    {
         public override byte Pereh(byte prznk)
         {
             return (byte) 1;
         }
    }

    class BranchZero : Command
    {
        public override byte Pereh(byte prznk)
        {
            return (byte) (prznk == 0 ? 1 : 0);
        }
    }

    class BranchNotZero : Command
    {
        public override byte Pereh(byte prznk)
        {
            return (byte) (prznk != 0 ? 1 : 0);
        }
    }
}
