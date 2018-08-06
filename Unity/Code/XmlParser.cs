using System.Collections.Generic;

using MyXml = XmlFactory<System.Collections.Generic.LinkedList<IXmlAttribute>, System.Collections.Generic.LinkedList<IXmlChild>>;

public static class XmlParser
{
    private class XmlParent
    {
        public uint level;
        public string name;
        public XmlParent parent;

        public XmlParent(uint level, string name, XmlParent parent)
        {
            this.level = level;
            this.name = name;
            this.parent = parent;
        }
    }

    private static bool IsSpecialCharExceptGreaterThan(char ch)
    {
        switch (ch)
        {
            case '<': return true;
            case '\"': return true;
            case '\'': return true;
        }
        return false;
    }

    private static bool IsSpecialChar(char ch)
    {
        if (IsSpecialCharExceptGreaterThan(ch) || ch == '>') return true;
        else return false;
    }

    public static bool IsWhiteChar(char ch)
    {
        switch (ch)
        {
            case ' ': return true;
            case '\t': return true;
            case '\n': return true;
            case '\r': return true;
            case '\b': return true;
            case '\f': return true;
            case '\v': return true;
        }
        return false;
    }

    public static bool SkipString(ref string text, ref string skip, ref int index)
    {
        int start = index;
        for (index = start; index < start + skip.Length; index++) if (index >= text.Length || text[index] != skip[index - start]) return false;
        return true;
    }

    public static bool SkipWhiteChar(ref string text, ref int index)
    {
        for (; index < text.Length; index++)
        {
            if (IsWhiteChar(text[index])) continue;
            else return true;
        }
        return false;
    }

    private static bool SkipLessThanChar(ref string text, ref int index)
    {
        if (!SkipWhiteChar(ref text, ref index)) return false;
        return text[index++] == '<';
    }

    private static bool SkipGreaterThanChar(ref string text, ref int index)
    {
        if (!SkipWhiteChar(ref text, ref index)) return false;
        return text[index++] == '>';
    }

    private static void SkipContentEnd(ref string text, ref int index, ref int contentEnd)
    {
        for (; index < text.Length; index++) if (IsWhiteChar(text[index]) || text[index] == '<') break;
        contentEnd = index;
    }

    public static bool IsContainNonWhiteChar(ref string text, int from, int to)
    {
        for (int i = from; i < to; i++) if (!IsWhiteChar(text[i])) return true;
        return false;
    }

    private static bool FindNextChar(ref string text, ref int index, char ch)
    {
        for (; index < text.Length; index++) if (text[index] == ch) return true;
        return false;
    }

    private static string GetElementName(ref string text, ref int index)
    {
        int start = index;
        char firstChar = text[index++];
        if (IsWhiteChar(firstChar) || IsSpecialChar(firstChar)) return null;
        for (; index < text.Length; index++)
        {
            char ch = text[index];
            if (IsWhiteChar(ch) || ch == '>') return text.Substring(start, index - start);
            else if (IsSpecialCharExceptGreaterThan(ch)) return null;
        }
        return null;
    }

    private static string GetAttributeName(ref string text, ref int index)
    {
        int start = index;
        char firstChar = text[index++];
        if (IsSpecialCharExceptGreaterThan(firstChar) || firstChar == '=') return null;
        for (; index < text.Length; index++)
        {
            char ch = text[index];
            if (IsWhiteChar(ch) || ch == '=') return text.Substring(start, index - start);
            else if (IsSpecialChar(ch)) return null;
        }
        return null;
    }

    private static bool MatchAttributeEnd(ref string text, ref int index, char ch, ref string name, LinkedList<IXmlAttribute> xmlAttributes)
    {
        int strStart = index;
        if (!FindNextChar(ref text, ref index, ch)) return false;
        xmlAttributes.AddLast(MyXml.Attribute(name, text.Substring(strStart, index - strStart)));
        index++;
        return true;
    }

    private static ICollection<IXmlAttribute> GetAttributes(ref string text, ref int index)
    {
        LinkedList<IXmlAttribute> xmlAttributes = new LinkedList<IXmlAttribute>();
        char ch = text[index++];
        if (ch == '>') return xmlAttributes;
        if (!SkipWhiteChar(ref text, ref index)) return null;
        while (text[index] != '>')
        {
            string name = GetAttributeName(ref text, ref index);
            if (name == null) return null;

            if (!SkipWhiteChar(ref text, ref index)) return null;
            if (text[index++] != '=') return null;
            if (!SkipWhiteChar(ref text, ref index)) return null;

            switch (text[index++])
            {
                case '\"':
                    if (!MatchAttributeEnd(ref text, ref index, '\"', ref name, xmlAttributes)) return null;
                    break;
                case '\'':
                    if (!MatchAttributeEnd(ref text, ref index, '\'', ref name, xmlAttributes)) return null;
                    break;
                default:
                    return null;
            }

            if (!SkipWhiteChar(ref text, ref index)) return null;
        }

        index++;
        return xmlAttributes;
    }

    private static IXmlElement GetElement(ref string text, ref int index, out uint endLevel, XmlParent parent)
    {
        endLevel = 0;
        if (!SkipLessThanChar(ref text, ref index)) return null;

        if (text[index] == '/')
        {
            index++;
            if (parent.parent == null) return null;
            XmlParent curr = parent;
            int skipStringIndex;
            do
            {
                skipStringIndex = index;
                if (SkipString(ref text, ref curr.name, ref skipStringIndex) && SkipGreaterThanChar(ref text, ref skipStringIndex))
                {
                    endLevel = curr.level;
                    break;
                }
                curr = curr.parent;
            }
            while (curr.parent != null);
            index = skipStringIndex;
            return null;
        }

        string elementName = GetElementName(ref text, ref index);
        if (elementName == null) return null;

        ICollection<IXmlAttribute> attributes = GetAttributes(ref text, ref index);
        if (attributes == null) return null;

        uint nextEndLevel;
        XmlParent self = new XmlParent(parent.level + 1, elementName, parent);
        ICollection<IXmlChild> xmlChilds = Parse(ref text, ref index, out nextEndLevel, self);

        if (nextEndLevel <= parent.level) endLevel = nextEndLevel;

        if (xmlChilds == null) return null;
        else return MyXml.Element(elementName, attributes, xmlChilds);
    }

    public static ICollection<IXmlChild> Parse(ref string text)
    {
        int index = 0;
        uint endLevel;
        XmlParent self = new XmlParent(0, "", null);
        return Parse(ref text, ref index, out endLevel, self);
    }

    private static ICollection<IXmlChild> Parse(ref string text, ref int index, out uint endLevel, XmlParent parent)
    {
        LinkedList<IXmlChild> xmlChilds = new LinkedList<IXmlChild>();

        int contentStart = index;
        int contentEnd = contentStart;
        endLevel = 0;
        while (index < text.Length)
        {
            IXmlElement xmlElement = GetElement(ref text, ref index, out endLevel, parent);
            if (endLevel != 0)
            {
                if (xmlElement != null) xmlChilds.AddLast(xmlElement);
                break;
            }
            else if (xmlElement == null) SkipContentEnd(ref text, ref index, ref contentEnd);
            else
            {
                if (contentEnd > contentStart) xmlChilds.AddLast(MyXml.Content(text.Substring(contentStart, contentEnd - contentStart)));
                xmlChilds.AddLast(xmlElement);
                contentStart = index;
                contentEnd = contentStart;
            }
        }
        if (parent.level == 0)
        {
            if (contentEnd > contentStart && IsContainNonWhiteChar(ref text, contentStart, contentEnd))
                xmlChilds.AddLast(MyXml.Content(text.Substring(contentStart, contentEnd - contentStart)));
            return xmlChilds;
        }
        else if (endLevel != 0)
        {
            if (contentEnd > contentStart) xmlChilds.AddLast(MyXml.Content(text.Substring(contentStart, contentEnd - contentStart)));
            return xmlChilds;
        }
        else return null;
    }
}