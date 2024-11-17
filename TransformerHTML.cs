using System.Xml.Xsl;

public class TransformerHTML
{
    public void TransformXMLtoHTML(string xmlFilePath, string xslFilePath, string outputHtmlPath)
    {
        var xslt = new XslCompiledTransform();
        xslt.Load(xslFilePath);
        xslt.Transform(xmlFilePath, outputHtmlPath);
    }
}
