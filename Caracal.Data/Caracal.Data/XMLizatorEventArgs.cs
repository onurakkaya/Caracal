using System;
using System.Xml.Linq;

namespace Caracal.Data.Xmlizator
{
    public class XMLizatorEventArgs : EventArgs
    {
        public XDocument Document { get;  set; }
        public string Path { get; set; }
    }
}
