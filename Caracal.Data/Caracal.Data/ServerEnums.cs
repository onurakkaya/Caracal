using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Data
{
    public enum ServerLoginMode
    {
        Integrated = 1,
        Mixed = 2,
        Normal = 0,
        Unknown = 9
    }
    public enum ServerLoginType
    {
        AsymmetricKey = 4,
        Certificate = 3,
        ExternalGroup = 6,
        ExternalUser = 5,
        SqlLogin = 2,
        WindowsGroup = 1,
        WindowsUser = 0
    }
    public enum ServerPasswordHashAlgorithm
    {
        None = 0,
        ShaOne = 2,
        ShaTwo = 3,
        SqlServer7 = 1
    }
    public enum XState
    {
        Creating = 1,
        Dropped = 4,
        Existing = 2,
        Pending = 0,
        ToBeDropped = 3
    }
    public enum XWindowsLoginAccessType
    {
        Deny = 2,
        Grant = 1,
        NotNtLogin = 99,
        NotDefined = 0
    }
    public enum XStatus
    {
        Offline = 16,
        OfflinePending = 48,
        Online = 1,
        OnlinePending = 3,
        Unknown = 0
    }
    public enum XServiceStartMode
    {
        Auto = 2,
        Boot = 0,
        Disabled = 4,
        Manual = 3,
        System = 1
    }
    public enum ServerEdition
    {
        EnterpriseOrDeveloper = 3,
        Express = 4,
        PersonalOrDesktopEngine = 1,
        SqlDatabase = 5,
        SqlWarehouse = 6,
        SqlStrechDatabase = 7,
        Standart = 2,
        Unknown = 0
    }
}
