using System.Xml.Linq;

namespace LAB2_OOP.Manager
{
    public class AttributeManager
    {
        private readonly Dictionary<string, string> attributeTranslations = new Dictionary<string, string>
        {
            { "fullName", "П.І.Б" },
            { "faculty", "Факультет" },
            { "department", "Кафедра" },
            { "specialty", "Спеціальність" },
            { "admissionYear", "Рік вступу" },
            { "graduationYear", "Рік випуску" },
            { "role", "Посада" },
            { "company", "Компанія" },
            { "start", "Рік початку роботи" },
            { "end", "Рік звільнення" }
        };

        public List<KeyValuePair<string, string>> GetAttributes(string filePath)
        {
            var document = XDocument.Load(filePath);
            var graduate = document.Descendants("graduate").FirstOrDefault();
            var attributes = new List<KeyValuePair<string, string>>();

            if (graduate != null)
            {
                foreach (var attr in graduate.Attributes())
                {
                    attributes.Add(new KeyValuePair<string, string>(
                        attributeTranslations.GetValueOrDefault(attr.Name.LocalName, attr.Name.LocalName),
                        attr.Name.LocalName));
                }

                foreach (var position in graduate.Element("career")?.Elements("position") ?? Enumerable.Empty<XElement>())
                {
                    foreach (var attr in position.Attributes())
                    {
                        if (!attributes.Any(a => a.Value == attr.Name.LocalName))
                        {
                            attributes.Add(new KeyValuePair<string, string>(
                                attributeTranslations.GetValueOrDefault(attr.Name.LocalName, attr.Name.LocalName),
                                attr.Name.LocalName));
                        }
                    }
                }
            }

            return attributes;
        }

        public string FormatGraduateInfo(XElement graduate)
        {
            var info = new List<string>
            {
                $"• П.І.Б: {graduate.Attribute("fullName")?.Value ?? "Невідомо"}",
                $"• Факультет: {graduate.Attribute("faculty")?.Value ?? "Невідомо"}",
                $"• Кафедра: {graduate.Attribute("department")?.Value ?? "Невідомо"}",
                $"• Спеціальність: {graduate.Attribute("specialty")?.Value ?? "Невідомо"}",
                $"• Роки навчання: {graduate.Attribute("admissionYear")?.Value ?? "Невідомо"} - {graduate.Attribute("graduationYear")?.Value ?? "Невідомо"}"
            };

            var positions = graduate.Element("career")?.Elements("position");

            if (positions != null && positions.Any())
            {
                info.Add("• Кар'єра:");
                foreach (var position in positions)
                {
                    info.Add($"--- Посада: {position.Attribute("role")?.Value ?? "Невідомо"}");
                    info.Add($"----- Компанія: {position.Attribute("company")?.Value ?? "Невідомо"}");
                    info.Add($"------- Роки роботи: {position.Attribute("start")?.Value ?? "Невідомо"} - {position.Attribute("end")?.Value ?? "Невідомо"}");
                }
            }
            else { info.Add("Кар'єра: Дані відсутні"); }

            return string.Join("\n", info);
        }
    }
}
