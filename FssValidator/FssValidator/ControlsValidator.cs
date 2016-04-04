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
                    Console.WriteLine(ParseConditionAndRule(condition));
                }
            }
        }

        protected static List<int[]> ParseConditionAndRule(string condition)
        {
            char[] separator = new[] {'[', ']'};
            string[] parseString = condition.Split(separator);
            List<int[]> numberSections = new List<int[]>();

            foreach (var str in parseString)
            {
                int i;
                string sectionsList;
                string rowList;
                string cellList;
                if (int.TryParse(str.Substring(0, 1), out i));
                {
                    sectionsList = str;
                }

            }
            return numberSections;
        }



    }
}
