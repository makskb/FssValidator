using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

using FssValidator;

namespace FssValidator
{
    public class FssTemplate 
    {
        public static List<string> Diction = new List<string>(); //список фильтров справочников
        public static List<string> DicFilter = new List<string>(); //список фильтров справочников
        
        //метод для получения списки имен справочников
        public static void ParseDic(XDocument xml)
        {
            var Dictionaries = xml.Root.Element("dics").Descendants("dic");
            
            foreach (var dictionary in Dictionaries.Where(e => e.Attributes("parent").Any()))
            {
                DicFilter.Add(dictionary.Attribute("id").Value);
            }

            foreach (var dictionary in Dictionaries.Where(e => !e.Attributes("parent").Any()))
            {
                Diction.Add(dictionary.Attribute("id").Value);
            }

            foreach (var makeNeedDictionary in MakeNeedDictionaries(Diction, DicFilter, xml))
            {
                Console.WriteLine(makeNeedDictionary);
            }
        }


        //проверка наличия всех справочников и фильтров
        public static List<string> CheckDictionaries(Func<List<string>, List<string>, XDocument, List<string>> MakeNeedDictionaries)
        {
            
            return 
        } 


        //получание названия всех справоников, которые нужны в разделах
        public static List<string> MakeNeedDictionaries(List<string> Dictionaries, List<string> Dicfilter, XDocument template)
        {
            var listNeedDic = new List<string>();
            var needDic = template.Root.Elements().Descendants().Attributes("dic");
            foreach (var dic in needDic)
            {
                if (!listNeedDic.Contains(dic.Value))
                {
                    listNeedDic.Add(dic.Value);
                }
            }
            return listNeedDic;
            #region
            /*var needDic = template.Root.Elements().Descendants().Attributes("dic");
            foreach (var attribute in needDic)
            {
                if (!Diction.Contains(attribute.Value))
                    yield return "Справочника " + attribute.Value + " нет в шаблоне";
                else if (Diction.Contains(attribute.Value))
                {
                    yield return "Справочник " + attribute.Value + " присутствует в шаблоне";
                }
            }

            var needFilterDic = template.Root.Elements().Descendants().Where(e => e.Attribute("vldType") != null && e.Attribute("vldType").Value == "4").Attributes("vld");
            foreach (var attribute in needFilterDic)
            {
                if (!DicFilter.Contains(attribute.Value))
                    yield return "Фильтра справочника " + attribute.Value + " нет в шаблоне";
                else if (DicFilter.Contains(attribute.Value))
                {
                    yield return "Фильтр справочника " + attribute.Value + " присутствует в шаблоне";
                }
            }*/
            #endregion
        }
        //получание названия всех фильтров справоников, которые нужны в разделах
        public static List<string> MakeNeedFilterDictionaries(List<string> Dictionaries, List<string> Dicfilter, XDocument template)
        {
            var listNeedFilter = new List<string>();
            var needFilterDic = template.Root.Elements().Descendants().Where(e => e.Attribute("vldType") != null && e.Attribute("vldType").Value == "4").Attributes("vld");
            foreach (var dic in needFilterDic)
            {
                if (!listNeedFilter.Contains(dic.Value))
                {
                    listNeedFilter.Add(dic.Value);
                }
            }
            return listNeedFilter;
        }
    }


    class Program
    {
        public static void ParseTemplate(XDocument xml)
        {
            #region
            /*var fssTemplate = new FssTemplate();
            fssTemplate.Title = xml.Root.Element("title");
            fssTemplate.Sections = xml.Root.Element("sections");
            fssTemplate.Controls = xml.Root.Element("controls");
            fssTemplate.Dics = xml.Root.Element("dics");*/
            #endregion

            FssTemplate.ParseDic(xml);
        }


        

        /*public static void ParseDic(XDocument xml)
        {
            var fssTemplate = new FssTemplate();
            fssTemplate.Title = xml.Root.Element("title");
            fssTemplate.Sections = xml.Root.Element("sections");
            fssTemplate.Controls = xml.Root.Element("controls");
            fssTemplate.Dics = xml.Root.Element("dics");

            var requiredDic = fssTemplate.Sections.Descendants().Attributes("dic");
            var containsDic = fssTemplate.Dics.Descendants("dic").Attributes("id");
            foreach (var dic in requiredDic)
            {
                Console.WriteLine(dic);
                foreach (var dicc in containsDic)
                {
                    if (dic.Value == dicc.Value)
                    {
                        Console.WriteLine("Справочник {0} содержится в шаблоне", dic.Value);
                    }
                }
                
            }
        }*/



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
