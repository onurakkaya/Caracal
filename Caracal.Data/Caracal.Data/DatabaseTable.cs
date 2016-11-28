using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Data
{
    public class DatabaseTable
    {
        public DateTime CreateDate { internal set; get; }
        public DateTime DateLastModified { internal set; get; }
        public bool HasIndex { internal set; get; }
        public bool HasInsertTrigger { internal set; get; }
        public bool HasUpdateTrigger { internal set; get; }
        public bool HasDeleteTrigger { internal set; get; }
        public int Id { internal set; get; }
        public string Name { internal set; get; }
    }
}
