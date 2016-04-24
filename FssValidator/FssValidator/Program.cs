using System;
using System.Linq;
using System.Xml.Linq;

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
                    Settings.Template = XDocument.Load(arg.First());
                    LogEvent.Write("успешно загрузили файл " + arg.First());
                    //инициализируем список номеров разделов
                    var str = new TemplateStructure();
                    Settings.SectionsList = str.ReadStructure(Settings.Template);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Произошло непредвиденное исключение");
                throw;
            }
        }
    }
}
