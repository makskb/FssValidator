using System;
using System.Linq;
using System.Xml.Linq;
using static RosstatValidator.Settings;

namespace RosstatValidator
{
    class Program
    {
        static void Main(string[] arg)
        {
            try
            {
                if (arg.First() != "")
                {
                    Template = XDocument.Load(arg.First());
                    LogEvent.Write("успешно загрузили файл " + arg.First());
                    var str = new TemplateStructure();
                    SectionsList = str.ReadStructure(Template);
                    ControlsValidator.ParseControls(Template);
                    DictionariesValidator.CheckDictionaries(Template);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Произошло непредвиденное исключение");
                throw;
            }
            finally
            {
                LogEvent.Write("Well done");
            }
        }
    }
}
