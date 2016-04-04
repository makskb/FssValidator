using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FssValidator
{
    class DictionariesValidator
    {
        static readonly List<string> Diction = new List<string>(); //список справочников
        static readonly List<string> DicFilter = new List<string>(); //список фильтров справочников

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
        }

        //проверка наличия всех справочников и фильтров
        public static List<string> CheckDictionaries(XDocument template)
        {
            var name = template.Root.Attribute("name").Value;
            var list = new List<string>();
            var listNeedDic = MakeNeedDictionaries(template);
            var listNeedFDic = MakeNeedFilterDictionaries(template);
            foreach (var dic in listNeedDic)
            {
                if (Diction.Contains(dic))
                {
                    list.Add("Справочник " + dic + " присутствует в шаблоне " + name);
                }
                if (!Diction.Contains(dic))
                {
                    list.Add("Справочник " + dic + " отсутствует в шаблоне " + name);
                }
            }
            foreach (var dic in listNeedFDic)
            {
                if (DicFilter.Contains(dic))
                {
                    list.Add("Фильтр справочника " + dic + " присутствует в шаблоне " + name);
                }
                if (!DicFilter.Contains(dic))
                {
                    list.Add("Фильтр справочника " + dic + " отсутствует в шаблоне " + name);
                }
            }
            return list;
        }

        //получание названия всех справоников, которые нужны в разделах
        public static List<string> MakeNeedDictionaries(XDocument template)
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
        }
        //получание названия всех фильтров справоников, которые нужны в разделах
        public static List<string> MakeNeedFilterDictionaries(XDocument template)
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
}
