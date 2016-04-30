using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RosstatValidator.Settings;

namespace RosstatValidator
{
    class DictionariesValidator
    {
        private static List<string> Diction
        {
            get
            {
                var dictionaries = Template.Root.Element("dics").Descendants("dic");
                return dictionaries.Where(e => !e.Attributes("parent").Any()).Select(dictionary => dictionary.Attribute("id").Value).ToList();
            }
        }
        private static List<string> DicFilter
        {
            get
            {
                var dictionaries = Template.Root.Element("dics").Descendants("dic");
                return dictionaries.Where(e => e.Attributes("parent").Any()).Select(dictionary => dictionary.Attribute("id").Value).ToList();
            }
        }

        //проверка наличия всех справочников и фильтров
        public static void CheckDictionaries(XDocument template)
        {
            LogEvent.Write("Начинаем проверку справочников");
            var listNeedDic = MakeNeedDictionaries(template);
            var listNeedFilterDic = MakeNeedFilterDictionaries(template);
            foreach (var dic in listNeedDic.Where(dic => !Diction.Contains(dic)))
                LogEvent.Write("Справочник " + dic + " отсутствует в шаблоне");
            foreach (var dic in listNeedFilterDic.Where(dic => !DicFilter.Contains(dic)))
                LogEvent.Write("Фильтр справочника " + dic + " отсутствует в шаблоне");
            LogEvent.Write("Закончили проверки справочников");
        }
        //получание названия всех справоников, которые нужны в разделах
        public static List<string> MakeNeedDictionaries(XDocument template)
        {
            var listNeedDic = new List<string>();
            var needDic = template.Root.Element("sections").Descendants().Attributes("dic");
            foreach (var dic in needDic.Where(dic => !listNeedDic.Contains(dic.Value)))
                listNeedDic.Add(dic.Value);
            return listNeedDic;
        }
        //получание названия всех фильтров справоников, которые нужны в разделах
        public static List<string> MakeNeedFilterDictionaries(XDocument template)
        {
            var listNeedFilter = new List<string>();
            var needFilterDic = template.Root.Elements().Descendants().Where(e => e.Attribute("vldType") != null && e.Attribute("vldType").Value == "4").Attributes("vld");
            foreach (var dic in needFilterDic.Where(dic => !listNeedFilter.Contains(dic.Value)))
                listNeedFilter.Add(dic.Value);
            return listNeedFilter;
        }
    }
}
