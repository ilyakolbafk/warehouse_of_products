using System.Windows;
using System.Windows.Controls;

namespace _1C
{
    /// <summary>
    ///     Interaction logic for RandomFill.xaml
    /// </summary>
    public partial class RandomFill
    {
        // Default number of products.
        private string _oldProducts = "0";

        // Default value for number of sections.
        private string _oldSections = "0";

        public RandomFill()
        {
            InitializeComponent();
        }

        // Number of sections.
        public int Sections
        {
            get => int.Parse(NumberOfSections.Text);
            set => NumberOfSections.Text = value.ToString();
        }

        // Number of products.
        public int Products
        {
            get => int.Parse(NumberOfProducts.Text);
            set => NumberOfProducts.Text = value.ToString();
        }

        /// <summary>
        ///     Check for the correctness of the filled in fields and close dialog box.
        /// </summary>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfProducts.Text.Length == 0 || NumberOfSections.Text.Length == 0)
                MessageBox.Show("Заполните все поля.");
            else if (Sections == 0 && Products != 0)
                MessageBox.Show("Нельзя добавить товары на склад без категорий.");
            else DialogResult = true;
        }

        /// <summary>
        ///     Control the numerical value of the number of sections.
        /// </summary>
        private void NumberOfSections_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(NumberOfSections.Text, out var num) || num < 0 || num > 1000)
            {
                MessageBox.Show(num > 1000
                    ? "Для нормальной генерации, не вводите число категорий большее 1000"
                    : "Количество должно быть целым неотрицательным числом.");
                Sections = int.Parse(_oldSections);
            }
            else if (!int.TryParse(NumberOfSections.Text, out _))
            {
                MessageBox.Show("Количество должно быть целым неотрицательным числом.");
            }
            else
            {
                _oldSections = Sections.ToString();
            }
        }

        /// <summary>
        ///     Control the numerical value of the number of products.
        /// </summary>
        private void NumberOfProducts_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(NumberOfProducts.Text, out var num) || num < 0 || num > 10000)
            {
                MessageBox.Show(num > 10000
                    ? "Для нормальной генерации, не вводите число товаров большее 10000"
                    : "Количество должно быть целым неотрицательным числом.");
                Products = int.Parse(_oldProducts);
            }
            else if (!int.TryParse(NumberOfSections.Text, out _))
            {
                MessageBox.Show("Количество должно быть целым неотрицательным числом.");
            }
            else
            {
                _oldProducts = Products.ToString();
            }
        }
    }
}