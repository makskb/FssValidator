using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RosstatValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static RosstatValidator.Settings;
using static RosstatValidator.TemplateStructure;
using static Validator.Tests.Settings;

namespace Validator.Tests
{ 
    [TestClass]
    public class ControlsTests
    {
        private static readonly XDocument template = XDocument.Load(TemplatePath + @"..\..\..\resources\template1.xml");
        private static readonly List<Section> StructureSections = new TemplateStructure().ReadStructure(template);

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
                            new Row {NumberRow = 1, Cells = new List<Col>() {new Col() {NumberCell = 3}}},
                            new Row
                            {
                                NumberRow = 2,
                                Cells = new List<Col>() {new Col() {NumberCell = 3}, new Col() {NumberCell = 4} }
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
                            new Row {NumberRow = 30, Cells = new List<Col> {new Col() {NumberCell = 3}, new Col {NumberCell = 4}}},
                            new Row {NumberRow = 31, Cells = new List<Col> {new Col() {NumberCell = 3}, new Col {NumberCell = 4}}}
                        }
                }
                #endregion
            };
            //ожидаю увидеть section.count == 2
            Assert.AreEqual(StructureSections.Count, listGood.Count);
            var firstRow = StructureSections.Where(x => x.NumberSection == 2).Select(z => z.Rows.Select(y=>y.NumberRow).First());
            var firstRow2 = listGood.Where(x => x.NumberSection == 2).Select(z => z.Rows.Select(y=>y.NumberRow).First());
            //ожидаю получить во второй секции первый row с номером 30
            Assert.AreEqual(firstRow2.First(), firstRow.First());
        }
        [TestMethod]
        public void CheckDash()
        {
            var str = "1-6,45,2";
            var result = ControlsValidator.Dash(str);
            var assertString = "1,2,3,4,5,6,45,2";
            Assert.AreEqual(assertString, result);
        }
        [TestMethod]
        public void CheckStarHandler()
        {
            var str = "[1][15][6][4]";
            var result = ControlsValidator.StarHandler(str);
            var assertString = "[1][15][6]";
            Assert.AreEqual(assertString, result);
        }
        [TestMethod]
        public void CheckStarHandlerWithStar()
        {
            SectionsList = StructureSections;
            var str = "[1][*][3][4]";
            var result = ControlsValidator.StarHandler(str);
            var assertString = "[1][1,2,3][3]";
            Assert.AreEqual(assertString, result);
        }
    }
}
