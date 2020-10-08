using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XmlToCsv
{
    public sealed class JsonProcess
    {
        public string SourceJasonFile { get; set; }
        public string JsonDocumentPath{get;set;}
        public string DestinationFolderPath { get; set; }
        public string JsonString { get; set; }
        public string LastMessage { get; set; }
        public string XmlDocuemntPath { get; set; }

        public event EventHandler<LogEventArgs> LogEvent;

        public JsonProcess(string destinationFolderPath, string sourceJasonFile)
        {
            this.DestinationFolderPath = destinationFolderPath;
            this.SourceJasonFile = sourceJasonFile;
       
            this.JsonDocumentPath = destinationFolderPath;
            this.XmlDocuemntPath = Path.Combine(this.DestinationFolderPath, "document.xml");

        }

        public bool OpenJasonFile()
        {
            if (!TestJFileExists())
            {
                this.LastMessage = "JSON File does not exist!";
                OnLogEvent(this.LastMessage);
                return false;
            }
            try
            {
                OnLogEvent("Open JSON-File");
                ReadJasonFile();
                return true;
            }
            catch (Exception e)
            {
                this.LastMessage = e.Message;
                OnLogEvent("Error:" + e.Message);
                return false;
            }
        }

        public void WriteXmlToJson()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.XmlDocuemntPath);
            string json = JsonConvert.SerializeXmlNode(doc);
            File.WriteAllText(this.JsonDocumentPath, json);

        }

        public void WriteJasonToXmlAndCsv()
        {
            try
            {
                OnLogEvent("Open Json-File..");
                if (!OpenJasonFile())
                    throw new Exception("Cound not open JSON File=>" + this.SourceJasonFile);
                OnLogEvent("Json-File is OPEN");
                OnLogEvent("Start Json To XML and CSV");
                JsonToXml(this.JsonString,true);

                ConvertXmltoCsv(this.XmlDocuemntPath);

                OnLogEvent("Json To XML and CSV ended");

                OnLogEvent("Files Written");
            }
            catch (Exception e)
            {

                this.LastMessage = e.Message;
                OnLogEvent("Error:" + e.Message);
            }

        }

        private bool TestJFileExists()
        {
            return File.Exists(this.SourceJasonFile);
        }

        private void ReadJasonFile()
        {
            if (!TestJFileExists())
                throw new FileNotFoundException("Jason File dous not exist!=>" + this.SourceJasonFile);
            using (var streamReader = new StreamReader(this.SourceJasonFile, Encoding.UTF8))
            {
                this.JsonString = streamReader.ReadToEnd();
            }
        }




        private void JsonToXml(string text, bool packInRootObj = false)
        {

            var jObject = JObject.Parse(text);

            JContainer cont;
            if (packInRootObj)
                cont = new JObject(new JProperty("RootObject", jObject));
            else
            {
                cont = jObject;
            }


            OnLogEvent("Convert JSON to XML");

            XmlDocument doc = JsonConvert.DeserializeObject<XmlDocument>(cont.ToString());
            OnLogEvent("Convert OK");
            OnLogEvent("save XML:" + this.XmlDocuemntPath);
            doc.Save(this.XmlDocuemntPath);
            OnLogEvent("XML Saved:" + this.XmlDocuemntPath);

        }

        public void ConvertXmltoCsv(string documentPath)
        {
            DataSet ds = new DataSet("JObject");

            OnLogEvent("Convert XML to DataSet...");
            ds.ReadXml(documentPath);
            OnLogEvent("DataSet loaded");
            foreach (DataTable dataTable in ds.Tables)
            {
                string tName = dataTable.TableName;
                string fileName = $"{tName}.csv";
                string filePath = Path.Combine(this.DestinationFolderPath, fileName);
                OnLogEvent("Write CSV:" + filePath);
                using (var file = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    int counter = 0;
                    StringBuilder sb = new StringBuilder();
                    List<ColDescriptor> columns = new List<ColDescriptor>();
                    foreach (DataColumn dataTableColumn in dataTable.Columns)
                    {
                        if (counter > 0)
                            sb.Append(";");
                        sb.Append(dataTableColumn.ColumnName);

                        columns.Add(new ColDescriptor(dataTableColumn.ColumnName, dataTableColumn.DataType));
                        counter += 1;
                    }
                    file.WriteLine(sb.ToString());


                    foreach (DataRow dataTableRow in dataTable.Rows)
                    {
                        sb.Clear();
                        counter = 0;
                        foreach (ColDescriptor columnName in columns)
                        {
                            if (counter > 0)
                                sb.Append(";");
                            object rowValue = dataTableRow[columnName.Name];
                            Type valueType = columnName.ColType;
                            if (valueType == typeof(string))
                            {
                                sb.Append('"');
                                if (rowValue is DBNull)
                                    rowValue = string.Empty;
                                string value = (string)rowValue;
                                value = value.Replace(Environment.NewLine, "");
#if DEBUG
                                if (value.Contains("\""))
                                    Debug.Print(value);

                                if (value.Contains(";"))
                                    Debug.Print(value);
                                if(value.Contains("#"))
                                    Debug.Print(value);
#endif 
                                value = value.Replace("\"", "''");
                                value = value.Replace(";", ",");
                                value = value.Replace("#", "*");
                                //value = value.Replace("\r", "");
                                rowValue = value;
                            }

                            sb.Append(rowValue);
                            if (valueType == typeof(string))
                                sb.Append('"');
                            counter += 1;
                        }
                        file.WriteLine(sb.ToString());
                    }
                }
                OnLogEvent("File written:" + fileName);
            }
        }


        private void OnLogEvent(string message)
        {
            LogEvent?.Invoke(this, new LogEventArgs(message));
        }
    }
}