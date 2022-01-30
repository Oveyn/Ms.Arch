namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    public abstract class AspNetCoreDiagnosticObserver : DiagnosticObserverBase
    {
        protected override bool IsMatch(string name)
        {
            return name == "Microsoft.AspNetCore" || name == "HttpHandlerDiagnosticListener";
        }

        protected override bool IsEnabled(string name)
        {
            return true;
        }
    }
}