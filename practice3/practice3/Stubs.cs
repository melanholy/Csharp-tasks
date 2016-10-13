namespace practice3
{
    public interface IEngine { }

    public interface IEntity { }

    public interface ILogger { }

    public class Engine : IEngine { }

    public class Logger : ILogger { }

    public class Entity : IEntity { }
}