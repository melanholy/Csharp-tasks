using System;
using System.Collections.Generic;

namespace CVS
{
    public class Clone
    {
        public Stack LearnHistory = new Stack();
        public Stack ForgotHistory = new Stack();

        public Clone(Stack LH, Stack FH)
        {
            LearnHistory.tail = LH.tail;
            ForgotHistory.tail = FH.tail;
        }

        public Clone() { }

        public void Learn(string program)
        {
            LearnHistory.Push(program);
        }

        public void Rollback()
        {
            ForgotHistory.Push(LearnHistory.Pop());
        }

        public void Relearn()
        {
            LearnHistory.Push(ForgotHistory.Pop());
        }

        public void Check()
        {
            if (LearnHistory.tail == null) Console.WriteLine("basic");
            else Console.WriteLine(LearnHistory.tail.Value);
        }

        public void Clone(List<Clone> clones)
        {
            clones.Add(new Clone(LearnHistory, ForgotHistory));
        }
    }
}
