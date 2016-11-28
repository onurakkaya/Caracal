using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Data
{
    public enum DbEngineEdition
    {
        Enterprise = 3,
        Express = 4,
        Personal = 1,
        SqlDatabase = 5,
        SqlDataWarehouse = 6,
        SqlStrechDatabase = 7,
        Standart = 2,
        Unknown = 0
    }
    public enum DbEngineType
    {
        SqlAzureDatabase = 2,
        Standalone = 1,
        Unknown = 0
    }
    public enum DbStatus
    {
        AutoClosed = 512,
        EmergencyMode = 256,
        Normal = 1,
        Offline = 32,
        Recovering = 8,
        RecoveryPending = 4,
        Restoring = 2,
        Shutdown = 128,
        Standby = 64,
        Suspect = 16
    }
    public enum DbShrinkMethod
    {
        Default = 0,
        EmptyFile = 3,
        NoTruncate = 1,
        TruncateOnly = 2
    }
    public enum DbCompatibilityLevel
    {
        Lower = 60 | 65 | 70,
        SqlServer2000 = 80,
        SqlServer2005 = 90,
        SqlServer2008 = 100,
        SqlServer2012 = 110,
        SqlServer2014 = 120,
        SqlServer2016 = 130
    }
}
