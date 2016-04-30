using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RosstatValidator.Settings;

namespace RosstatValidator
{
    public class TemplateStructure
    {
        public class Section
        {
            public int NumberSection { get; set; }
            public List<Row> Rows { get; set; }
        }

        public class Row
        {
            public int NumberRow { get; set; }
            public List<Col> Cells { get; set; }
        }

        public class Col
        {
            public int NumberCell { get; set; }
        }

        public List<Section> ReadStructure(XDocument xml)
        {
            LogEvent.Write("Начинаем читать структуру нумерации разделов шаблона");
            List<Section> sections = new List<Section>();
            var rootSection = xml.Root.Element("sections").Elements("section");
            //получаем все номера секции
            var allNumberSections = rootSection.Attributes("code").Select(x => x.Value.ToInt()).ToList();
            foreach (var numSection in allNumberSections)
            {
                //записываем номер секции
                var section = new Section {NumberSection = numSection};
                LogEvent.Write("Успешно записали номера sections");
                //добавляем в коллекцию
                sections.Add(section);
                //получаем текущую секцию
                var currentSection = rootSection.Where(x => x.Attribute("code").Value.ToInt() == section.NumberSection);
                //получаем список row в конкретной секции
                var currentRow = currentSection
                    .Elements("rows").Elements("row").Select(y => y.Attribute("code").Value.ToInt())
                    .ToList();
                //на каждую row создаем объект, записываем номер и добавляем в list
                var listRow = (from i in currentRow where i != 0 select new Row { NumberRow = i }).ToList();
                //добавляем в коллекцию
                section.Rows = listRow;
                LogEvent.Write("Успешно записали номера rows");
                //список значений column в default-cell, у которых inputType != 0
                var currentDefaultCell = currentSection
                        .Elements("columns")
                        .Elements("column")
                        .Elements("default-cell")
                        .Where(x => x.Attribute("inputType").Value.ToInt() != 0)
                        .Select(x => x.Attribute("column").Value)
                        .ToList();
                foreach (var row in section.Rows)
                {
                    var listCells = new List<Col>();
                    //список значений column в cell, которые запрещены для вода в текущем row
                    var currentForbiddenCells = currentSection
                            .Elements("rows")
                            .Elements("row")
                            .Where(z => z.Attribute("code").Value.ToInt() == row.NumberRow)
                            .Elements("cell")
                            .Where(b => b.Attribute("inputType").Value.ToInt() == 0)
                            .Select(y => y.Attribute("column").Value)
                            .ToList();
                    //добавляем в row все номера currentDefaultCell, кроме тех, которые есть в currentForbiddenCells
                    foreach (var str in currentDefaultCell)
                    {
                        var currentCell = new Col();
                        if (!currentForbiddenCells.Contains(str) && str.ToInt() != 0)
                        {
                            currentCell.NumberCell = str.ToInt();
                            listCells.Add(currentCell);
                        }
                    }
                    row.Cells = listCells;
                }
                LogEvent.Write("Успешно записали номера cells");
            }
            LogEvent.Write("Закончили получение структуры нумераций");
            return sections;
        }
    }

    public static class MyExtentions
    {
        public static int ToInt(this string str)
        {
            int i = 0;
            int.TryParse(str, out i);
            return i;
        }

        //метод возвращает индекс элемента, содержащего *
        public static int[] IndexOfContains(this string[] massStr, char x)
        {
            var result = new List<int>();
            for (var i = 0; i < massStr.Length; i++)
            {
                if (massStr[i].Contains(x)) result.Add(i);
            }
            return result.ToArray();
        }
    }
}
    