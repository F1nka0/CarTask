using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Car> Cars = new List<Car>();
        static List<string> BasicStringValuesForTextBoxes = new List<string>(new string[] { "Номер", "Марка", "Заполните поле", " ", "", "Путь" });
        public MainWindow()
        {
            InitializeComponent();
            dg.ItemsSource = Cars;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (IsFieldFilled(Number) == false | IsFieldFilled(Mark) == false)
            {
                return;
            }
            else {
                Cars.Add(new Car(Number.Text, Failure.Text, Mark.Text));
                dg.Items.Refresh();
            }
        }
        public bool IsFieldFilled(TextBox tb) {

            if (BasicStringValuesForTextBoxes.Any(it => tb.Text == it))
            {
                tb.Background = Brushes.Red;
                tb.Text = "Заполните поле";
                return false;
            }
            else {
                tb.Background = Brushes.Transparent;
                return true;
            }
        }

        private void Number_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => Number.Text == it))
            {

                Number.Text = "";
            }
        }

        private void Mark_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => Mark.Text == it))
            {
                Mark.Text = "";
            }
        }

        private void Number_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => Number.Text == it))
            {
                Number.Text = "Номер";
            }
        }

        private void Mark_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => Mark.Text == it))
            {
                Mark.Text = "Марка";
            }
        }
        private bool AreAllInputsValid(string entireString, string number,string mark) {


            return new Regex(@"[а-я]\d{3}[а-я]{2}").IsMatch(number) && new Regex(@"([a-z]|[а-я])+").IsMatch(mark)&& entireString.Count(it=>it=='@') == 2;
        }
        private void GetFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] CurrentCar;
                StreamReader reader = new StreamReader(File.Open(XMLPath.Text,FileMode.Open,FileAccess.Read,FileShare.Read));
                foreach (string str in reader.ReadToEnd().Split('|')) {
                    CurrentCar = str.Split('@');
                    if ((AreAllInputsValid(str,CurrentCar.FirstOrDefault(), CurrentCar.LastOrDefault())==false)) { XMLPath.Text = "Файл не валиден"; return; }
                    Cars.Add(new Car(CurrentCar[0],CurrentCar[1],CurrentCar[2]));
                }
                dg.Items.Refresh();  
            }
            catch (Exception)
            {
                XMLPath.Text = "Файла не существует";
                return;
            }
        }

        private void XMLPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => XMLPath.Text == it))
            {
                XMLPath.Text = "";
            }
        }

        private void XMLPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => XMLPath.Text == it))
            {
                XMLPath.Text = "Путь";
            }
            
        }
    }

    class Car {
        public Car(string num,string fail,string CM)
        {
            Number = num;
            Failure = fail;
            CarMark = CM;
        }
        public string Number { get; }
        public string Failure { get; }
        public string CarMark { get; }
    }
}
