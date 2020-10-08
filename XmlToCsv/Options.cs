using CommandLine;

namespace XmlToCsv
{
    internal class Options
    {

        [Option('i', "input", Required = true, HelpText = "Input XML file to read.")]
        public string InputXmlFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output Folder for destination Files")]
        public string OutputDirectorey { get; set; }

        [Option('j', "json", Required = false, HelpText = "Input Json File to read")]
        public string JsonFile { get; set; } = "";

        //public string GetUsage()
        //{
        //    // this without using CommandLine.Text
        //    // this without using CommandLine.Text
        //    var usage = new StringBuilder();


        //    return usage.ToString();
        //}


    }
}