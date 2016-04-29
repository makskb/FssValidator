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
                    foreach (var section in SectionsList)
                    {
                        Console.WriteLine(section.NumberSection);
                        foreach (var row in section.Rows)
                        {
                            Console.WriteLine("    " + row.NumberRow);
                            foreach (var cell in row.Cells)
                            {
                                Console.WriteLine("        " + cell.NumberCell);
                            }
                        }
                    }
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
