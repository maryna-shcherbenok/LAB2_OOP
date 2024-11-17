using LAB2_OOP.Export;
using LAB2_OOP.Manager;

namespace LAB2_OOP
{
    public partial class MainPage : ContentPage
    {
        private XMLparserType parserContext = new XMLparserType();

        private string selectedFilePath;

        private readonly AttributeManager attributeManager = new AttributeManager();
        private readonly ExportHelper exportHelper = new ExportHelper();

        public MainPage()
        {
            InitializeComponent();
            ParserPicker.ItemsSource = new string[] { "SAX", "DOM", "LINQ" };
        }

        private async void OnSelectFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                selectedFilePath = result.FullPath;
                LoadAttributes();
            }
        }

        private void LoadAttributes()
        {
            var attributes = attributeManager.GetAttributes(selectedFilePath);
            AttributeCollectionView.ItemsSource = attributes;
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchEntry.Text) ||
                AttributeCollectionView.SelectedItems.Count == 0 ||
                ParserPicker.SelectedIndex < 0)
            {
                DisplayAlert("Помилка", "Оберіть парсер, атрибути та введіть ключові слова.", "OK");
                return;
            }

            parserContext.Strategy(ParserPicker.SelectedItem.ToString());

            var attributes = AttributeCollectionView.SelectedItems
                .Cast<KeyValuePair<string, string>>()
                .Select(item => item.Value)
                .ToList();

            var keywords = SearchEntry.Text.Split(',')
                .Select(key => key.Trim())
                .ToList();

            if (keywords.Count != attributes.Count)
            {
                DisplayAlert("Помилка", "Перевірте відповідність між вибраними атрибутами пошуку та ключовими значеннями, які Ви ввели.", "OK");
                return;
            }

            var results = parserContext.ExecuteSearch(selectedFilePath, attributes, keywords);

            ResultsStackLayout.Children.Clear();
            if (!results.Any())
            {
                DisplayAlert("Результат", "Нічого не знайдено.", "OK");
                return;
            }

            foreach (var result in results)
            {
                ResultsStackLayout.Children.Add(new Label
                {
                    Text = attributeManager.FormatGraduateInfo(result),
                    TextColor = Colors.Black,
                    Padding = new Thickness(10),
                    BackgroundColor = Colors.LightGray,
                    Margin = new Thickness(0, 10)
                });
            }
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            SearchEntry.Text = string.Empty;
            ResultsStackLayout.Children.Clear();
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            await exportHelper.ExportToHtml(selectedFilePath);
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Повідомлення", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
            if (confirm)
            {
                Application.Current.Quit();
            }
        }
    }
}
