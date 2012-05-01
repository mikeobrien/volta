using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Volta.Core.Infrastructure.Framework.Latex
{
    public class LatexException : Exception
    {
        public LatexException(string output) : base("An error occured rendering the latex document.")
        {
            Output = output;
        }

        public string Output { get; set; }
    }

    public class LatexEngine : ILatexEngine
    {
        public string GeneratePdf(string path, LatexOptions options = null)
        {
            var latexPath = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Machine).Split(';')
                .FirstOrDefault(x => File.Exists(Path.Combine(x, "latex.exe")));
            if (string.IsNullOrEmpty(latexPath)) throw new Exception("Could not find latex installation.");
            var arguments = new LatexArgumentFactory().Create(options ?? new LatexOptions(), path);
            var processInfo =
               new ProcessStartInfo(Path.Combine(latexPath, "pdflatex"))
               {
                   Arguments = arguments,
                   UseShellExecute = false,
                   RedirectStandardOutput = true
               };

            bool error;
            string output;
            using (var process = Process.Start(processInfo))
            {
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                error = process.ExitCode != 0;
            }
            var outputPath = Path.Combine(Path.GetDirectoryName(path),
                                          Path.GetFileNameWithoutExtension(path) + ".pdf");
            Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".*")
                .Where(x => x != path && x != outputPath).ToList()
                .ForEach(File.Delete);
            if (error) throw new LatexException(output);
            return outputPath;
        } 
    }
}