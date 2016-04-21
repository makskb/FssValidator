using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static FssValidator.Settings;

namespace FssValidator
{
    //тут будем собирать свойства каждой таблицы
    public class TableCollector
    {
        public static List<int> Table
        {
            get { return Template.Root?.Element("sections")?.Elements("section").Attributes("code").Select(x =>
            {
                int i;
                int.TryParse(x.Value, out i);
                return i;
            }).ToList(); }
        }
    }
}
