using System.IO;
using System.Collections.Generic;

public static class XmlFileReader {
    public static ICollection<IXmlChild> Read(string fileName)
    {
        StreamReader reader = new StreamReader(fileName + ".xml");
        string str = reader.ReadToEnd();
        ICollection<IXmlChild> buff = XmlParser.Parse(ref str);
        reader.Close();
        return buff;
    }
}