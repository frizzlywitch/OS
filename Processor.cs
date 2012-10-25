using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Processor.Vol2
{
    internal class Processor
    {
        public readonly Dictionary<byte, Command> Commands
            = new Dictionary<byte, Command>();
        public readonly List<IWirePart> Parts = new List<IWirePart>();
        public readonly Dictionary<string, int> Config=new Dictionary<string, int>();

        public readonly IndReg Ind;
        public readonly IP Ip;
        public readonly Memory Memory;
        public readonly ALU Alu;
        public readonly RON Ron;


        public Processor()
        {
            InitConfig();

            @Memory=new Memory(Config);
            Ind=new IndReg(Config);
            Ip=new IP(Config);
            Alu=new ALU(Config);
            Ron = new RON(Config);

            Memory.InitialiseMemory();

            Commands.Add(0, new Command {OpCode = 0, I = 0, P = 0, Op = 0});
            Commands.Add(17, new Command {OpCode = 17, I = 0, P = 1, Op = 1});
            Commands.Add(21, new Command {OpCode = 21, I = 1, P = 1, Op = 1});
            Commands.Add(2, new Command {OpCode = 2, I = 1, P = 2, Op = 0});
            Commands.Add(33, new Command {OpCode = 33, I = 0, P = 1, Op = 2});
            Commands.Add(37, new Command {OpCode = 37, I = 1, P = 1, Op = 2});
            Commands.Add(49, new Command {OpCode = 49, I = 0, P = 1, Op = 3});
            Commands.Add(254, new Branch() {OpCode = 254, I = 0, P = 4, Op = 15});
            Commands.Add(240, new BranchZero {OpCode = 240, I = 0, P = 4, Op = 15});
            Commands.Add(241, new BranchNotZero {OpCode = 241, I = 0, P = 4, Op = 15});
            Commands.Add(255, new Command {OpCode = 255, I = 0, P = 4, Op = 15});

            Parts.Add(Memory);
            Parts.Add(Ip);
            Parts.Add(Ind);
            Parts.Add(Alu);
            Parts.Add(Ron);
        }

        public void Run()
        {
            while (Ip.Pusk == 1)
            {
                var ip = Ip.Pointer;
                var opcode = Memory.GetByte(ip);
                var address = Memory.GetByte((byte)(ip + 1));
                var command = Commands[opcode];
                foreach (var wirePart in Parts)
                    wirePart.SetWire(command, Ron.Prznk);

                var effectiveAddr =(byte)( address + Ind.Value);
                var memoryValue = Memory.GetByte(effectiveAddr);

                Ip.IncOrSetPointer(effectiveAddr);

                var result = Alu.Calc(memoryValue, effectiveAddr, Ron.Value);
                var prznk = (byte) (result == 0 ? 0 : 1);

                Memory.WriteByte(result,effectiveAddr);
                Ron.SetRon(result,prznk);
                Ind.SetValue(result);
            }

            Console.WriteLine("Ir: {0}  Ron: {1}  Ip: {2}", Ind.Value.ToString("x2"), Ron.Value.ToString("x2"),
                              Ip.Pointer.ToString("x2"));

            Memory.PrintDump();
        }

        private void InitConfig()
        {
            const string fileName = @"..\..\config.txt";
            var text = File.ReadAllText(fileName);
            var substr = text.Split(' ', '\n', '\r', '\t').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            for (int i = 0; i < substr.Length; i++)
            {
                Config.Add(substr[i],int.Parse(substr[i+1]));
                ++i;
            }
        }
    }
}