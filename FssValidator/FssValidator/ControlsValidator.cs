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
using static RosstatValidator.Settings;

namespace RosstatValidator

{
    class ControlsValidator
    {
        public static void ParseControls(XDocument template)
        {
            var controlsList = Template.Root.Element("controls").Elements("control");
            LogEvent.Write("Начинаем валидацию контролей");
            foreach (var control in controlsList)
            {
                LogEvent.Write("Валидируем контроль с id=" + control.Attribute("id").Value);
                var parseCondition = ParseConditionAndRule(control.Attribute("condition").Value);
                if (parseCondition.Count != 0)
                {
                    //получаем номер секции текущего condition
                    var numberSection = parseCondition[0].Select(x => x.Value).FirstOrDefault();
                    //получаем структуру секции с текущим номером и работаем уже только с ней
                    var currentSection = SectionsList.Where(x => x.NumberSection == numberSection).ToList();
                    if (currentSection.Count == 0)
                        LogEvent.Write("ERROR! Номера секции " + numberSection + " не существует");
                    var numberRows = currentSection.Select(x => x.Rows);
                    




                }
            }
        }

        //метод для парсинга значений аттрибутов condition и rule
        protected static List<IEnumerable<int?>> ParseConditionAndRule(string ruleOrCondition)
        {
            var separator = new[] {'[', ']'};
            return ruleOrCondition
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
        }
    }
}
