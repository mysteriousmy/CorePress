using System;
using Microsoft.Extensions.DependencyInjection;

namespace CorePress.Common.Utils;

public class AutoFacUtil
{
    private readonly IServiceProvider _serviceProvider;
    public AutoFacUtil(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public T LazyGetRequireProvider<T>(ref T value)
    {
        if (value != null)
        {
            return value;
        }
        return value = _serviceProvider.GetRequiredService<T>();
    }
}