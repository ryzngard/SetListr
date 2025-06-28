using System;

using Avalonia;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;

namespace SetListr.Avalonia;

public static class AppExtensions
{
    public static T? GetService<T>(this Application application) where T : notnull
    {
        if (application is not App app)
        {
            throw new InvalidOperationException("This method can only be called on the App instance.");
        }

        return app.Services.GetService<T>();
    }
    public static object? GetService(this Application application, Type serviceType)
    {
        if (application is not App app)
        {
            throw new InvalidOperationException("This method can only be called on the App instance.");
        }

        return app.Services.GetService(serviceType);
    }

    public static T GetRequiredService<T>(this Application application) where T : notnull
    {
        if (application is not App app)
        {
            throw new InvalidOperationException("This method can only be called on the App instance.");
        }
        
        return app.Services.GetRequiredService<T>();
    }

    public static object GetRequiredService(this Application application, Type serviceType)
    {
        if (application is not App app)
        {
            throw new InvalidOperationException("This method can only be called on the App instance.");
        }
        
        return app.Services.GetRequiredService(serviceType);
    }
}