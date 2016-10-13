using System;
using System.Collections.Generic;
using System.Linq;

namespace func.petooh
{
	public class Petooh
	{
		public static void Run(string program, Func<int> read, Action<char> write)
		{
			var runner = new ProgramRunner(program);
			runner.RegisterCommand('<', b => {
                b.Pointer--;
            });
			runner.RegisterCommand('>', b => {
                b.Pointer++;
            });
			runner.RegisterCommand('+', b => {
                b.Memory[b.Pointer]++;
            });
			runner.RegisterCommand('-', b => {
                b.Memory[b.Pointer]--;
            });
			runner.RegisterCommand('.', b => {
                write((char)b.Memory[b.Pointer]);
            });
			runner.RegisterCommand(',', b => {
                b.Memory[b.Pointer] = (byte)read();
            });

			runner.RegisterCommand('[', b => {
                if (b.Memory[b.Pointer] == 0)
                    b.CommandPointer = b.Loops[b.CommandPointer];
            });
			runner.RegisterCommand(']', b => {
                if (b.Memory[b.Pointer] != 0)
                    b.CommandPointer = b.Loops[b.CommandPointer];
            });

            for (var i = 'a'; i <= 'z'; i++ )
            {
                var c = (byte)i;
                runner.RegisterCommand(i, b =>
                {
                    b.Memory[b.Pointer] = c;
                });
            }
            for (var i = 'A'; i <= 'Z'; i++)
            {
                var c = (byte)i;
                runner.RegisterCommand(i, b =>
                {
                    b.Memory[b.Pointer] = c;
                });
            }
            for (var i = '0'; i <= '9'; i++)
            {
                var c = (byte)i;
                runner.RegisterCommand(i, b =>
                {
                    b.Memory[b.Pointer] = c;
                });
            }
            runner.Run();
		}
	}


	public class ProgramRunner
	{
        public byte[] Memory { get; set; }
        Dictionary<char, Action<ProgramRunner>> Commands;
        public readonly string Program;
        private int pointer;
        public int CommandPointer { get; set; }
        public Dictionary<int, int> Loops { get; set; }

        public int Pointer
        {
            get { return pointer; }
            set
            {
                pointer = value;
                if (pointer < 0) pointer += 30000;
                if (pointer >= 30000) pointer -= 30000;
            }
        }

		public ProgramRunner(string program)
		{
            Loops = new Dictionary<int, int>();
            var memory = new Stack<int>();
            Program = program.Replace("\r\n", "").Replace("Слышь че", ",").Replace(" ", "").Replace("Лево", "<").Replace("Право", ">")
                             .Replace("Внатуре", "+").Replace("Типа", "-").Replace("Базарю", ".").Replace("Сюда", "[").Replace("Туда", "]");
            for (var i = 0; i<Program.Length; i++)
            {
                if (Program[i] == '[')
                    memory.Push(i);
                else if (Program[i] == ']')
                {
                    Loops[i] = memory.ElementAt(0);
                    Loops[memory.Pop()] = i;
                }
            }
            Memory = new byte[30000];
            Commands = new Dictionary<char, Action<ProgramRunner>>();
            pointer = 0;
		}

		public void RegisterCommand(char name, Action<ProgramRunner> executeCommand)
		{
            Commands.Add(name, executeCommand);
		}

		public void Run()
		{
            for (CommandPointer = 0; CommandPointer < Program.Length; CommandPointer++)
            {
                Commands[Program[CommandPointer]](this);
            }
		}
	}
}