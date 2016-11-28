using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Data
{
    public class DatabaseTrigger
    {
        public string ClassName { internal set; get; }
        public DateTime CreateDate { internal set; get; }
        public int Id { internal set; get; }
        public bool IsEnabled { get; set; }
        public bool IsEncrypted { get; set; }
        public string Name { get; set; }
        public string TextBody { get; set; }
        public string TextHeader { get; set; }
        public bool TextMode { get; set; }
    }
}
