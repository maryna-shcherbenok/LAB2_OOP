using System.Xml.Linq;

namespace LAB2_OOP
{
    public class LINQparserStrategy : IXMLparserStrategy
    {
        public IEnumerable<XElement> Execute(string filePath, List<string> attributes, List<string> keywords)
        {
            var doc = XDocument.Load(filePath);

            return doc.Descendants("graduate")
                      .Where(graduate => MatchesAttributes(graduate, attributes, keywords))
                      .ToList();
        }

        private bool MatchesAttributes(XElement graduate, List<string> attributes, List<string> keywords)
        {
            foreach (var attribute in attributes)
            {
                var value = graduate.Attribute(attribute)?.Value ?? string.Empty;

                // Якщо значення не знайдено, перевіряємо вкладені `position`
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
