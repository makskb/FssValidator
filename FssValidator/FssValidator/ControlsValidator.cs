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
                var currentRule = control.Attribute("condition").Value;
                if (currentRule.Trim() != "")
                {
                    var conditionCollector = currentRule.Split(new char[] { '{', '}' });
                    foreach (var s in conditionCollector)
                    {
                        try
                        {
                            Comparator(ParseConditionAndRule(s));
                        }
                        catch (Exception)
                        {
                            //LogEvent.Write("Попытка обработки строки '" + s + "' закончилась неудачей");
                            continue;
                        }
                    }
                }
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
            var s =  ruleOrCondition.Split(separator).Select(x =>
            {
                if (x.Contains('-'))
                    return Dash(x); //дописываем перечисления, если строка содержит -
                return x;
            }).Select(x => x.Split(',')) // получаем лист листов(разделителем является ',')
                .Select(x => x.Select(y =>
                {
                    int value;
                    var isInt = int.TryParse(y, out value);
                    if (isInt) return value;
                    else return null as int?;
                }))//выбираем intы
                .Where(x => x.All(y => y.HasValue))//выбираем листы с intами
                .ToList();
            return s;
        }

        //метод получает на вход строку типа 15,20-23,65 и выдает 15,20,21,22,23,65
        public static string Dash(string str)
        {
            var listAll = str.Split(',').ToList();
            var listWithDash = listAll.Where(x => x.Contains('-')).ToList();//отделяем строки, содержащие -
            var listWhithoutDash = listAll.Where(x=>!x.Contains('-')).ToList().Select(x=>x + ",").ToList(); //строки, не содержащие -
            var listResult = new List<string>();
            //каждую строку с '-' обрабатываем, и записываем в итоговый лист
            foreach (var list in listWithDash)
            {
                var s = list.Split('-');
                int valLeft;
                int.TryParse(s[0], out valLeft);
                int valRigth;
                int.TryParse(s[1], out valRigth);
                string result = null;
                for (int i = valLeft; i < valRigth + 1; i++)
                    result += i + ",";
                listResult.Add(result);
            }
            //объединяем все значения
            listResult.AddRange(listWhithoutDash);
            //записываем все значения в строку
            string resultStr = listResult.Aggregate<string, string>(null, (current, s) => current + s);
            return resultStr.Substring(0, resultStr.Length - 1); // удаляем последнюю ',' и возвращяем значение
        }
    }
}
