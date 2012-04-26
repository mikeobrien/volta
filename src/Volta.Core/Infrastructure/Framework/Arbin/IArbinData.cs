using System;
using System.Collections.Generic;

namespace Volta.Core.Infrastructure.Framework.Arbin
{
    public interface IArbinData : IDisposable
    {
        IEnumerable<AuxGlobalData> GetAuxGlobalData();
        IEnumerable<Auxiliary> GetAuxiliary();
        IEnumerable<ChannelNormal> GetChannelNormal();
        IEnumerable<ChannelStatistic> GetChannelStatistic();
        IEnumerable<Event> GetEvent();
        IEnumerable<Global> GetGlobal();
        IEnumerable<MCellAciData> GetMCellAciData();
        IEnumerable<Resume> GetResume();
        IEnumerable<SmartBatteryClockStretch> GetSmartBatteryClockStretch();
        IEnumerable<SmartBatteryData> GetSmartBatteryData();
        IEnumerable<SmartBatteryInfo> GetSmartBatteryInfo();
        Version GetVersion();
    }
}