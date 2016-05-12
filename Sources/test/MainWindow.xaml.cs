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

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum typesValidation
        {
            mixed = 0,
            letters = 1,
            phone = 2
        }

        struct inputScreen
        {
            public string label;
            public string input;
            public bool inputChanged;
            public typesValidation type;

            public void set(string inputLabel, string startText, typesValidation validation = typesValidation.mixed)
            {
                label = inputLabel;
                input = startText;
                inputChanged = false;
                type = validation;
            }
        }

        private const int amountScreens = 4;
        private inputScreen[] screens;
        private int screenId;
        private string errorMsg;
        
        public MainWindow()
        {
            screens = new inputScreen[amountScreens];
            errorMsg = "";
            screenId = 0;

            screens[0].set("Imię:", "Tutaj wpisz swoje imię", typesValidation.letters);
            screens[1].set("Nazwisko:", "Tutaj wpisz swoje nazwisko", typesValidation.letters);
            screens[2].set("Telefon:", "Tutaj wpisz swój numer telefonu", typesValidation.phone);
            screens[3].set("Adres:", "Tutaj wpisz swój adres");

            InitializeComponent();

            lInput.Content = screens[screenId].label;
            tbInput.Text = screens[screenId].input;
            tbInput.Focus();
            tbInput.SelectAll();
        }

        void inputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                nextScreen();
            }
        }

        private void btPrev_Click(object sender, RoutedEventArgs e)
        {
            prevScreen();
        }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            nextScreen();
        }

        private void nextScreen()
        {
            if (validate())
            {
                saveInput();
                screenId++;
                changeScreen();
            }
            else
            {
                msgError.Visibility = Visibility.Visible;
                msgError.Text = errorMsg;
            }
            tbInput.SelectAll();
        }

        private void prevScreen()
        {
            if (validate())
            {
                saveInput();
                screenId--;
                changeScreen();
            }
            else
            {
                msgError.Visibility = Visibility.Visible;
                msgError.Text = errorMsg;
            }
            tbInput.SelectAll();
        }

        private void changeScreen()
        {
            switch (screenId)
            {
                case 0:
                    btPrev.IsEnabled = false;
                    goto default;
                case 1:
                    btPrev.IsEnabled = true;
                    goto default;
                case (amountScreens - 1):
                    showInScreen();
                    goto default;
                case amountScreens:
                    btNext.IsEnabled = true;
                    showConfirmScreen();
                    break;
                case (amountScreens + 1):
                    btNext.IsEnabled = false;
                    showOutScreen();
                    break;
                default:
                    lInput.Content = screens[screenId].label;
                    tbInput.Text = screens[screenId].input;
                    msgError.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void saveInput()
        {
            if (screenId < amountScreens && !screens[screenId].inputChanged && !screens[screenId].input.Equals(tbInput.Text))
            {
                screens[screenId].inputChanged = true;
                screens[screenId].input = tbInput.Text;
            }
        }

        private bool validate()
        {
            if(screenId < amountScreens)
            switch (screens[screenId].type)
            {
                case typesValidation.letters:
                    errorMsg = "Formularz akceptuje tylko ciąg liter";
                    return tbInput.Text.All(Char.IsLetter);
                case typesValidation.phone:
                    errorMsg = "Formularz akceptuje tylko ciąg cyfr";
                    return tbInput.Text.All(sign => Char.IsDigit(sign) || sign=='+');
                default:
                    break;
            }
            return true;
        }

        private void showInScreen()
        {
            gOut.Visibility = Visibility.Hidden;
            gCon.Visibility = Visibility.Hidden;
            gIn.Visibility = Visibility.Visible;
        }

        private void showConfirmScreen()
        {
            gOut.Visibility = Visibility.Hidden;
            gCon.Visibility = Visibility.Visible;
            gIn.Visibility = Visibility.Hidden;
        }

        private void showOutScreen()
        {
            gOut.Visibility = Visibility.Visible;
            gCon.Visibility = Visibility.Hidden;
            gIn.Visibility = Visibility.Hidden;
            TextBox[] tbOutputs = {tbOut0 , tbOut1, tbOut2, tbOut3};

            for (int i = 0; i < amountScreens; i++)
            {
                tbOutputs[i].Text = screens[i].input;
            }
        }

        
    }
}
