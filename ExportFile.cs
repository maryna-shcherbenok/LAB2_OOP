using System.Xml.Xsl;

namespace LAB2_OOP.Export
{
    public class ExportFile
    {
        public async Task ExportToHtml(string filePath)
        {
            string xslFilePath = Path.Combine(FileSystem.AppDataDirectory, "graduates.xsl");

            if (!File.Exists(xslFilePath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("graduates.xsl");
                using var fileStream = File.Create(xslFilePath);
                await stream.CopyToAsync(fileStream);
            }

            var outputHtml = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "graduates.html");

            try
            {
                var transformer = new XslCompiledTransform();
                transformer.Load(xslFilePath);
                transformer.Transform(filePath, outputHtml);

                await App.Current.MainPage.DisplayAlert("Повідомлення", $"Файл збережено: {outputHtml}", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Помилка", $"Не вдалося експортувати HTML: {ex.Message}", "OK");
            }
        }
    }
}
