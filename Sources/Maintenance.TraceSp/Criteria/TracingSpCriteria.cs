namespace Maintenance.TraceSp.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class TracingSpCriteria : NonQueryCommand
    {
        public string Site { get; set; }

        public int TraceTimer { get; set; }

        public string TextDataLike { get; set; }

        public string TextDataLike2 { get; set; }

        public string TextDataLike3 { get; set; }

        public bool TraceStartEvents { get; set; }

        public bool Mode { get; set; }

        public string ErrorMsg { get; set; }

        public bool HasError { get; set; }

        public override bool IsValid()
        {
            return TraceTimer <= 120 && (!string.IsNullOrWhiteSpace(TextDataLike) || !string.IsNullOrWhiteSpace(TextDataLike2) || !string.IsNullOrWhiteSpace(TextDataLike3));
        }

        [SpParam(Ignored = true)]
        public string SuffixKey { get; set; } = string.Empty;
        public override string GetSettingKey() => "vndba_sp_trace_WebApp" + SuffixKey;
    }
}
