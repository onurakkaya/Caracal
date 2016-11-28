using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Caracal.Net;
using x = Microsoft.SqlServer.Management.Smo;
namespace Caracal.Data
{
    public class Server
    {
        private x.Server srv;
        public Server(LoginInfo info, IpAddress connectionAddress)
        {
            LoginInfo = info;
            ConnectionAddress = connectionAddress;

            Microsoft.SqlServer.Management.Common.ServerConnection connector = new Microsoft.SqlServer.Management.Common.ServerConnection();
            connector.ConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder { DataSource = connectionAddress.ToString(","), UserID = info.UserName, Password = info.Password.GetDecryptedValue() }.ConnectionString;
            srv = new x.Server(connector);
        }
        public LoginInfo LoginInfo { get; set; }
        public IpAddress ConnectionAddress { get; set; }
        public string ServerName { get; set; }
        public Version ServerVersion { get { return Version.Parse(srv.VersionString); } }
        public string ServerCollation { get { return srv.Collation; } }
        public List<Database> Databases
        {
            get
            {
                return srv.Databases.Cast<x.Database>().Select(y => new Database
                {
                    baseObject = y,
                    ActiveConnectionCount = y.ActiveConnections,
                    AnsiNullDefault = y.AnsiNullDefault,
                    AnsiNullsEnabled = y.AnsiNullsEnabled,
                    AnsiPaddingEnabled = y.AnsiPaddingEnabled,
                    AnsiWarningsEnabled = y.AnsiWarningsEnabled,
                    ArithmeticAbortEnabled = y.ArithmeticAbortEnabled,
                    AutoClose = y.AutoClose,
                    AutoCreateIncrementalStatisticsEnabled = y.AutoCreateIncrementalStatisticsEnabled,
                    AutoCreateStatisticsEnabled = y.AutoCreateStatisticsEnabled,
                    AutoShrink = y.AutoShrink,
                    AutoUpdateStatisticsEnabled = y.AutoUpdateStatisticsEnabled,
                    CaseSensitive = y.CaseSensitive,
                    Collation = y.Collation,
                    CompatibilityLevel = (DbCompatibilityLevel)y.CompatibilityLevel,
                    DatabaseEngineType = (DbEngineType)y.DatabaseEngineType,
                    EncrpytionEnabled = y.EncryptionEnabled,
                    NestedTriggersEnabled = y.NestedTriggersEnabled,
                    Owner = y.Owner,
                    PrimaryFilePath = y.PrimaryFilePath,
                    CreateDate = y.CreateDate,
                    Name = y.Name,
                    Id = y.ID,
                    DatabaseEngineEdition = (DbEngineEdition)y.DatabaseEngineEdition,
                    DatabaseGuid = y.DatabaseGuid,
                    DataSpaceUsage = y.DataSpaceUsage,
                    IndexSpaceUsage = y.IndexSpaceUsage,
                    IsAccessible = y.IsAccessible,
                    IsDbAccessAdmin = y.IsDbAccessAdmin,
                    IsDbBackupOperator = y.IsDbBackupOperator,
                    IsDbDatareader = y.IsDbDatareader,
                    IsDbDatawriter = y.IsDbDatawriter,
                    IsDbDdlAdmin = y.IsDbDdlAdmin,
                    IsDbDenyDatareader = y.IsDbDenyDatareader,
                    IsDbDenyDatawriter = y.IsDbDenyDatawriter,
                    IsDbManager = y.IsDbManager,
                    IsDbOwner = y.IsDbOwner,
                    IsDbSecurityAdmin = y.IsDbSecurityAdmin,
                    IsMailHost = y.IsMailHost,
                    IsMirroringEnabled = y.IsMirroringEnabled,
                    IsSystemObject = y.IsSystemObject,
                    IsUpdateable = y.IsUpdateable,
                    LastBackupDate = y.LastBackupDate,
                    LastDifferentialBackupDate = y.LastDifferentialBackupDate,
                    LastLogBackupDate = y.LastLogBackupDate,
                    Script = string.Join("\n", y.Script()),
                    Size = y.Size,
                    SpaceAvailable = y.SpaceAvailable,
                    State = (XState)y.State,
                    Status = (DbStatus)y.Status,
                    TargetRecoveryTime = y.TargetRecoveryTime,
                    Version = y.Version,
                    Procedures = y.StoredProcedures.Cast<x.StoredProcedure>().Select(z => new DatabaseStoredProcedure
                    {
                        IsEncrypted = z.IsEncrypted,
                        Name = z.Name,
                        TextBody = z.TextBody,
                        TextHeader = z.TextHeader,
                        TextMode = z.TextMode,
                        ClassName = z.ClassName,
                        CreateDate = z.CreateDate,
                        Id = z.ID
                    }).ToList(),
                    Triggers = y.Triggers.Cast<x.Trigger>().Select(r => new DatabaseTrigger
                    {
                        IsEnabled = r.IsEnabled,
                        IsEncrypted = r.IsEncrypted,
                        Name = r.Name,
                        TextBody = r.TextBody,
                        TextHeader = r.TextHeader,
                        TextMode = r.TextMode,
                        Id = r.ID,
                        ClassName = r.ClassName,
                        CreateDate = r.CreateDate
                    }).ToList(),
                    Tables = y.Tables.Cast<x.Table>().Select(p => new DatabaseTable
                    {
                        CreateDate = p.CreateDate,
                        DateLastModified = p.DateLastModified,
                        HasDeleteTrigger = p.HasDeleteTrigger,
                        HasIndex = p.HasIndex,
                        HasInsertTrigger = p.HasInsertTrigger,
                        HasUpdateTrigger = p.HasUpdateTrigger,
                        Id = p.ID,
                        Name = p.Name
                    }).ToList(),
                    Views = y.Views.Cast<x.View>().Select(o => new DatabaseView
                    {
                        CreateDate = o.CreateDate,
                        DateLastModified = o.DateLastModified,
                        HasDeleteTrigger = o.HasDeleteTrigger,
                        HasIndex = o.HasIndex,
                        HasInsertTrigger = o.HasInsertTrigger,
                        HasUpdateTrigger = o.HasUpdateTrigger,
                        Id = o.ID,
                        Name = o.Name
                    }).ToList()
                }).ToList();
            }
        }
        public ServerLoginMode LoginMode { get { return (ServerLoginMode)srv.LoginMode; } set { srv.LoginMode = (x.ServerLoginMode)value; } }
        public List<ServerLogin> Logins
        {
            get
            {
                return srv.Logins.Cast<x.Login>().Select(y => new ServerLogin
                {
                    CreateDate = y.CreateDate,
                    Credential = y.Credential,
                    DateLastModified = y.DateLastModified,
                    DefaultDatabase = y.DefaultDatabase,
                    DenyWindowsLogin = y.DenyWindowsLogin,
                    Id = y.ID,
                    IsDisabled = y.IsDisabled,
                    IsLocked = y.IsLocked,
                    IsPasswordExpired = y.IsPasswordExpired,
                    IsSystemObject = y.IsSystemObject,
                    Language = y.Language,
                    LoginType = (ServerLoginType)y.LoginType,
                    MustChangePassword = y.MustChangePassword,
                    Name = y.Name,
                    PasswordHashAlgorithm = (ServerPasswordHashAlgorithm)y.PasswordHashAlgorithm,
                    PasswordPolicyEnforced = y.PasswordPolicyEnforced,
                    PaswordExpirationEnabled = y.PasswordExpirationEnabled,
                    Script = string.Join("\n", y.Script()),
                    State = (XState)y.State,
                    WindowsLoginAccessType = (XWindowsLoginAccessType)y.WindowsLoginAccessType
                }).ToList();
            }
        }
        public XStatus Status { get { return (XStatus)srv.Status; } }
        public bool TcpEnabled { get; }
        public bool BackupEnabled
        {
            get
            {
                return srv.SmartAdmin.BackupEnabled;
            }
            set
            {
                srv.SmartAdmin.BackupEnabled = value;
            }
        }
        public int BackupRetentionPeriodInDays
        {
            get
            {
                return srv.SmartAdmin.BackupRetentionPeriodInDays;
            }
            set
            {
                srv.SmartAdmin.BackupRetentionPeriodInDays = value;
            }
        }
        public XServiceStartMode ServiceStartMode
        {
            get { return (XServiceStartMode)srv.ServiceStartMode; }
        }
        public string ServiceName { get { return srv.ServiceName; } }
        public string ServiceInstanceId { get { return srv.ServiceInstanceId; } }
        public string ServiceAccount { get { return srv.ServiceAccount; } }
        public DbEngineType ServerType { get { return (DbEngineType)srv.ServerType; } }
        public string RootDirectory { get { return srv.RootDirectory; } }
        public string ProductLevel { get { return srv.ProductLevel; } }
        public string Product { get { return srv.Product; } }
        public int ProcessorUsagePercent { get { return srv.ProcessorUsage; } }
        public int ProcessorCount { get { return srv.Processors; } }
        public string Platform { get { return srv.Platform; } }
        public long PhysicalMemoryUsageInKB { get { return srv.PhysicalMemoryUsageInKB; } }
        public int PhysicalMemory { get { return srv.PhysicalMemory; } }
        public string OSVersion { get { return srv.OSVersion; } }
        public string NetName { get { return srv.NetName; } }
        public bool NamedPipesEnabled { get { return srv.NamedPipesEnabled; } }
        public string MasterDbPath { get { return srv.MasterDBPath; } }
        public string MasterDbLogPath { get { return srv.MasterDBLogPath; } }
        public string Language { get { return srv.Language; } }
        public bool IsSingleUser { get { return srv.IsSingleUser; } }
        public string InstanceName { get { return srv.InstanceName; } }
        public string InstallDataDirectory { get { return srv.InstallDataDirectory; } }
        public string InstallSharedDirectory { get { return srv.InstallSharedDirectory; } }
        public ServerEdition EngineEdition { get { return (ServerEdition)srv.EngineEdition; } }
        public string ComputerNamePhysicalNetBIOS { get { return srv.ComputerNamePhysicalNetBIOS; } }
        public string Edition { get { return srv.Edition; } }
        public int CollationId { get { return srv.CollationID; } }
        public string Collation { get { return srv.Collation; } }
        public int BulidNumber { get { return srv.BuildNumber; } }
        public string BulidClrVersionString { get { return srv.BuildClrVersionString; } }
        public XServiceStartMode BrowserStartMode { get { return (XServiceStartMode)srv.BrowserStartMode; } }
        public string BrowserServiceAccount { get { return srv.BrowserServiceAccount; } }
        public string BackupDirectory { get { return srv.BackupDirectory; } }
        public void KillProcess(int processId)
        {
            srv.KillProcess(processId);
        }
        public void KillDatabase(string databaseName)
        {
            srv.KillDatabase(databaseName);
        }
        public void KillAllProcesses(string databaseName)
        {
            srv.KillAllProcesses(databaseName);
        }
        public int GetActiveDbConnectionCount(string databaseName)
        {
            return srv.GetActiveDBConnectionCount(databaseName);
        }
        public void Refresh()
        { srv.Refresh(); }
    }


}
