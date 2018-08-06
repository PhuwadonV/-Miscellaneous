using System.IO;
using System.Collections.Generic;

public static class XmlFileWriter {
    public static void Write(string fileName, IXmlChild xmlChilds)
    {
        StreamWriter writer = new StreamWriter(fileName + ".xml", false);
        writer.WriteLine(xmlChilds);
        writer.Close();
    }

    public static void Write(string fileName, ICollection<IXmlChild> xmlChilds)
    {
        StreamWriter writer = new StreamWriter(fileName + ".xml", false);
        foreach(IXmlChild xmlChild in xmlChilds) writer.WriteLine(xmlChild);
        writer.Close();
    }

    public static void Write(string fileName, Delegate<bool, StreamWriter>.Type callback)
    {
        StreamWriter writer = new StreamWriter(fileName + ".xml", false);
        while (callback(writer));
        writer.Close();
    }
}