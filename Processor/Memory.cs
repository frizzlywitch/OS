using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Processor.Vol2
{
    class Memory : IWirePart
    {
        public const int Size = 256;
        public byte Zapp { get; private set; }
        public bool IsOkZapp { get; private set; }
        
        private byte[] _memoryCells;

        public Memory(Dictionary<string,int> config)
        {
            IsOkZapp = (config["Zapp"] == 1);
        }

        public void SetWire(Command command, byte prznk)
        {
            if(IsOkZapp)
                Zapp = (byte) (command.P == 0 ? 1 : 0);
        }

        public void WriteByte(byte value, byte pos)
        {
            if(Zapp==1)
                _memoryCells[pos] = value;
        }

        public byte GetByte(byte pos)
        {
            return _memoryCells[pos];
        }

        public void PrintDump()
        {
            byte pos = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Console.Write(_memoryCells[pos].ToString("X2") + " ");
                    ++pos;
                }
                Console.WriteLine();
            }
        }

        public void InitialiseMemory()
        {
            const string fileName = @"..\..\memory.txt";
            _memoryCells=new byte[Size];

            var text = File.ReadAllText(fileName);
            var substrs = text.Split(' ','\n','\r','\t').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            for (int i = 0; i < substrs.Length && i<Size; i++)
                _memoryCells[i] = Convert.ToByte(substrs[i], 16);
        }
    }
}
