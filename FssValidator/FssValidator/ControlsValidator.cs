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
                var parseRule = ParseConditionAndRule(control.Attribute("rule").Value);
                //валидируем condition
                if (parseCondition.Count != 0)
                    Comparator(parseCondition);
                //валидируем rule
                if (parseRule.Count != 0)
                    Comparator(parseRule);
            }
        }

        //метод сравнивает список перечислений int с sections, rows, cells
        public static void Comparator(List<IEnumerable<int?>> parseCondition)
        {
            //получаем номер секции текущего condition
            var numberSectioninControl = parseCondition[0].Select(x => x.Value).FirstOrDefault();
            //получаем структуру секции с текущим номером и работаем уже только с ней
            var currentSection = SectionsList.Where(x => x.NumberSection == numberSectioninControl).FirstOrDefault();
            if (currentSection == null)
            {
                LogEvent.Write("ERROR! Номера секции " + numberSectioninControl + " не существует");
            }
            else // если секция существует, то продолжаем проверять row в ней
            {
                var numberRowsInControl = parseCondition[1].Select(x => x.Value).ToList();
                foreach (var row in numberRowsInControl)
                {
                    var currentRow = currentSection.Rows.Where(x => x.NumberRow == row).FirstOrDefault();
                    if (currentRow == null)
                    {
                        LogEvent.Write("ERROR! Номера row " + row + " не существует в секции №" + currentSection.NumberSection);
                    } // если row существует, то проверяем cells в ней
                    else
                    {
                        var numberCellsInControl = parseCondition[2].Select(x => x.Value).ToList();
                        foreach (var cell in numberCellsInControl.Where(cell => !currentRow.Cells.Select(x => x.NumberCell).ToList().Contains(cell)))
                        {
                            LogEvent.Write("ERROR! Номера cell " + cell + " не существует в row №" + currentRow.NumberRow + " в секции №" + currentSection.NumberSection);
                        }
                    }
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
