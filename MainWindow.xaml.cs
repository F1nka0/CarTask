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
        static List<string> BasicStringValuesForTextBoxes = new List<string>(new string[] { "Неисправность","Номер", "Марка", "Заполните поле", " ", "", "Путь" });
        public MainWindow()
        {
            InitializeComponent();
            dg.ItemsSource = Cars;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new Regex(@"[а-я]\d{3}[а-я]{2}").IsMatch(Number.Text)==false)
            {
                Number.Text = "Номер введён некорректно";
                return;
            }
            if (new Regex(@"^([a-z]+|[а-я]+)$").IsMatch(Mark.Text)==false)
            {
                Mark.Text = "Марка введена некорректно";
                return;
            }
            if (new Regex(@"^[0-9]+$").IsMatch(Failure.Text) == false)
            {
                Failure.Text = "Неисправность введена некорректно";
                return;
            }
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
        private bool AreAllInputsValid(string entireString, string number,string mark,string failure) {
            return new Regex(@"[а-я]\d{3}[а-я]{2}").IsMatch(number) && new Regex(@"([a-z]|[а-я])+").IsMatch(mark)&& entireString.Count(it=>it=='@') == 2&& new Regex(@"^[0-9]+$").IsMatch(failure);
        }
        private void GetFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Car> tempCarList = new List<Car>();
                string[] CurrentCar;
                StreamReader reader = new StreamReader(File.Open(XMLPath.Text,FileMode.Open,FileAccess.Read,FileShare.Read));
                foreach (string str in reader.ReadToEnd().Split('|')) {
                    CurrentCar = str.Split('@');
                    if ((AreAllInputsValid(str,CurrentCar.FirstOrDefault(), CurrentCar.LastOrDefault(), CurrentCar.Skip(1).FirstOrDefault()) ==false)) { XMLPath.Text = "Файл не валиден"; return; }
                    tempCarList.Add(new Car(CurrentCar[0],CurrentCar[1].Aggregate("",(a,b)=>a+" "+b),CurrentCar[2]));
                }
                Cars.AddRange(tempCarList);
                tempCarList.Clear();
                dg.Items.Refresh();
                reader.Close();
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

        private void Failure_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => Failure.Text == it))
            {
                Failure.Text = "";
            }
        }

        private void Failure_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BasicStringValuesForTextBoxes.Any(it => Failure.Text == it))
            {
                Failure.Text = "Неисправность";
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
