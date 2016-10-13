namespace practice3
{
    public class ForEngineBuilder<TEngine>
        where TEngine : IEngine
    {
        private readonly TEngine Engine;

        public ForEngineBuilder(TEngine engine)
        {
            Engine = engine;
        }

        public WithEngineBuilder<TEngine, TEntity> For<TEntity>() 
            where TEntity : IEntity, new()
        {
            return new WithEngineBuilder<TEngine, TEntity>(Engine, new TEntity());
        }
    }
}
