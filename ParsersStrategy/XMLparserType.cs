using System.Xml.Linq;

namespace LAB2_OOP
{
    public class XMLparserType
    {
        private IXMLparserStrategy strategy;

        public void Strategy(string parserType)
        {
            switch (parserType)
            {
                case "SAX":
                    strategy = new SAXparserStrategy();
                    break;
                case "DOM":
                    strategy = new DOMparserStrategy();
                    break;
                case "LINQ":
                    strategy = new LINQparserStrategy();
                    break;
                default:
                    throw new ArgumentException("Невідома стратегія парсера.");
            }
        }

        public IEnumerable<XElement> ExecuteSearch(string filePath, List<string> attributes, List<string> keywords)
        {
            if (strategy == null)
            {
                throw new InvalidOperationException("Стратегія парсера не встановлена.");
            }

            var results = strategy.Execute(filePath, attributes, keywords);

            foreach (var graduate in results)
            {
                if (graduate.Element("career") == null)
                {
                    graduate.Add(new XElement("career"));
                }
            }

            return results;
        }
    }
}
