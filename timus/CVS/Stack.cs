namespace CVS
{
    public class StackItem
    {
        public string Value;
        public StackItem Prev;
    }

    public class Stack
    {
        public StackItem tail;

        public void Push(string value)
        {
            tail = new StackItem { Value = value, Prev = tail };
        }

        public string Pop()
        {
            var result = tail.Value;
            tail = tail.Prev;
            return result;
        }
    }
}
