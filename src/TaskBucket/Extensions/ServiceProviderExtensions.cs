using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class ServiceProviderExtensions
    {
        public static object GetExecutorInstance(this IServiceProvider serviceProvider, Type type, ILogger executionLogger)
        {
            ConstructorInfo? constructor = type.GetConstructors().SingleOrDefault();

            if (constructor == null)
            {
                return Activator.CreateInstance(type)!;
            }

            ParameterInfo[] parameters = constructor.GetParameters();
            object?[] parameterValues = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(ILogger))
                {
                    parameterValues[i] = executionLogger;
                }
                else if (parameters[i].ParameterType.IsGenericType && parameters[i].ParameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
                {
                    parameterValues[i] = executionLogger;
                }
                else
                {
                    if (parameters[i].HasDefaultValue)
                    {
                        parameterValues[i] = serviceProvider.GetService(parameters[i].ParameterType);
                    }
                    else
                    {
                        parameterValues[i] = serviceProvider.GetRequiredService(parameters[i].ParameterType);
                    }
                }
            }

            return Activator.CreateInstance(type, parameterValues)!;
        }
    }
}
