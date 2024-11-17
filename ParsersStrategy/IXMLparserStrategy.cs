using System.Xml.Linq;

namespace LAB2_OOP
{
    public interface IXMLparserStrategy
    {
        IEnumerable<XElement> Execute(string content, List<string> attributes, List<string> keywords);
    }
}
