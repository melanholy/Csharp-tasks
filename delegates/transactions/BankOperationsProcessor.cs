using System;

namespace transactions
{
    public class BankOperationsProcessor<TRequest, TOperation>
        where TRequest : IBankRequest 
        where TOperation : IBankOperation
    {
        public TOperation Process(TRequest request)
        {
            if (!Check(request))
                throw new ArgumentException();
            var result = Register(request);
            Save(result);
            return result;
        }

        public BankOperationsProcessor(
            Func<TRequest, bool> checker,
            Func<TRequest, TOperation> registrator,
            Action<TOperation> saver)
        {
            Check = checker;
            Register = registrator;
            Save = saver;
        }

        private readonly Func<TRequest, bool> Check;
        private readonly Func<TRequest, TOperation> Register;
        private readonly Action<TOperation> Save;
    }
}