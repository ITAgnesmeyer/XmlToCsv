using System;

namespace XmlToCsv
{
    public struct ColDescriptor
    {
        public string Name { get; set; }
        public Type ColType { get; set; }

        public ColDescriptor(string name, Type colType)
        {
            this.Name = name;
            this.ColType = colType;
        }
    }
}