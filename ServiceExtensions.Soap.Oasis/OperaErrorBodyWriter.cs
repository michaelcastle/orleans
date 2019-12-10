using Microsoft.Extensions.Options;
using ServiceExtensions.Soap.Core.Response;
using System;

namespace ServiceExtensions.Soap.Oasis
{
    public class OperaErrorBodyWriter : IOperationErrorBodyWriterService
    {
        private readonly OasisSettings _config;

        public OperaErrorBodyWriter(IOptions<OasisSettings> config)
        {
            _config = config.Value;
        }

        public object GetOperationBodyWriter(Exception exception)
        {
            return new OperaResponseBody(exception, _config.ResponseIncludeErrors);
        }
    }
}
