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
using static FssValidator.Settings;

namespace FssValidator
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
                    ControlsValidator.ParseControls(Template);
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
