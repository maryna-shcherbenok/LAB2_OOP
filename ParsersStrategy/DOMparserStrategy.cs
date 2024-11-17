using System.Xml;
using System.Xml.Linq;

namespace LAB2_OOP
{
    public class DOMparserStrategy : IXMLparserStrategy
    {
        public IEnumerable<XElement> Execute(string filePath, List<string> attributes, List<string> keywords)
        {
            var results = new List<XElement>();
            var document = new XmlDocument();
            document.Load(filePath);

            foreach (XmlNode graduateNode in document.GetElementsByTagName("graduate"))
            {
                var graduateElement = XElement.Parse(graduateNode.OuterXml);

                if (MatchesAttributes(graduateElement, attributes, keywords))
                {
                    results.Add(graduateElement);
                }
            }

            return results;
        }

        private bool MatchesAttributes(XElement graduate, List<string> attributes, List<string> keywords)
        {
            foreach (var attribute in attributes)
            {
                var value = graduate.Attribute(attribute)?.Value ?? string.Empty;

                if (string.IsNullOrEmpty(value))
                {
                    foreach (var position in graduate.Element("career")?.Elements("position") ?? Enumerable.Empty<XElement>())
                    {
                        value = position.Attribute(attribute)?.Value ?? string.Empty;
                        if (!string.IsNullOrEmpty(value) && keywords.Any(keyword => value.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                        {
                            return true;
                        }
                    }
                }
                else if (keywords.Any(keyword => value.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}





