using System;
using System.Collections.Generic;

namespace ServiceExtensions.Soap.Core
{
    public interface IModelBindingFilter
    {
        List<Type> ModelTypes { get; set; }
        void OnModelBound(object model, IServiceProvider serviceProvider, out object output);
    }
}
