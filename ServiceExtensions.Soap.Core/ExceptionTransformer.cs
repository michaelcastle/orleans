using System;

namespace ServiceExtensions.Soap.Core
{
    internal class ExceptionTransformer
    {
        private readonly Func<Exception, string> _transformer;

        public ExceptionTransformer(Func<Exception, string> transformer)
        {
            _transformer = transformer;
        }

        public string Transform(Exception ex)
        {
            return _transformer(ex);
        }
    }
}
