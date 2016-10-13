namespace practice3
{
    public class WithEngineBuilder<TEngine, TEntity>
        where TEngine : IEngine
        where TEntity : IEntity
    {
        private readonly TEngine Engine;
        private readonly TEntity Entity;

        public WithEngineBuilder(TEngine engine, TEntity entity)
        {
            Engine = engine;
            Entity = entity;
        }

        public Processor<TEngine, TEntity, TLogger> With<TLogger>()
            where TLogger : ILogger, new()
        {
            return new Processor<TEngine, TEntity, TLogger>(Engine, Entity, new TLogger());
        }
    }
}
