using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FssValidator
{
    class Settings
    {
        public static XDocument Template = new XDocument();
        public static string TemplatesDirection = @"../../templates";
    }
}
