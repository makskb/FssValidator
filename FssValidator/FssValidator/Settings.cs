using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RosstatValidator.TemplateStructure;

namespace RosstatValidator
{
    public class Settings
    {
        public static XDocument Template = new XDocument();
        public static List<Section> SectionsList = new List<Section>();
        public static string TemplatesDirection = @"../../templates";
        public static LogEvent LogEvent { get; set; }
    }
}
