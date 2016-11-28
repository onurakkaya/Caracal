using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Data
{
    public class ServerLogin
    {
        void ChangePassword(string newPassword) { }
        public DateTime CreateDate { get; internal set; }
        public string Credential { get; internal set; }
        public DateTime DateLastModified { get; internal set; }
        public string DefaultDatabase { get; set; }
        public bool DenyWindowsLogin { get; set; }
        public void Disable() { }
        public bool IsDisabled { get; internal set; }
        public void Enable() { }
        public int Id { get; internal set; }
        public bool IsLocked { get; internal set; }
        public bool IsPasswordExpired { get; internal set; }
        public bool IsSystemObject { get; internal set; }
        public string Language { get; set; }
        public ServerLoginType LoginType { get; set; }
        public bool MustChangePassword { get; set; }
        public string Name { get; set; }
        public bool PaswordExpirationEnabled { get; set; }
        public ServerPasswordHashAlgorithm PasswordHashAlgorithm { get; set; }
        public bool PasswordPolicyEnforced { get; set; }
        public void Refresh() { }
        public void Rename(string newName) { }
        public string Script { get; internal set; }
        public XState State { get; set; }
        public XWindowsLoginAccessType WindowsLoginAccessType { get; set; }
    }
}
