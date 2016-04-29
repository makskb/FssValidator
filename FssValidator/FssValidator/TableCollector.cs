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
            //получили все номера секции
            var allNumberSections =
                xml.Root.Element("sections").Elements("section").Attributes("code").Select(x =>
                {
                    int i;
                    int.TryParse(x.Value, out i);
                    return i;
                }).ToList();
            foreach (var numSection in allNumberSections)
            {
                var section = new Section();
                //записываем номер секции
                section.NumberSection = numSection;
                //добавляем в коллекцию
                sections.Add(section);
            }
            LogEvent.Write("Успешно записали номера sections");
            //для каждой секции записываем список row
            foreach (var section in sections)
            {
                var listRow = new List<Row>();
                //получаем список row в конкретной секции
                var currentRow = xml.Root.Elements("sections").Elements("section")
                    .Where(x =>
                    {
                        int i;
                        int.TryParse(x.Attribute("code").Value, out i);
                        return i == section.NumberSection;
                    }).Elements("rows").Elements("row").Select(y =>
                    {
                        int i;
                        int.TryParse(y.Attribute("code").Value, out i);
                        return i;
                    }).ToList();
                //на каждую row создаем объект, записываем номер и добавляем в list
                foreach (var i in currentRow)
                {
                    if (i != 0)
                    {
                        var row = new Row { NumberRow = i };
                        listRow.Add(row);
                    }
                    
                }
                //добавляем list<row> в коллекцию
                section.Rows = listRow;
            }
            LogEvent.Write("Успешно записали номера rows");
            foreach (var section in sections)
            {
                var currentDefaultCell = xml.Root.Elements("sections").Elements("section")
                    .Where(x => x.Attribute("code").Value.ToInt() == section.NumberSection)
                    .Elements("columns")
                    .Elements("column")
                    .Elements("default-cell")
                    .Where(x=>x.Attribute("inputType").Value.ToInt() != 0).Select(x=>x.Attribute("column").Value)
                    .ToList();
                foreach (var row in section.Rows)
                {
                    var listCells = new List<Col>();
                    var currentForbiddenCells = xml.Root.Elements("sections").Elements("section")
                        .Where(x => x.Attribute("code").Value.ToInt() == section.NumberSection)
                        .Elements("rows").Elements("row").Where(z => z.Attribute("code").Value.ToInt() == row.NumberRow)
                        .Elements("cell").Where(b=>b.Attribute("inputType").Value.ToInt() == 0).Select(y => y.Attribute("column").Value)
                        .ToList();
                    
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
            }
            LogEvent.Write("Успешно записали номера cells");
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
    }
}
    