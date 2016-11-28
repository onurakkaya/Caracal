using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using x = Microsoft.SqlServer.Management.Smo;
namespace Caracal.Data
{
    public class Database
    {
        internal x.Database baseObject;
        public int ActiveConnectionCount { get; set; }
        public bool AnsiNullDefault { get; set; }
        public bool AnsiNullsEnabled { get; set; }
        public bool AnsiPaddingEnabled { get; set; }
        public bool AnsiWarningsEnabled { get; set; }
        public bool ArithmeticAbortEnabled { get; set; }
        public bool AutoClose { get; set; }
        public bool AutoCreateIncrementalStatisticsEnabled { get; set; }
        public bool AutoCreateStatisticsEnabled { get; set; }
        public bool AutoShrink { get; set; }
        public bool AutoUpdateStatisticsEnabled { get; set; }
        public bool CaseSensitive { get; set; }
        public string Collation { get; set; }
        public DbCompatibilityLevel CompatibilityLevel { get; set; }
        public DateTime CreateDate { internal set; get; }
        public DbEngineEdition DatabaseEngineEdition { internal set; get; }
        public DbEngineType DatabaseEngineType { get; set; }
        public Guid DatabaseGuid { internal set; get; }
        public double DataSpaceUsage { internal set; get; }
        public bool EncrpytionEnabled { get; set; }
        public int Id { internal set; get; }
        public double IndexSpaceUsage { internal set; get; }
        public bool Initialize() { return false; }
        public bool IsAccessible { internal set; get; }
        public bool IsDbAccessAdmin { internal set; get; }
        public bool IsDbBackupOperator { internal set; get; }
        public bool IsDbDatareader { internal set; get; }
        public bool IsDbDatawriter { internal set; get; }
        public bool IsDbDdlAdmin { internal set; get; }
        public bool IsDbDenyDatareader { internal set; get; }
        public bool IsDbDenyDatawriter { internal set; get; }
        public bool IsDbManager { internal set; get; }
        public bool IsDbOwner { internal set; get; }
        public bool IsDbSecurityAdmin { internal set; get; }
        public bool IsMirroringEnabled { internal set; get; }
        public bool IsMailHost { internal set; get; }
        public bool IsSystemObject { internal set; get; }
        public bool IsUpdateable { internal set; get; }
        public DateTime LastBackupDate { internal set; get; }
        public DateTime LastDifferentialBackupDate { internal set; get; }
        public DateTime LastLogBackupDate { internal set; get; }
        public string Name { internal set; get; }
        public bool NestedTriggersEnabled { get; set; }
        public string Owner { get; set; }
        public string PrimaryFilePath { get; set; }
        public void Refresh()
        {
            if (baseObject != null)
                baseObject.Refresh();
        }
        public void Rename(string newName)
        {
            if (baseObject != null)
                baseObject.Rename(newName);
        }
        public string Script { internal set; get; }
        public void SetOffline()
        {
            if (baseObject != null)
                baseObject.SetOffline();
        }
        public void SetOnline()
        {
            if (baseObject != null)
                baseObject.SetOnline();
        }
        public void SetOwner(string loginName)
        {
            if (baseObject != null)
                baseObject.SetOwner(loginName);
        }
        public void Shrink(int percentFreeSpace, DbShrinkMethod shrinkMethod)
        {
            if (baseObject != null)
                baseObject.Shrink(percentFreeSpace, (x.ShrinkMethod)shrinkMethod);
        }
        public double Size { internal set; get; }
        public double SpaceAvailable { internal set; get; }
        public XState State { internal set; get; }
        public DbStatus Status { internal set; get; }
        public int TargetRecoveryTime { internal set; get; }
        public void TruncateLog()
        {
            if (baseObject != null)
                baseObject.TruncateLog();
        }
        public int Version { internal set; get; }
        public List<DatabaseTable> Tables { get; set; }
        public List<DatabaseView> Views { get; set; }
        public List<DatabaseTrigger> Triggers { get; set; }
        public List<DatabaseStoredProcedure> Procedures { get; set; }
    }
}
