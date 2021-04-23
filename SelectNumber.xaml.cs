using System.Windows;
using System.Windows.Controls;

namespace _1C
{
    /// <summary>
    ///     Interaction logic for SelectNumber.xaml
    /// </summary>
    public partial class SelectNumber
    {
        // Default value for number.
        private string _oldNumber = "0";

        public SelectNumber()
        {
            InitializeComponent();
        }

        // Number for csv table.
        public int Number
        {
            get => int.Parse(number.Text);
            set => number.Text = value.ToString();
        }

        /// <summary>
        ///     Check for the correctness of the filled in fields and close dialog box.
        /// </summary>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (number.Text.Length == 0)
                MessageBox.Show("Введите число");
            else DialogResult = true;
        }

        /// <summary>
        ///     Control the numerical value of the number.
        /// </summary>
        private void Number_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(number.Text, out var num) || num <= 0)
            {
                MessageBox.Show("Количество должно быть целым положительным числом.");
                Number = int.Parse(_oldNumber);
            }
            else
            {
                _oldNumber = Number.ToString();
            }
        }
    }
}