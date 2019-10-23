using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace wpfOnlineCurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static readonly HttpClient client = new HttpClient();
        private async void calculateOnlineCurrency(string theUrl)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(theUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                responseBody = responseBody.Replace("\"", "\'");

                Console.WriteLine(responseBody);
                LoopCurrency(responseBody);
            }
            catch
            {
                MessageBox.Show("Gegevens niet op kunnen halen", "Fout - niet verbonden");
            }
        }
        private void LoopCurrency(string responseBody)
        {
            String[] target = new string[4];
            target[0] = "eur";
            target[1] = "usd";
            target[2] = "cny";
            target[3] = "gbp";
            Dictionary<string, Dictionary<string, string>> values = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(responseBody);
            CalculateOutcome(values[target[cbToValues.SelectedIndex]]["rate"].Replace(".", ","));
        }

        private void CalculateOutcome(string conversionRate)
        {
            float inputNumber = float.Parse(tbxAmountValue.Text);
            float result = inputNumber * float.Parse(conversionRate);
            double outcome = Convert.ToDouble(result);
            lblResultText.Content = "Het is omgerekend: " + outcome.ToString("0.00");
        }

        private void BtnStartCalc_Click(object sender, RoutedEventArgs e)
        {
            String[] urls = new string[4];
            urls[0] = "http://www.floatrates.com/daily/eur.json";
            urls[1] = "http://www.floatrates.com/daily/usd.json";
            urls[2] = "http://www.floatrates.com/daily/cny.json";
            urls[3] = "http://www.floatrates.com/daily/gbp.json";
            calculateOnlineCurrency(urls[cbFromValues.SelectedIndex]);
        }
    }
}
// Hallo