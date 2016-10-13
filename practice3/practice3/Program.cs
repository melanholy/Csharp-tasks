namespace practice3
{
    internal class Program
    {
        private static void Main()
        {
            Processor.CreateEngine<Engine>().For<Entity>().With<Logger>();
        }
    }
}
