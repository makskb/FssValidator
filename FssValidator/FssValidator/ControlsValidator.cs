using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FssValidator

{
    class ControlsValidator
    {
        public XElement ControlsList(XDocument template)
        {
            var controls = template.Root.Element("controls");
            return controls;
        }

        public static void ParseControls(XDocument template)
        {
            var controlsList = new ControlsValidator().ControlsList(template).Elements("control");

            foreach (var control in controlsList)
            {
                //Console.WriteLine("Валидируем контроль с id={0}", control.Attribute("id").Value);
                var condition = control.Attribute("condition").Value;
                if (condition != "")
                {
                    var s = ParseConditionAndRule(condition);
                }
                
            }
        }

        protected static List<IEnumerable<int?>> ParseConditionAndRule(string ruleOrCondition)
        {
            //честно скопировал со stackOverflow, но разобрался http://stackoverflow.com/questions/36675434/how-to-correctly-divide-liststring
            char[] separator = new[] {'[', ']'};
            var result = ruleOrCondition
                .Split(separator)
                .Select(x => x.Split(',')) // получаем лист листов(разделителем является ',')
                .Select(x => x.Select(y =>
            {
                int value;
                var isInt = int.TryParse(y, out value);
                if (isInt) return value;
                else return null as int?; 
            }))//выбираем intы
                .Where(x=>x.All(y=>y.HasValue))//выбираем листы с intами
                .ToList();
            return result;
        }



    }
}
