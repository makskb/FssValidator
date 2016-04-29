using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RosstatValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RosstatValidator.TemplateStructure;
using static Validator.Tests.Settings;

namespace Validator.Tests
{ 
    [TestClass]
    public class ControlsTests
    {
        /*static void Main()
        {
            GetNumberStructure();
        }*/
        [TestMethod]
        public void GetNumberStructure()
        {
            var listGood = new List<Section>()
            {
                #region
                new Section()
                {
                    NumberSection = 1,
                    Rows =
                        new List<Row>()
                        {
                            new Row {NumberRow = 0, Cells = new List<Col>() {}},
                            new Row {NumberRow = 1, Cells = new List<Col>() {new Col() {NumberCell = 3}}},
                            new Row
                            {
                                NumberRow = 2,
                                Cells = new List<Col>() {new Col() {NumberCell = 3}, new Col() {NumberCell = 4}, new Col() {NumberCell = 5}, new Col() {NumberCell = 6}, new Col() {NumberCell = 0}, new Col() {NumberCell = 1561616161 } }
                            },
                            new Row {NumberRow = 3, Cells = new List<Col>() {new Col() {NumberCell = 3}}}
                        }
                },
                new Section()
                {
                    NumberSection = 2,
                    Rows =
                        new List<Row>()
                        {
                            new Row {NumberRow = 0, Cells = new List<Col> {}},
                            new Row {NumberRow = 0, Cells = new List<Col> {}},
                            new Row {NumberRow = 30, Cells = new List<Col> {new Col() {NumberCell = 3}, new Col {NumberCell = 4}}},
                            new Row {NumberRow = 31, Cells = new List<Col> {new Col() {NumberCell = 3}, new Col {NumberCell = 4}}}
                        }
                }
                #endregion
            };
            
            var xml = new XDocument();
            xml = XDocument.Load(TemplatePath + "template1.xml");
            var properties = new TemplateStructure();
            var str = properties.ReadStructure(xml);
            #region
            /*foreach (var section in listGood)
            {
                Console.WriteLine("раздел" + section.NumberSection);
                foreach (var row in section.Rows)
                {
                    Console.WriteLine("row" + row.NumberRow);
                    foreach (var cell in row.Cells)
                    {
                        Console.WriteLine("cell" + cell.NumberCell);
                    }
                }
            }

            foreach (var section in str)
            {
                Console.WriteLine("раздел" + section.NumberSection);
                foreach (var row in section.Rows)
                {
                    Console.WriteLine("row" + row.NumberRow);
                    foreach (var cell in row.Cells)
                    {
                        Console.WriteLine("cell" + cell.NumberCell);
                    }
                }
            }
            Console.ReadKey();*/
            #endregion
            Assert.AreEqual(str.Count, listGood.Count);
            var firstRow = str.Where(x => x.NumberSection == 1).Select(z => z.Rows.Select(y=>y.NumberRow).First());
            var firstRow2 = listGood.Where(x => x.NumberSection == 1).Select(z => z.Rows.Select(y=>y.NumberRow).First());
            Assert.AreEqual(firstRow2.First(), firstRow.First());
        }
    }
}
