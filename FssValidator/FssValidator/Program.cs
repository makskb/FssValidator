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
    class Program
    {

        static void Main(string[] args)
        {
            var fssTemplates = Directory.GetFiles(Settings.TemplatesDirection);
            foreach (var template in fssTemplates)
            {
                try
                {

                    var xmlTemplate = XDocument.Load(template);
                    ControlsValidator.ParseControls(xmlTemplate);

                    #region

                    //DictionariesValidator.ParseDic(xmlTemplate);
                    //foreach (var dictionary in DictionariesValidator.CheckDictionaries(xmlTemplate))
                    //{
                    //    Console.WriteLine(dictionary);
                    //}

                    #endregion
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
