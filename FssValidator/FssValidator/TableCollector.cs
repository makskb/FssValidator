using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static FssValidator.Settings;

namespace FssValidator
{
    //тут будем собирать свойства каждой таблицы
    public class Section
    {
        public int Number { get; set; }
        public List<string> Rows { get; set; }
        public List<string> Cell { get; set; } 
        /*public static List<int> Table
        {
            get { return Template.Root?.Element("sections")?.Elements("section").Attributes("code").Select(x =>
            {
                int i;
                int.TryParse(x.Value, out i);
                return i;
            }).ToList(); }
        }*/
    }

    internal class GetProperties
    {
        public static List<Section> PropertiesSections()
        {
            var section = new Section();
            var allNumberSections = Template.Root?.Element("sections")?.Elements("section").Attributes("code").Select(x =>
            {
                int i;
                int.TryParse(x.Value, out i);
                return i;
            }).ToList();
            foreach (var numSection in allNumberSections)
            {
                section.Number = numSection;
                /*section.Rows =
                    Template.Root.Elements("sections")
                        .Elements("section")
                        .Where(x =>
                        {
                            int i;
                            var t = x.Attribute("code").Value;
                            int.TryParse(t, out i);
                            if (i == numSection)
                                return true;
                            return false;
                        });*/
            }
        }
    }
} 