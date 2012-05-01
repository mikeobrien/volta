namespace Volta.Core.Infrastructure.Framework.Latex
{
    public class LatexOptions
    {
        public enum InteractionMode
        {
            BatchMode,
            NonStopMode,
            ScrollMode,
            ErrorStopMode
        }

        public string Alias { get; set; }
        public string AuxDirectory { get; set; }
        public int? BufferSize { get; set; }
        public bool? CStyleErrors { get; set; }
        public bool? Disable8bitChars { get; set; }
        public bool? EnableInstaller { get; set; }
        public bool? DisableInstaller { get; set; }
        public bool? DisablePipes { get; set; }
        public bool? DisableWrite18 { get; set; }
        public bool? EnableWrite18 { get; set; }
        public bool? DontParseFirstLine { get; set; }
        public bool? ParseFirstLine { get; set; }
        public bool? DraftMode { get; set; }
        public bool? Enable8BitChars { get; set; }
        public bool? EnableEncTex { get; set; }
        public bool? EnableETex { get; set; }
        public bool? EnableMLTex { get; set; }
        public bool? EnablePipes { get; set; }
        public int? ErrorLine { get; set; }
        public int? ExtraMemBottom { get; set; }
        public int? ExtraMemTop { get; set; }
        public int? FontMax { get; set; }
        public int? FontMemSize { get; set; }
        public int? HalfErrorLine { get; set; }
        public bool? HaltOnError { get; set; }
        public int? HashExtra { get; set; }
        public string IncludeDirectory { get; set; }
        public bool? Initialize { get; set; }
        public InteractionMode? Interaction { get; set; }
        public string JobName { get; set; }
        public string JobTime { get; set; }
        public int? MainMemory { get; set; }
        public int? MaxInOpen { get; set; }
        public int? MaxPrintLine { get; set; }
        public int? MaxStrings { get; set; }
        public int? NestSize { get; set; }
        public bool? NoCStyleErrors { get; set; }
        public string OutputDirectory { get; set; }
        public string OutputFormat { get; set; }
        public int? ParamSize { get; set; }
        public int? PoolFree { get; set; }
        public int? PoolSize { get; set; }
        public bool? Quiet { get; set; }
        public string RecordPackageUsages { get; set; }
        public bool? Recorder { get; set; }
        public bool? RestrictWrite18 { get; set; }
        public int? SaveSize { get; set; }
        public bool? SourceSpecials { get; set; }
        public int? StackSize { get; set; }
        public int? StringVacancies { get; set; }
        public int? StringsFree { get; set; }
        public int? SyncTex { get; set; }
        public string Tcx { get; set; }
        public bool? TimeStatistics { get; set; }
        public string Trace { get; set; }
        public int? TrieSize { get; set; }
        public string Undump { get; set; }
        public bool? Version { get; set; }
    }
}