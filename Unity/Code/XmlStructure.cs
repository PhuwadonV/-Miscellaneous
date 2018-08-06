using System.Collections.Generic;
using System.Text;

public interface ICloneable<out T>
{
    T Clone();
}

public interface IStringBuilderIndentLevel
{
    StringBuilder GetStringBuilderIndentLevel(uint level, bool isFollowingContent = false);
}

public enum XmlChildType
{
    Element,
    Content
}

public interface IXmlChild : ICloneable<IXmlChild>, IStringBuilderIndentLevel
{
    XmlChildType ChildType { get; }
    IXmlElement AsIXmlElement();
    IXmlContent AsIXmlContent();
}

public interface IXmlContent : ICloneable<IXmlContent>, IXmlChild
{
    string Text { get; set; }
}

public interface IXmlElement : ICloneable<IXmlElement>, IXmlChild
{
    string Name { get; set; }
    ICollection<IXmlAttribute> Attributes { get; set; }
    ICollection<IXmlChild> Childs { get; set; }
    IXmlElement AddAttribute(IXmlAttribute xmlAttribute);
    IXmlElement AddAttribute(string name, string value);
    IXmlElement AddChild(IXmlChild xmlChild);
    IXmlElement AddChild(string text);
}

public interface IXmlAttribute : ICloneable<IXmlAttribute>
{
    string Name { get; set; }
    string Value { get; set; }
}

public static class IXmlChildExt
{
    public static T XmlChildPath<T>(this IXmlChild xmlChild, Delegate<T, IXmlContent>.Type OnIXmlContent, Delegate<T, IXmlElement>.Type OnIXmlElement)
    {
        switch (xmlChild.ChildType)
        {
            case XmlChildType.Content:
                return OnIXmlContent(xmlChild.AsIXmlContent());
            case XmlChildType.Element:
                return OnIXmlElement(xmlChild.AsIXmlElement());
            default:
                return default(T);
        }
    }
}

public static class IXmlElementExt
{
    public static IXmlContent GetXmlContentIfExist(this IXmlElement xmlElement)
    {
        IEnumerator<IXmlChild> enumerator = xmlElement.Childs.GetEnumerator();
        if (!enumerator.MoveNext()) return null;
        IXmlChild xmlChild = enumerator.Current;
        if (xmlChild.ChildType != XmlChildType.Content) return null;
        return xmlChild.AsIXmlContent();
    }

    public static IDictionary<string, string> GetAttributeDictionary(this IXmlElement xmlElement)
    {
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach(IXmlAttribute attribute in xmlElement.Attributes) dictionary.Add(attribute.Name, attribute.Value);
        return dictionary;
    }
}

// Implement : IXmlContent
public partial class XmlContent : IXmlContent
{
    private string _text;

    public XmlContent(string text)
    {
        _text = text;
    }

    string IXmlContent.Text
    {
        get { return _text; }
        set { _text = value; }
    }

    public override string ToString()
    {
        return _text;
    }
}

// Implement : ICloneable<IXmlChild>
partial class XmlContent : ICloneable<IXmlChild>
{
    IXmlChild ICloneable<IXmlChild>.Clone()
    {
        return (this as ICloneable<IXmlContent>).Clone();
    }
}

// Implement : ICloneable<IXmlContent>
partial class XmlContent : ICloneable<IXmlContent>
{
    IXmlContent ICloneable<IXmlContent>.Clone()
    {
        return new XmlContent(_text);
    }
}

// Implement : IXmlChild
partial class XmlContent : IXmlChild
{
    XmlChildType IXmlChild.ChildType
    {
        get { return XmlChildType.Content; }
    }

    IXmlElement IXmlChild.AsIXmlElement()
    {
        return null;
    }

    IXmlContent IXmlChild.AsIXmlContent()
    {
        return this;
    }
}

// Implement : IStringBuilderIndentLevel
partial class XmlContent : IStringBuilderIndentLevel
{
    StringBuilder IStringBuilderIndentLevel.GetStringBuilderIndentLevel(uint level, bool isFollowingContent)
    {
        return new StringBuilder(_text);
    }
}

// Implement : IXmlAttribute
public partial class XmlAttribute : IXmlAttribute
{
    private string _name;
    private string _value;

    string IXmlAttribute.Name
    {
        get { return _name; }
        set { _name = value; }
    }

    string IXmlAttribute.Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public XmlAttribute(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public override string ToString()
    {
        return new StringBuilder(_name)
            .Append("=\"")
            .Append(_value)
            .Append("\"")
            .ToString();
    }
}

// Implement : ICloneable<IXmlAttribute>
partial class XmlAttribute : ICloneable<IXmlAttribute>
{
    IXmlAttribute ICloneable<IXmlAttribute>.Clone()
    {
        return new XmlAttribute(_name, _value);
    }
}

// Implement : IXmlElement
public partial class XmlElement<XmlAttributes, XmlChilds> : IXmlElement where XmlAttributes : ICollection<IXmlAttribute>, new()
                                                                        where XmlChilds : ICollection<IXmlChild>, new()
{
    private string _name;
    private ICollection<IXmlAttribute> _attributes;
    private ICollection<IXmlChild> _childs ;

    string IXmlElement.Name
    {
        get { return _name; }
        set { _name = value; }
    }

    ICollection<IXmlAttribute> IXmlElement.Attributes
    {
        get { return _attributes; }
        set {
            if (value == null) _attributes.Clear();
            else _attributes = value;
        }
    }

    ICollection<IXmlChild> IXmlElement.Childs
    {
        get { return _childs; }
        set
        {
            if (value == null) _childs.Clear();
            else _childs = value;
        }
    }

    public XmlElement(string name) : this(name, new XmlAttributes(), new XmlChilds()) {}

    public XmlElement(string name, ICollection<IXmlAttribute> xmlAttributes) : this(name, xmlAttributes, new XmlChilds()) { }

    public XmlElement(string name, ICollection<IXmlChild> xmlChilds) : this(name, new XmlAttributes(), xmlChilds) { }

    public XmlElement(string name, ICollection<IXmlAttribute> xmlAttributes, ICollection<IXmlChild> xmlChilds)
    {
        _name = name;
        _attributes = xmlAttributes;
        _childs = xmlChilds;
    }

    IXmlElement IXmlElement.AddAttribute(IXmlAttribute xmlAttribute)
    {
        _attributes.Add(xmlAttribute);
        return this;
    }

    IXmlElement IXmlElement.AddAttribute(string name, string value)
    {
        return (this as IXmlElement).AddAttribute(new XmlAttribute(name, value));
    }

    IXmlElement IXmlElement.AddChild(IXmlChild child)
    {
        _childs.Add(child);
        return this;
    }

    IXmlElement IXmlElement.AddChild(string text)
    {
        return (this as IXmlElement).AddChild(new XmlContent(text));
    }

    public override string ToString()
    {
        return (this as IStringBuilderIndentLevel).GetStringBuilderIndentLevel(0).ToString();
    }
}

// Implement : ICloneable<IXmlChild>
partial class XmlElement<XmlAttributes, XmlChilds> : ICloneable<IXmlChild>
{
    IXmlChild ICloneable<IXmlChild>.Clone()
    {
        return (this as ICloneable<IXmlElement>).Clone();
    }
}

// Implement : ICloneable<IXmlElement>
partial class XmlElement<XmlAttributes, XmlChilds> : ICloneable<IXmlElement>
{
    IXmlElement ICloneable<IXmlElement>.Clone()
    {
        IXmlElement xmlElement = new XmlElement<XmlAttributes, XmlChilds>(_name);
        foreach (IXmlAttribute attr in _attributes) xmlElement.AddAttribute(attr.Clone());
        foreach (IXmlChild child in _childs) xmlElement.AddChild(child.Clone());
        return xmlElement;
    }
}

// Implement : IXmlChild
partial class XmlElement<XmlAttributes, XmlChilds> : IXmlChild
{
    XmlChildType IXmlChild.ChildType
    {
        get { return XmlChildType.Element; }
    }

    IXmlElement IXmlChild.AsIXmlElement()
    {
        return this;
    }

    IXmlContent IXmlChild.AsIXmlContent()
    {
        return null;
    }
}

// Implement : IStringBuilderIndentLevel
partial class XmlElement<XmlAttributes, XmlChilds> : IStringBuilderIndentLevel
{
    StringBuilder IStringBuilderIndentLevel.GetStringBuilderIndentLevel(uint level, bool isFollowingContent)
    {
        StringBuilder indentStr = new StringBuilder();
        StringBuilder attributesStr = new StringBuilder();
        StringBuilder contentStr = new StringBuilder();

        if(!isFollowingContent) for (uint i = 0; i < level; i++) indentStr.Append("\t");

        switch (_childs.Count)
        {
            case 0:
                break;
            case 1:
                {
                    IEnumerator<IXmlChild> enumerator = _childs.GetEnumerator();
                    enumerator.MoveNext();
                    IXmlChild child = enumerator.Current;
                    switch (child.ChildType)
                    {
                        case XmlChildType.Content:
                            contentStr.Append(child.GetStringBuilderIndentLevel(0));
                            break;
                        case XmlChildType.Element:
                            contentStr.Append("\n");
                            contentStr.Append(child.GetStringBuilderIndentLevel(level + 1));
                            contentStr.Append("\n");
                            contentStr.Append(indentStr);
                            break;
                    }
                    break;
                }
            default:
                {
                    bool isContentLast = false;

                    foreach (IXmlChild child in _childs)
                    {

                        switch (child.ChildType)
                        {
                            case XmlChildType.Content:
                                isContentLast = true;
                                contentStr.Append(child.GetStringBuilderIndentLevel(0));
                                break;
                            case XmlChildType.Element:
                                if (isContentLast)
                                {
                                    isContentLast = false;
                                    contentStr.Append(child.GetStringBuilderIndentLevel(level + 1, true));
                                }
                                else
                                {
                                    isContentLast = false;
                                    contentStr.Append("\n");
                                    contentStr.Append(child.GetStringBuilderIndentLevel(level + 1));
                                }
                                break;
                        }
                    }

                    if(!isContentLast)
                    {
                        contentStr.Append("\n");
                        contentStr.Append(indentStr);
                    }
                    break;
                }
        }

        foreach (IXmlAttribute attr in _attributes)
        {
            attributesStr.Append(" ");
            attributesStr.Append(attr);
        }

        return new StringBuilder()
            .Append(indentStr)
            .Append("<")
            .Append(_name)
            .Append(attributesStr)
            .Append(">")
            .Append(contentStr)
            .Append("</")
            .Append(_name)
            .Append(">");
    }
}

public static class XmlFactory<XmlAttributes, XmlChilds> where XmlAttributes : ICollection<IXmlAttribute>, new()
                                                         where XmlChilds : ICollection<IXmlChild>, new()
{
    public static IXmlElement Element(string name, ICollection<IXmlAttribute> xmlAttributes, ICollection<IXmlChild> xmlChilds)
    {
        return new XmlElement<XmlAttributes, XmlChilds>(name, xmlAttributes, xmlChilds);
    }

    public static IXmlElement Element(string name, ICollection<IXmlAttribute> xmlAttributes)
    {
        return new XmlElement<XmlAttributes, XmlChilds>(name, xmlAttributes);
    }

    public static IXmlElement Element(string name, ICollection<IXmlChild> xmlChilds)
    {
        return new XmlElement<XmlAttributes, XmlChilds>(name, xmlChilds);
    }

    public static IXmlElement Element(string name)
    {
        return new XmlElement<XmlAttributes, XmlChilds>(name);
    }

    public static IXmlContent Content(string name)
    {
        return new XmlContent(name);
    }

    public static IXmlAttribute Attribute(string name, string value)
    {
        return new XmlAttribute(name, value);
    }
}