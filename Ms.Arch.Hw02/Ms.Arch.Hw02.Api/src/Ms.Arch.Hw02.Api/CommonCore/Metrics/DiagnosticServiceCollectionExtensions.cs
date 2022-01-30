using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    public static class DiagnosticServiceCollectionExtensions
    {
        public static void AddDiagnosticObserver<TDiagnosticObserver>(
            this IServiceCollection services)
            where TDiagnosticObserver : DiagnosticObserverBase
        {
            services.TryAddEnumerable(ServiceDescriptor
                .Transient<DiagnosticObserverBase, TDiagnosticObserver>());
        }
    }
}