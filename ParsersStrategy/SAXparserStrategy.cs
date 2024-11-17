using System.Xml;
using System.Xml.Linq;

namespace LAB2_OOP
{
    public class SAXparserStrategy : IXMLparserStrategy
    {
        public IEnumerable<XElement> Execute(string filePath, List<string> attributes, List<string> keywords)
        {
            var results = new List<XElement>();
            XElement currentGraduate = null;
            XElement careerElement = null;
            bool insideGraduate = false;

            using (var reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "graduate")
                        {
                            insideGraduate = true;
                            currentGraduate = new XElement("graduate");
                            careerElement = new XElement("career");
                            currentGraduate.Add(careerElement);

                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    currentGraduate.SetAttributeValue(reader.Name, reader.Value);
                                }
                                reader.MoveToElement();
                            }
                        }
                        else if (reader.Name == "position" && insideGraduate)
                        {
                            var positionElement = new XElement("position");

                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    positionElement.SetAttributeValue(reader.Name, reader.Value);
                                }
                                reader.MoveToElement();
                            }

                            careerElement.Add(positionElement);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "graduate")
                    {
                        insideGraduate = false;

                        if (GraduateMatches(currentGraduate, attributes, keywords))
                        {
                            results.Add(currentGraduate);
                        }
                    }
                }
            }

            return results;
        }

        private bool GraduateMatches(XElement graduate, List<string> attributes, List<string> keywords)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                var attribute = attributes[i];
                var keyword = keywords[i];

                var value = graduate.Attribute(attribute)?.Value ?? string.Empty;
                if (!string.IsNullOrEmpty(value) && value.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var careerMatches = graduate.Element("career")?
                    .Elements("position")
                    .Any(position =>
                    {
                        var positionValue = position.Attribute(attribute)?.Value ?? string.Empty;
                        return positionValue.Contains(keyword, StringComparison.OrdinalIgnoreCase);
                    }) ?? false;

                if (!careerMatches)
                {
                    return false;
                }
            }

            return true;
        }
    }
}


