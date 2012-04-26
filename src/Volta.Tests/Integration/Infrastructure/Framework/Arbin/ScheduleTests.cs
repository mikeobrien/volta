using NUnit.Framework;
using Should;
using Volta.Core.Infrastructure.Framework.Arbin;

namespace Volta.Tests.Integration.Infrastructure.Framework.Arbin
{
    [TestFixture]
    public class ScheduleTests
    {
        public string File = System.IO.File.ReadAllText(@"Integration\Infrastructure\Framework\Arbin\test.sdu");

        [Test]
        public void should_parse_schedule()
        {
            var schedule = new Schedule(File);

            schedule.CurrentVersion.ShouldEqual("Schedule Version 3.03");
            schedule.Signature.ShouldEqual("{8242764E-5CEB-49CA-92E8-5AD318942418}");
            schedule.MUID.ShouldEqual(1942708548);
            schedule.Creator.ShouldEqual("USER");
            schedule.Comments.ShouldEqual("some comments");
            schedule.MaxTimesOfUnsafe.ShouldEqual(3);
            schedule.MinDtSeconds.ShouldEqual(15);
            schedule.LogChanNormalData.ShouldBeTrue();
            schedule.LogChanStatData.ShouldBeTrue();
            schedule.LogChanAuxData.ShouldBeFalse();
            schedule.LogSBData.ShouldBeFalse();
            schedule.CheckCurrentSafety.ShouldBeTrue();
            schedule.DefaultCurrentSafetyMode.ShouldBeTrue();
            schedule.DefaultCurrentSafetyScopePercent.ShouldEqual(105);
            schedule.CurrentSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.CheckVoltageSafety.ShouldBeTrue();
            schedule.DefaultVoltageSafetyMode.ShouldBeTrue();
            schedule.DefaultVoltageSafetyScopePercent.ShouldEqual(105);
            schedule.VoltageSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.CheckPowerSafety.ShouldBeFalse();
            schedule.PowerSafetyScope.ShouldEqual("0.000000^0.000000");
            var saftyEnabled = schedule.AuxSafetyEnabled;
            saftyEnabled[0].ShouldEqual("0^0");
            saftyEnabled[1].ShouldEqual("1^0");
            saftyEnabled[2].ShouldEqual("2^0");
            saftyEnabled[3].ShouldEqual("3^0");
            saftyEnabled[4].ShouldEqual("4^0");
            saftyEnabled[5].ShouldEqual("5^0");
            saftyEnabled[6].ShouldEqual("6^0");
            saftyEnabled[7].ShouldEqual("7^0");
            schedule.AuxVoltSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.AuxTempSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.AuxPresSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.AuxPHSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.AuxFlowrateSafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.AuxDensitySafetyScope.ShouldEqual("0.000000^0.000000");
            schedule.StepNum.ShouldEqual(35);
            schedule.FormulaNum.ShouldEqual(0);
            schedule.PulseNum.ShouldEqual(0);
            schedule.CVNum.ShouldEqual(0);
            schedule.AddInNum.ShouldEqual(0);
            schedule.AppCategory.ShouldEqual(0);
            schedule.OriginalInfo.Count.ShouldEqual(17);
            schedule.OriginalInfo[0].ShouldEqual("MITS PRO VERSION: 4.27 Build: 100903");
            schedule.OriginalInfo[1].ShouldEqual(@"MITS PRO INSTALLATION PATH: C:\ArbinSoftware\MITS_PRO\");
            schedule.OriginalInfo[2].ShouldEqual("FREE HARD DISK SPACE IN \"C\" DRIVE: 138591 MB");
            schedule.OriginalInfo[3].ShouldEqual("TOTAL PHYSICAL MEMORY: 2046 MB");
            schedule.OriginalInfo[4].ShouldEqual("AVAILABLE PHYSICAL MEMORY: 1461 MB");
            schedule.OriginalInfo[5].ShouldEqual("OPERATING SYSTEM: Windows 2000, Build 2195,");
            schedule.OriginalInfo[6].ShouldEqual("COMPUTER NAME: HAL");
            schedule.OriginalInfo[7].ShouldEqual("USER NAME: arbin");
            schedule.OriginalInfo[8].ShouldEqual("DAO SYSTEM: Microsoft Access 2000/2002(XP) or DAO 3.6");
            schedule.OriginalInfo[9].ShouldEqual("CUSTOMER ID: Ultraviolet Catastrophe");
            schedule.OriginalInfo[10].ShouldEqual("SERIAL NUMBER: 999999");
            schedule.OriginalInfo[11].ShouldEqual("SYSTEM CONFIG FILE VERSION: System Config Version 3.02");
            schedule.OriginalInfo[12].ShouldEqual("BATCH FILE VERSION: Batch Version 2.01");
            schedule.OriginalInfo[13].ShouldEqual("SCHEDULE FILE VERSION: Schedule Version 3.03");
            schedule.OriginalInfo[14].ShouldEqual("RESULTS FILE VERSION: Results File 5.23");
            schedule.OriginalInfo[15].ShouldEqual("FILE NAME: test.sdu");
            schedule.OriginalInfo[16].ShouldEqual("FILE CREATION TIME: 2010-12-30 14:29 (PM)");

            schedule.Steps.Count.ShouldEqual(35);

            var step = schedule.Steps[0];

            step.Number.ShouldEqual(0);
            step.LimitNum.ShouldEqual(3);
            step.Label.ShouldEqual("Heating up, OCV");
            step.StepCtrlType.ShouldEqual("Rest");
            step.CurrentRange.ShouldEqual("High");
            step.VoltageRange.ShouldEqual("High");
            step.CtrlValue.ShouldEqual("");
            step.ExtCtrlValue1.ShouldEqual("");
            step.ExtCtrlValue2.ShouldEqual("");
            step.ExtDef.ShouldEqual("");
            step.StepCtrlTypeExtFlag.ShouldEqual(0);
            step.AddIn.ShouldEqual("");
            step.SampleClockStretch.ShouldBeFalse();

            step.Limits.Count.ShouldEqual(3);

            var limit = step.Limits[0];

            limit.Number.ShouldEqual(0);
            limit.StepLimit.ShouldBeTrue();
            limit.LogDataLimit.ShouldBeTrue();
            limit.GotoStep.ShouldEqual("Current step, 0.1A");

            var equation = limit.Equations[0];
            equation.Number.ShouldEqual(0);
            equation.Left.ShouldEqual("PV_CHAN_Test_Time");
            equation.CompareSign.ShouldEqual(">=");
            equation.Right.ShouldEqual("86400");

            equation = limit.Equations[1];
            equation.Number.ShouldEqual(1);
            equation.Left.ShouldEqual("");
            equation.CompareSign.ShouldEqual("");
            equation.Right.ShouldEqual("");

            equation = limit.Equations[2];
            equation.Number.ShouldEqual(2);
            equation.Left.ShouldEqual("");
            equation.CompareSign.ShouldEqual("");
            equation.Right.ShouldEqual("");

            limit = step.Limits[1];
            limit.Number.ShouldEqual(1);
            limit.StepLimit.ShouldBeTrue();
            limit.LogDataLimit.ShouldBeTrue();
            limit.GotoStep.ShouldEqual("Next Step");

            limit = step.Limits[2];
            limit.Number.ShouldEqual(2);
            limit.StepLimit.ShouldBeFalse();
            limit.LogDataLimit.ShouldBeTrue();
            limit.GotoStep.ShouldEqual("Next Step");

            step = schedule.Steps[34];

            step.Number.ShouldEqual(34);
            step.LimitNum.ShouldEqual(2);
            step.Label.ShouldEqual("OCV final");
            step.StepCtrlType.ShouldEqual("Rest");
            step.CurrentRange.ShouldEqual("High");
            step.VoltageRange.ShouldEqual("High");
            step.CtrlValue.ShouldEqual("");
            step.ExtCtrlValue1.ShouldEqual("");
            step.ExtCtrlValue2.ShouldEqual("");
            step.ExtDef.ShouldEqual("");
            step.StepCtrlTypeExtFlag.ShouldEqual(0);
            step.AddIn.ShouldEqual("");
            step.SampleClockStretch.ShouldBeFalse();

            limit = step.Limits[0];

            limit.Number.ShouldEqual(0);
            limit.StepLimit.ShouldBeTrue();
            limit.LogDataLimit.ShouldBeTrue();
            limit.GotoStep.ShouldEqual("End Test");

            limit = step.Limits[1];
            limit.Number.ShouldEqual(1);
            limit.StepLimit.ShouldBeFalse();
            limit.LogDataLimit.ShouldBeTrue();
            limit.GotoStep.ShouldEqual("Next Step");
        }
    }
}