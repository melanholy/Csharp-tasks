using System;

namespace practice3
{
    public class Processor<TEngine, TEntity, TLogger> 
        where TEngine : IEngine 
        where TEntity : IEntity 
        where TLogger : ILogger
    {
        private readonly TEngine Engine;
        private readonly TEntity Entity;
        private readonly TLogger Logger;

        public Processor(TEngine engine, TEntity entity, TLogger logger)
        {
            Engine = engine;
            Entity = entity;
            Logger = logger;
        }

        public void Process()
        {
            Console.WriteLine("{0}, {1}, {2}", Engine, Entity, Logger);
        }
    }

    public static class Processor
    {
        public static ForEngineBuilder<T> CreateEngine<T>() 
            where T : IEngine, new()
        {
            return new ForEngineBuilder<T>(new T());
        }
    }
}
