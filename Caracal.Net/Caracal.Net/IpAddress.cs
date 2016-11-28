using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caracal.Common;
namespace Caracal.Net
{
    public class IpAddress
    {
        private string _value { get; set; }
        private int _port { get; set; }

        public static IpAddress Parse(string address, IpAddressType type, int port = 80)
        {
            if (IsValidFormat(address, type)) return new IpAddress() { _value = address, _port = port };
            throw new FormatException(string.Format("{0} is a not valid Ip Address", address));
        }

        public static bool TryParse(string address, IpAddressType type, out IpAddress value, int port = 80)
        {
            if (IsValidFormat(address, type)) { value = new IpAddress() { _value = address, _port = port }; return true; }
            value = new IpAddress();
            return false;
        }
        private static bool IsValidFormat(string address, IpAddressType type)
        {
            //if (!address.IsDigit() || address.Length < 11) return false;

            switch (type)
            {
                case IpAddressType.v4:
                    const char separatorV4 = '.';
                    string[] splitV4 = address.Split(separatorV4);
                    if (splitV4.Length == 4 && splitV4.ToList().TrueForAll(x => x.Length <= 3 && x.IsNumber()))
                        return true;
                    break;
                case IpAddressType.v6:
                    const char separatorV6 = ':';
                    string[] splitV6 = address.Split(separatorV6);
                    List<string> v6List = splitV6.ToList();
                    if (splitV6.Length == 8 && v6List.TrueForAll(x => x.Length.Between(1, 4) && x.IsHexadecimal())) return true;
                    if (v6List.Where(x => x.Length == 0).Count() > 1 && v6List.TrueForAll(x => x.IsHexadecimal())) return true;
                    break;
                default:
                    return false;
            }
            return false;
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", _value, _port);
        }
        public string ToString(string portSepartor)
        {
            return string.Concat(_value, portSepartor, _port);
        }
    }
    public enum IpAddressType
    {
        v4, v6
    }
}
