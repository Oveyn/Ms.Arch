using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    public abstract class DiagnosticObserverBase :
        IObserver<DiagnosticListener>
    {
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

        protected abstract bool IsMatch(string name);
        protected abstract bool IsEnabled(string name);

        void IObserver<DiagnosticListener>.OnNext(DiagnosticListener diagnosticListener)
        {
            if (IsMatch(diagnosticListener.Name))
            {
                var subscription = diagnosticListener.SubscribeWithAdapter(this, IsEnabled);
                _subscriptions.Add(subscription);
            }
        }

        void IObserver<DiagnosticListener>.OnError(Exception error)
        {
        }

        void IObserver<DiagnosticListener>.OnCompleted()
        {
            _subscriptions.ForEach(x => x.Dispose());
            _subscriptions.Clear();
        }
    }
}

