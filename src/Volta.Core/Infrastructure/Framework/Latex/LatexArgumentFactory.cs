using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Volta.Core.Infrastructure.Framework.Latex
{
    public class LatexArgumentFactory
    {
        private static readonly Dictionary<PropertyInfo, string> Options = new Dictionary<PropertyInfo, string>();

        private static void AddOption<T>(Expression<Func<LatexOptions, T>> property, string option)
        {
            Options.Add(((PropertyInfo)((MemberExpression)property.Body).Member), option);
        }

        static LatexArgumentFactory()
        {
            AddOption(x => x.Alias, "alias");
            AddOption(x => x.AuxDirectory, "aux-directory");
            AddOption(x => x.BufferSize, "buf-size");
            AddOption(x => x.CStyleErrors, "c-style-errors");
            AddOption(x => x.Disable8bitChars, "disable-8bit-chars");
            AddOption(x => x.EnableInstaller, "enable-installer");
            AddOption(x => x.DisableInstaller, "disable-installer");
            AddOption(x => x.DisablePipes, "disable-pipes");
            AddOption(x => x.DisableWrite18, "disable-write18");
            AddOption(x => x.EnableWrite18, "enable-write18");
            AddOption(x => x.DontParseFirstLine, "dont-parse-first-line");
            AddOption(x => x.ParseFirstLine, "parse-first-line");
            AddOption(x => x.DraftMode, "draftmode");
            AddOption(x => x.Enable8BitChars, "enable-8bit-chars");
            AddOption(x => x.EnableEncTex, "enable-enctex");
            AddOption(x => x.EnableETex, "enable-etex");
            AddOption(x => x.EnableMLTex, "enable-mltex");
            AddOption(x => x.EnablePipes, "enable-pipes");
            AddOption(x => x.ErrorLine, "error-line");
            AddOption(x => x.ExtraMemBottom, "extra-mem-bot");
            AddOption(x => x.ExtraMemTop, "extra-mem-top");
            AddOption(x => x.FontMax, "font-max");
            AddOption(x => x.FontMemSize, "font-mem-size");
            AddOption(x => x.HalfErrorLine, "half-error-line");
            AddOption(x => x.HaltOnError, "halt-on-error");
            AddOption(x => x.HashExtra, "hash-extra");
            AddOption(x => x.IncludeDirectory, "include-directory");
            AddOption(x => x.Initialize, "initialize");
            AddOption(x => x.Interaction, "interaction");
            AddOption(x => x.JobName, "job-name");
            AddOption(x => x.JobTime, "job-time");
            AddOption(x => x.MainMemory, "main-memory");
            AddOption(x => x.MaxInOpen, "max-in-open");
            AddOption(x => x.MaxPrintLine, "max-print-line");
            AddOption(x => x.MaxStrings, "max-strings");
            AddOption(x => x.NestSize, "nest-size");
            AddOption(x => x.NoCStyleErrors, "no-c-style-errors");
            AddOption(x => x.OutputDirectory, "output-directory");
            AddOption(x => x.OutputFormat, "output-format");
            AddOption(x => x.ParamSize, "param-size");
            AddOption(x => x.PoolFree, "pool-free");
            AddOption(x => x.PoolSize, "pool-size");
            AddOption(x => x.Quiet, "quiet");
            AddOption(x => x.RecordPackageUsages, "record-package-usages");
            AddOption(x => x.Recorder, "recorder");
            AddOption(x => x.RestrictWrite18, "restrict-write18");
            AddOption(x => x.SaveSize, "save-size");
            AddOption(x => x.SourceSpecials, "src-specials");
            AddOption(x => x.StackSize, "stack-size");
            AddOption(x => x.StringVacancies, "string-vacancies");
            AddOption(x => x.StringsFree, "strings-free");
            AddOption(x => x.SyncTex, "synctex");
            AddOption(x => x.Tcx, "tcx");
            AddOption(x => x.TimeStatistics, "time-statistics");
            AddOption(x => x.Trace, "trace");
            AddOption(x => x.TrieSize, "trie-size");
            AddOption(x => x.Undump, "undump");
            AddOption(x => x.Version, "version");
        }

        public string Create(LatexOptions options, string path)
        {
            var arguments = Options
                .Select(x => new { Name = x.Value, Value = x.Key.GetValue(options, null) })
                .Where(x => x.Value != null && (!(x.Value is bool?) ||
                            (x.Value is bool? && ((bool?)x.Value) == true)))
                .Select(x => x.Value is bool? ?
                    string.Format("-{0}", x.Name) :
                    string.Format("-{0}={1}", x.Name, x.Value));
            return arguments.Any() ?
                string.Format("{0} {1}", arguments.Aggregate((i, a) => i + " " + a), path) :
                path;
        }
    }
}