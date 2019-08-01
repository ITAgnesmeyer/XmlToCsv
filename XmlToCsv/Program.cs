using System;
using CommandLine;
using JsonToXMLAndCSV;

namespace XmlToCsv
{
    class Program
    {
        public static int Main(string[] args)
        {
            Options options = new Options();
            if (!GetOptions(args, options))
                return 10;

            JsonProcess jProcessor = new JsonProcess(options.OutputDirectorey, options.JsonFile, options.JsonFile)
            {
                XmlDocuemntPath = options.InputXmlFile
            };
            jProcessor.LogEvent += OnLogEvnet;
            try
            {
                if (string.IsNullOrEmpty(options.JsonFile))
                {
                    jProcessor.ConvertXmltoCsv(options.InputXmlFile);
                }
                else
                {
                    jProcessor.WriteJasonToXmlAndCsv();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 10;
            }
            return 0;
        }

        private static void OnLogEvnet(object sender, LogEventArgs e)
        {
            Console.WriteLine(e.LogMessage);
        }

        private static bool GetOptions(string[] args,Options options)
        {
            bool parsResult = false;
            var parser = new Parser(config => config.HelpWriter = Console.Out);

            var result = parser.ParseArguments<Options>(args)
                .WithParsed(x =>
                {
                    options.InputXmlFile  = x.InputXmlFile ;
                    options.OutputDirectorey = x.OutputDirectorey;
                    options.JsonFile = x.JsonFile;
                    parsResult = true;
                })
                .WithNotParsed(x =>
                {
                    parsResult = false;
                });
            return parsResult;
            // consume Options type properties
        }
    }
}
