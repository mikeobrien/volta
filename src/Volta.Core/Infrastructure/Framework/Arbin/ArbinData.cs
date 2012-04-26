using System;
using System.Collections.Generic;
using System.Linq;
using Volta.Core.Infrastructure.Framework.Data;

namespace Volta.Core.Infrastructure.Framework.Arbin
{
    public class ArbinData : IDisposable
    {
        private readonly JetQuery _query;

        public ArbinData(string path)
        {
            _query = new JetQuery(path);
        }

        public IEnumerable<AuxGlobalData> GetAuxGlobalData() { return _query.Table<AuxGlobalData>(); }
        public IEnumerable<Auxiliary> GetAuxiliary() { return _query.Table<Auxiliary>(); }
        public IEnumerable<ChannelNormal> GetChannelNormal() { return _query.Table<ChannelNormal>(); }
        public IEnumerable<ChannelStatistic> GetChannelStatistic() { return _query.Table<ChannelStatistic>(); }
        public IEnumerable<Event> GetEvent() { return _query.Table<Event>(); }
        public IEnumerable<Global> GetGlobal() { return _query.Table<Global>(); }
        public IEnumerable<MCellAciData> GetMCellAciData() { return _query.Table<MCellAciData>(); }
        public IEnumerable<Resume> GetResume() { return _query.Table<Resume>(); }
        public IEnumerable<SmartBatteryClockStretch> GetSmartBatteryClockStretch() { return _query.Table<SmartBatteryClockStretch>(); }
        public IEnumerable<SmartBatteryData> GetSmartBatteryData() { return _query.Table<SmartBatteryData>(); }
        public IEnumerable<SmartBatteryInfo> GetSmartBatteryInfo() { return _query.Table<SmartBatteryInfo>(); }
        public Version GetVersion() { return _query.Table<Version>().First(); }

        public void Dispose()
        {
            _query.Dispose();
        }

        public class AuxGlobalData 
        {
	        public short ChannelIndex { get; set; }
	        public short AuxiliaryIndex { get; set; }
	        public short DataType { get; set; }
	        public string Nickname { get; set; }
	        public string Unit { get; set; }
        }

        public class Auxiliary 
        {
	        public int TestId { get; set; }
	        public int DataPoint { get; set; }
	        public short AuxiliaryIndex { get; set; }
	        public short DataType { get; set; }
	        public float X { get; set; }
        }

        public class ChannelNormal 
        {
	        public int TestId { get; set; }
	        public int DataPoint { get; set; }
	        public double TestTime { get; set; }
	        public double StepTime { get; set; }
	        public double DateTime { get; set; }
	        public short StepIndex { get; set; }
	        public short CycleIndex { get; set; }
	        public byte IsFCData { get; set; }
	        public float Current { get; set; }
	        public float Voltage { get; set; }
	        public double ChargeCapacity { get; set; }
	        public double DischargeCapacity { get; set; }
	        public double ChargeEnergy { get; set; }
	        public double DischargeEnergy { get; set; }
	        public float dVdt { get; set; }
	        public float InternalResistance { get; set; }
	        public float ACImpedance { get; set; }
	        public float ACIPhaseAngle { get; set; }
        }

        public class ChannelStatistic 
        {
	        public int TestId { get; set; }
	        public int DataPoint { get; set; }
	        public float VmaxOnCycle { get; set; }
	        public double ChargeTime { get; set; }
	        public double DischargeTime { get; set; }
        }

        public class Event 
        {
	        public int TestId { get; set; }
	        public double DateTime { get; set; }
	        public double TestTime { get; set; }
	        public short EventType { get; set; }
	        public string EventDescribe { get; set; }
        }

        public class Global 
        {
	        public int TestId { get; set; }
	        public string TestName { get; set; }
	        public short ChannelIndex { get; set; }
	        public double StartDateTime { get; set; }
	        public short DAQIndex { get; set; }
	        public short ChannelType { get; set; }
	        public string Creator { get; set; }
	        public string Comments { get; set; }
	        public string ScheduleFileName { get; set; }
	        public short ChannelNumber { get; set; }
	        public short MappedAuxVoltageNumber { get; set; }
	        public short MappedAuxTemperatureNumber { get; set; }
	        public short MappedAuxPressureNumber { get; set; }
	        public short MappedAuxPHNumber { get; set; }
	        public short MappedAuxFlowRateCNumber { get; set; }
	        public string ApplicationsPath { get; set; }
	        public bool LogChanStatDataFlag { get; set; }
	        public bool LogAuxDataFlag { get; set; }
	        public bool LogEventDataFlag { get; set; }
	        public bool LogSmartBatteryDataFlag { get; set; }
	        public string ItemId { get; set; }
	        public short MappedAuxConcCNumber { get; set; }
	        public short MappedAuxDICNumber { get; set; }
	        public short MappedAuxDOCNumber { get; set; }
        }

        public class MCellAciData 
        {
	        public int TestId { get; set; }
	        public int DataPoint { get; set; }
	        public short CellIndex { get; set; }
	        public float ACI { get; set; }
	        public float PhaseShift { get; set; }
	        public float Voltage { get; set; }
	        public float Current { get; set; }
        }

        public class Resume 
        {
	        public int TestId { get; set; }
	        public short StepIndex { get; set; }
	        public short CycleIndex { get; set; }
	        public short ChannelStatus { get; set; }
	        public double TestTime { get; set; }
	        public double StepTime { get; set; }
	        public double CCapacity { get; set; }
	        public double DCapacity { get; set; }
	        public double CEnergy { get; set; }
	        public double DEnergy { get; set; }
	        public double[] TCTime { get; set; }
	        public double[] TCCCapacity { get; set; }
	        public double[] TCDCapacity { get; set; }
	        public double[] TCCEnergy { get; set; }
	        public double[] TCDEnergy { get; set; }
	        public float[] MVCounter { get; set; }
	        public double ChargeTime { get; set; }
	        public double DischargeTime { get; set; }
        }

        public class SmartBatteryClockStretch 
        {
	        public int TestId { get; set; }
	        public int DataPoint { get; set; }
	        public int ManufacturerAccess { get; set; }
	        public int RemainingCapacityAlarm { get; set; }
	        public int RemainingTimeAlarm { get; set; }
	        public int BatteryMode { get; set; }
	        public int AtRate { get; set; }
	        public int AtRateTimeToFull { get; set; }
	        public int AtRateTimeToEmpty { get; set; }
	        public int AtRateOk { get; set; }
	        public int Temperature { get; set; }
	        public int Voltage { get; set; }
	        public int Current { get; set; }
	        public int AverageCurrent { get; set; }
	        public int MaxError { get; set; }
	        public int RelativeStateOfCharge { get; set; }
	        public int AbsoluteStateOfCharge { get; set; }
	        public int RemainingCapacity { get; set; }
	        public int FullChargeCapacity { get; set; }
	        public int RunTimeToEmpty { get; set; }
	        public int AverageTimeToEmpty { get; set; }
	        public int AverageTimeToFull { get; set; }
	        public int ChargingCurrent { get; set; }
	        public int ChargingVoltage { get; set; }
	        public int BatteryStatus { get; set; }
	        public int CycleCount { get; set; }
	        public int DesignCapacity { get; set; }
	        public int DesignVoltage { get; set; }
	        public int SpecificationInfo { get; set; }
	        public int ManufacturerDate { get; set; }
	        public int SerialNumber { get; set; }
	        public int ManufacturerName { get; set; }
	        public int DeviceName { get; set; }
	        public int DeviceChemistry { get; set; }
	        public int ManufacturerData { get; set; }
	        public int PackStatus { get; set; }
	        public int PackConfiguration { get; set; }
	        public int[] VCELL { get; set; }
        }

        public class SmartBatteryData 
        {
	        public int TestId { get; set; }
	        public int DataPoint { get; set; }
	        public float RemainingCapacityAlarm { get; set; }
	        public int RemainingTimeAlarm { get; set; }
	        public int BatteryMode { get; set; }
	        public float AtRate { get; set; }
	        public int AtRateTimeToFull { get; set; }
	        public int AtRateTimeToEmpty { get; set; }
	        public int AtRateOk { get; set; }
	        public float Temperature { get; set; }
	        public float Voltage { get; set; }
	        public float Current { get; set; }
	        public float AverageCurrent { get; set; }
	        public int MaxError { get; set; }
	        public int RelativeStateOfCharge { get; set; }
	        public int AbsoluteStateOfCharge { get; set; }
	        public float RemainingCapacity { get; set; }
	        public int RunTimeToEmpty { get; set; }
	        public int AverageTimeToEmpty { get; set; }
	        public int AverageTimeToFull { get; set; }
	        public int BatteryStatus { get; set; }
	        public int CycleCount { get; set; }
	        public int PackStatus { get; set; }
	        public int PackConfiguration { get; set; }
	        public float[] VCELL { get; set; }
	        public string ManufacturerAccess { get; set; }
	        public float FullChargeCapacity { get; set; }
	        public int BroadCast { get; set; }
	        public int[] GPIO { get; set; }
	        public float[] OptVCELL { get; set; }
	        public int[] OMF { get; set; }
	        public int FTEMP { get; set; }
	        public int STATUS { get; set; }
	        public int FETTEMP { get; set; }
	        public float ChargingCurrent { get; set; }
	        public float ChargingVoltage { get; set; }
	        public float ManufacturerData { get; set; }
        }

        public class SmartBatteryInfo 
        {
	        public int TestId { get; set; }
	        public double ManufacturerDate { get; set; }
	        public string ManufacturerAccess { get; set; }
	        public string SpecificationInfo { get; set; }
	        public float FullChargeCapacity { get; set; }
	        public float ChargingCurrent { get; set; }
	        public float ChargingVoltage { get; set; }
	        public float DesignCapacity { get; set; }
	        public float DesignVoltage { get; set; }
	        public int SerialNumber { get; set; }
	        public string ManufacturerName { get; set; }
	        public string DeviceName { get; set; }
	        public string DeviceChemistry { get; set; }
	        public string ManufacturerData { get; set; }
	        public int Frequency { get; set; }
        }

        public class Version 
        {
	        public string VersionSchemaField { get; set; }
	        public string VersionCommentsField { get; set; }
        }
    }
}