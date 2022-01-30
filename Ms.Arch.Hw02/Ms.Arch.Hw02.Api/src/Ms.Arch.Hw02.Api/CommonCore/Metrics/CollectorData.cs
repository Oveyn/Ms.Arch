using System;
using System.Diagnostics;
using System.Threading;
using Ms.Arch.Hw02.Api.CommonCore.VersionsInfo;

namespace Ms.Arch.Hw02.Api.CommonCore.Metrics
{
    internal static class CollectorData
    {
        private static readonly AsyncLocal<DiagnosticCollectorData>
            _current = new AsyncLocal<DiagnosticCollectorData>();

        public static DiagnosticCollectorData Current
        {
            get
            {
                var value = _current.Value;
                if (value == null)
                    throw new InvalidOperationException("CorrelationId isn't assigned.");

                return value;
            }

            set => _current.Value = value;
        }
    }

    internal class DiagnosticCollectorData
    {
        public Guid CorrelationId { get; set; }

        public Stopwatch Stopwatch { get; set; }

        public string ServiceName { get; } = CommonRuntimeData.ApplicationName;
        public string ServiceVersion { get; } = CommonRuntimeData.ApplicationVersion;

        public string Protocol { get; set; }
        public string Handler { get; set; }
    }
}