using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Linq;
using FssValidator;

namespace FssValidator
{
    public class FssTemplate
    {
        public XElement Title { get; set; }
        public XElement Sections { get; set; }
        public XElement Controls { get; set; }
        public XElement Dics { get; set; }
    }


    class Program
    {
        public static void ParseTemplate(XDocument xml)
        {
            var fssTemplate = new FssTemplate();
            fssTemplate.Title = xml.Root.Element("title");
            fssTemplate.Sections = xml.Root.Element("sections");
            fssTemplate.Controls = xml.Root.Element("controls");
            fssTemplate.Dics = xml.Root.Element("dics");



        }



        static void Main(string[] args)
        {
            var fssTemplates = Directory.GetFiles(Settings.TemplatesDirection);
            foreach (var template in fssTemplates)
            {
                try
                {
                    var xmlTemplate = XDocument.Load(template);
                    ParseTemplate(xmlTemplate);
                }
                catch (Exception)
                {
                    Console.WriteLine("Произошло непредвиденное исключение в файле {0}", template);
                    throw;
                }
            }

        }
    }
}
