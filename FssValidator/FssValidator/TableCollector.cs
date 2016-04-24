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
            public List<Cell> Cells { get; set; }
        }

        public class Cell
        {
            public int NumberCell { get; set; }
        }

        public List<Section> ReadStructure(XDocument xml)
        {
            List<Section> sections = new List<Section>();
            //просто получили все номера секции
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
                    var row = new Row {NumberRow = i};
                    listRow.Add(row);
                }
                //добавляем list<row> в коллекцию
                section.Rows = listRow;
            }

            foreach (var section in sections)
            {
                foreach (var row in section.Rows)
                {
                    var listCells = new List<Cell>();
                    var currentCells = xml.Root.Elements("sections").Elements("section")
                        .Where(x =>
                        {
                            int i;
                            int.TryParse(x.Attribute("code").Value, out i);
                            return i == section.NumberSection;
                        }).Elements("rows").Elements("row").Where(z =>
                        {
                            int i;
                            int.TryParse(z.Attribute("code").Value, out i);
                            return i == row.NumberRow;
                        }).Elements("cell").Select(y =>
                        {
                            int i = 0;
                            int.TryParse(y.Attribute("column").Value, out i);
                            return i;
                        }).ToList();
                    foreach (var currentCell in currentCells)
                    {
                        var cell = new Cell();
                        cell.NumberCell = currentCell;
                        listCells.Add(cell);
                    }
                    row.Cells = listCells;
                }
            }
            return sections;
        }
    }
}
    