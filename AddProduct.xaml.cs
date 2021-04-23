using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace _1C
{
    /// <summary>
    ///     Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct
    {
        // Default value for number.
        private string _oldNumber = "0";

        // Default value for price.
        private string _oldPrice = "1";

        public AddProduct()
        {
            InitializeComponent();
        }

        // Name of new product.
        public string PrName
        {
            get => ProductName.Text;
            set => ProductName.Text = value;
        }

        // Code of new product.
        public string PrCode
        {
            get => ProductCode.Text;
            set => ProductCode.Text = value;
        }

        // Price of new product.
        public double PrPrice
        {
            get => double.Parse(ProductPrice.Text);
            set => ProductPrice.Text = value.ToString(CultureInfo.CurrentCulture);
        }

        // Number of new product.
        public int PrNumber
        {
            get => int.Parse(ProductNumber.Text);
            set => ProductNumber.Text = value.ToString();
        }

        // Description of new product.
        public string PrDescription
        {
            get => ProductDescription.Text;
            set => ProductDescription.Text = value;
        }

        // Image of new product.
        public ImageSource PrImage
        {
            get => ProductImage.Source;
            set => ProductImage.Source = value;
        }

        /// <summary>
        ///     Check for the correctness of the filled in fields and close dialog box.
        /// </summary>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (PrName.Length == 0)
                ProductName.BorderBrush = Brushes.Red;
            if (PrCode.Length == 0)
                ProductCode.BorderBrush = Brushes.Red;
            if (ProductPrice.Text.Length == 0)
                ProductPrice.BorderBrush = Brushes.Red;
            if (ProductNumber.Text.Length == 0)
                ProductNumber.BorderBrush = Brushes.Red;
            if (PrName.Length == 0 || PrCode.Length == 0 || ProductPrice.Text.Length == 0 ||
                ProductNumber.Text.Length == 0)
            {
                MessageBox.Show("Заполните обязательные поля");
            }
            else if (MainWindow.Codes.Contains(PrCode))
            {
                MessageBox.Show("Товар с введенным артикулом уже существует");
                ProductCode.BorderBrush = Brushes.Red;
            }
            else
            {
                DialogResult = true;
            }
        }

        /// <summary>
        ///     Cancel creating new product.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        ///     Control the numerical value of the price.
        /// </summary>
        private void ProductPrice_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProductPrice.Text.Length > 0 && ProductPrice.Text[^1] == ',')
                ProductPrice.Text += '1';
            if (!double.TryParse(ProductPrice.Text, out var price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.");
                PrPrice = double.Parse(_oldPrice);
            }
            else
            {
                _oldPrice = ProductPrice.Text;
                PrPrice = double.Parse(_oldPrice);
                ProductPrice.BorderBrush = _oldPrice.Length > 0 ? Brushes.LightGreen : Brushes.HotPink;
            }
        }

        /// <summary>
        ///     Control the numerical value of the number.
        /// </summary>
        private void ProductNumber_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(ProductNumber.Text, out var number) || number < 0)
            {
                MessageBox.Show("Количество должно быть целым неотрицательным числом.");
                PrNumber = int.Parse(_oldNumber);
            }
            else
            {
                _oldNumber = ProductNumber.Text;
                PrNumber = int.Parse(_oldNumber);
                ProductNumber.BorderBrush = _oldNumber.Length > 0 ? Brushes.LightGreen : Brushes.HotPink;
            }
        }

        /// <summary>
        ///     Show the correctness / incorrectness of filling in the data.
        /// </summary>
        private void ProductCode_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ProductCode.BorderBrush = ProductCode.Text.Length > 0 ? Brushes.LightGreen : Brushes.HotPink;
            if (MainWindow.Codes.Contains(PrCode))
                ProductCode.BorderBrush = Brushes.HotPink;
        }

        /// <summary>
        ///     Show the correctness / incorrectness of filling in the data.
        /// </summary>
        private void ProductName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ProductName.BorderBrush = ProductName.Text.Length > 0 ? Brushes.LightGreen : Brushes.HotPink;
        }

        /// <summary>
        ///     Load product picture into the program.
        /// </summary>
        private void OpenImage_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog {Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"};
                if (openFileDialog.ShowDialog() == true)
                    PrImage = new ImageSourceConverter().ConvertFromString(openFileDialog.FileName) as ImageSource;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}