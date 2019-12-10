using System;

namespace ServiceExtensions.Soap.Core.Response
{
    public interface IOperationErrorBodyWriterService
    {
        object GetOperationBodyWriter(Exception exception);
    }
}
