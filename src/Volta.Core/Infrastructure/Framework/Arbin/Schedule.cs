using System;
using System.Collections.Generic;
using System.Linq;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Core.Infrastructure.Framework.Arbin
{
    public class Schedule
    {
        public Schedule() {}

        public Schedule(string file)
        {
            Steps = new List<Step>();
            Parse(file);
        }

        public string CurrentVersion { get; set; }
        public string Signature { get; set; }
        public int MUID { get; set; }
        public string Creator { get; set; }
        public string Comments { get; set; }
        public int MaxTimesOfUnsafe { get; set; }
        public double MinDtSeconds { get; set; }
        public bool LogChanNormalData { get; set; }
        public bool LogChanStatData { get; set; }
        public bool LogChanAuxData { get; set; }
        public bool LogSBData { get; set; }
        public bool CheckCurrentSafety { get; set; }
        public bool DefaultCurrentSafetyMode { get; set; }
        public int DefaultCurrentSafetyScopePercent { get; set; }
        public string CurrentSafetyScope { get; set; }
        public bool CheckVoltageSafety { get; set; }
        public bool DefaultVoltageSafetyMode { get; set; }
        public int DefaultVoltageSafetyScopePercent { get; set; }
        public string VoltageSafetyScope { get; set; }
        public bool CheckPowerSafety { get; set; }
        public string PowerSafetyScope { get; set; }
        public List<string> AuxSafetyEnabled { get; set; }
        public string AuxVoltSafetyScope { get; set; }
        public string AuxTempSafetyScope { get; set; }
        public string AuxPresSafetyScope { get; set; }
        public string AuxPHSafetyScope { get; set; }
        public string AuxFlowrateSafetyScope { get; set; }
        public string AuxDensitySafetyScope { get; set; }
        public int StepNum { get; set; }
        public int FormulaNum { get; set; }
        public int PulseNum { get; set; }
        public int CVNum { get; set; }
        public int AddInNum { get; set; }
        public int AppCategory { get; set; }
        public List<string> OriginalInfo { get; set; }

        public List<Step> Steps { get; set; }

        public class Step
        {
            public Step() { Limits = new List<Limit>(); }
            public int Number { get; set; }
            public int LimitNum { get; set; }
            public string Label { get; set; }
            public string StepCtrlType { get; set; }
            public string CurrentRange { get; set; }
            public string VoltageRange { get; set; }
            public string CtrlValue { get; set; }
            public string ExtCtrlValue1 { get; set; }
            public string ExtCtrlValue2 { get; set; }
            public string ExtDef { get; set; }
            public int StepCtrlTypeExtFlag { get; set; }
            public string AddIn { get; set; }
            public bool SampleClockStretch { get; set; }
            public List<Limit> Limits { get; set; } 
        }

        public class Limit
        {
            public Limit() { Equations = new List<Equation>(); }
            public int Number { get; set; }
            public bool StepLimit { get; set; }
            public bool LogDataLimit { get; set; }
            public string GotoStep { get; set; }
            public List<Equation> Equations { get; set; } 
        }

        public class Equation
        {
            public int Number { get; set; }
            public string Left { get; set; }
            public string CompareSign { get; set; }
            public string Right { get; set; }
        }

        private void Parse(string file)
        {
            var ini = new IniFile(file);
            SetValues(this, ini["Version Section"].Values);
            SetValues(this, ini["Signature Section"].Values);
            SetValues(this, ini["Schedule"].Values);
            var steps = new List<Step>();
            foreach (var section in ini.Where(x => x.Name.IsMatch(@"^Schedule_Step\d+$")))
            {
                var step = new Step();
                step.Number = int.Parse(section.Name.Match(@"^Schedule_Step(\d+)$"));
                SetValues(step, section.Values);
                steps.Add(step);
            }
            Steps = steps.OrderBy(x => x.Number).ToList();
            foreach (var section in ini.Where(x => x.Name.IsMatch(@"^Schedule_Step\d+_Limit\d+$")))
            {
                var limit = new Limit();
                limit.Number = int.Parse(section.Name.Match(@"^Schedule_Step\d+_Limit(\d+)$"));
                SetValues(limit, section.Values);
                var equationNumber = @"^Equation(\d+).*$";
                foreach (var equationValues in section.Values
                    .Where(x => x.Key.IsMatch(@"^Equation\d+.*$"))
                    .GroupBy(x => x.Key.Match(equationNumber)))
                {
                    var equation = new Equation();
                    equation.Number = int.Parse(equationValues.First().Key.Match(equationNumber));
                    SetValues(equation, equationValues.ToDictionary(x => x.Key.Match(@"^Equation\d+(.*)$"), x => x.Value));
                    limit.Equations = limit.Equations.Concat(equation).OrderBy(x => x.Number).ToList();
                }
                var step = Steps[int.Parse(section.Name.Match(@"^Schedule_Step(\d+)_Limit\d+$"))];
                step.Limits = step.Limits.Concat(limit).OrderBy(x => x.Number).ToList();
            }
        }

        private static void SetValues(object target, Dictionary<string, string> values)
        {
            if (values == null) return;
            Func<string, string, bool> namesMatch = (x, y) =>
                x.Replace("m_u", "").Replace("m_sz", "").Replace("m_f", "").Replace("m_b", "")
                .Replace("m_i", "").Replace("m_", "").Replace("_sz", "").Replace("_", "")
                .Equals(y, StringComparison.OrdinalIgnoreCase);
            var properties = target.GetType().GetProperties();
            foreach (var value in values)
            {
                var property = properties.FirstOrDefault(x => namesMatch(value.Key, x.Name));
                if (property == null) continue;
                object actualValue = null;
                if (property.PropertyType == typeof(bool)) actualValue = value.Value == "1";
                if (property.PropertyType == typeof(string)) actualValue = value.Value;
                if (property.PropertyType == typeof(double)) actualValue = double.Parse(value.Value);
                if (property.PropertyType == typeof(int)) actualValue = int.Parse(value.Value);
                if (property.PropertyType == typeof(List<string>)) 
                    actualValue = value.Value.Split(
                    new[] { (char)65533, ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).ToList();
                if (actualValue != null) property.SetValue(target, actualValue, null);
            }
        }
    }
}