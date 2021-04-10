using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace Exam
{
    public partial class Tourist : Window
    {
        private readonly string FILE_XML_COUNTRYES = @"Countries.xml";
        private readonly string FILE_SAVE = @"Save.txt";
        private string _text = null;

        public Tourist()
        {
            InitializeComponent();
            image.Width = 1000;
            image.Height = 750;
            image.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void listCountryesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (ListBoxItem el in listCountryes.SelectedItems)
            {
                name.Content = el.Content;

                switch (el.Content)
                {
                    case "Norway":
                        Clear();
                        Short("Norway", @"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\Norwey.jpg");
                        image.Source = new BitmapImage(new Uri(@"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\Norwey.jpg"));
                        break;
                    case "Ukraine":
                        Clear();
                        Short("Ukraine", @"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\Ukraine.jpg");
                        image.Source = new BitmapImage(new Uri(@"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\Ukraine.jpg"));                    
                        break;
                    case "Japan":
                        Clear();
                        Short("Japan", @"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\Japan.jpg");
                        image.Source = new BitmapImage(new Uri(@"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\Japan.jpg"));
                        break;   
                    case "USA":
                        Clear();
                        Short("USA", @"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\USA.jpg");
                        image.Source = new BitmapImage(new Uri(@"E:\Flash_Usb\study\study\IT STEP\c#\WPF\Exam\Exam\Exam\Exam\bin\Debug\img\USA.jpg"));
                        break;
                }
            }
        }

        private void Short(string country, string pathImage)
            {
              ReadFromXml(country, optionDanger.IsChecked.ToString(), pathImage);
              
            }

        private void Clear()
        {
            city.Content = "";
            description.Content = "";
            facts.Content = "";
            timeZone.Content = "";
            weather.Content = "";
            attractions.Content = "";
            danger.Content = "";
            currency.Content = "";
            price.Content = "";
        }

        private void ReadFromXml(string country, string flag, string pathImage)
        {
            
            var fs = new FileStream(FILE_XML_COUNTRYES, FileMode.Open, FileAccess.Read);
            var xml = XDocument.Load(fs);
            var buff = flag.ToLower();

            foreach (var item in xml.Descendants("Country"))
            {
                item.Element("Danger").Value.ToLower();

                if (country == item.Element("Name").Value && item.Element("Danger").Value.ToString().ToLower() == buff)
                {
                    name.Content = item.Element("Name").Value;
                    city.Content = $" Name city: {item.Element("City").Value}";
                    description.Content = $"Discribe: {item.Element("Description").Value}";
                    facts.Content = $"Facts: {item.Element("Facts").Value}";
                    timeZone.Content = $"Time zone: {item.Element("TimeZone").Value}";
                    weather.Content = $"Wheather: {item.Element("Weather").Value}";
                    attractions.Content = $"Atractions: {item.Element("Attractions").Value}";
                    currency.Content = $"Currency: {item.Element("Currency").Value}";
                    danger.Content = $"Danger: {item.Element("Danger").Value}";
                    price.Content = $"Price: {item.Element("Price").Value} {item.Element("Currency").Value}s";
                    //image.Source = new BitmapImage(new Uri(item.Element("img").Value));

                    break;
                }
                else
                {
                    name.Content = "Ivalid paraments";
                }
            }

            _text = $"\t{name.Content}\n" +
                    $"{city.Content}\n" +
                    $"{description.Content}\n" +
                    $"{facts.Content}\n" +
                    $"{timeZone.Content}\n" +
                    $"{weather.Content}\n" +
                    $"{attractions.Content}\n" +
                    $"{currency.Content}\n" +
                    $"{danger.Content}\n" +
                    $"{price.Content}\n";
                    
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pdf.IsChecked == true)
            {
                SaveToPdfFile(".pdf", _text);
            }
            else if (doc.IsChecked == true)
            {
                SaveToFile(".doc", _text);
            }
            else if (txt.IsChecked == true)
            {
                SaveToFile(".txt", _text);
            }
        }

        private void SaveToFile(string type, string text)
        {
            var path = $"{FILE_SAVE}{type}";

            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(text);
                }
            }

            Process.Start(path);
        }

        private void SaveToPdfFile(string type, string text)
        {
            var path = $"{FILE_SAVE}{type}";
            var oDoc = new Document();
            PdfWriter.GetInstance(oDoc, new FileStream(path, FileMode.Append));

            oDoc.Open();
            oDoc.Add(new Paragraph(text));
            oDoc.Close();

            Process.Start(path);
        }
    }
}
