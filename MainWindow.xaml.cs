using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace _1C
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Variables for generate words in sections and product.
        private const string Rus = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private const string RusFirst = "абвгдеёжзийклмнопрстуфхцчшщэюя";

        // List of product codes.
        public static List<string> Codes = new List<string>();
        private readonly Random _rnd = new Random();
        private readonly List<string> _russianWords;

        private readonly int _russianWordsCount;

        // Root sections.
        private ObservableCollection<Section> _sections;
        public Section SelectedSection;

        /// <summary>
        ///     Launching the application and initializing the interface and elements.
        /// </summary>
        public MainWindow()
        {
            try
            {
                _russianWords = File.ReadAllLines("russian.txt", Encoding.UTF8).ToList();
                _russianWordsCount = _russianWords.Count;
            }
            catch
            {
                _russianWordsCount = 0;
            }

            InitializeComponent();
            _sections = new ObservableCollection<Section>();
            XmlDeserialize();
            Registry.ItemsSource = _sections;
        }

        /// <summary>
        ///     Add a section to the warehouse.
        /// </summary>
        private void AddSection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Launch a dialog box for selecting a section name.
                var addSectionDialog = new AddSection();
                if (addSectionDialog.ShowDialog() != true) return;
                var name = addSectionDialog.SectionName;
                var newSection = new Section {Name = name, Parent = SelectedSection};
                // Check for same names.
                if (SelectedSection is null && _sections.All(x => x.Name != name))
                {
                    _sections.Add(newSection);
                    Save.IsEnabled = true;
                }
                else if (SelectedSection != null && SelectedSection.Sections.All(x => x.Name != name))
                {
                    SelectedSection.Sections.Add(newSection);
                    Save.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Введенное имя уже существует в этом разделе.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Edit a section in the warehouse.
        /// </summary>
        private void EditSection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Launch a dialog box for selecting a new section name.
                if (SelectedSection == null) return;
                var addSectionDialog = new AddSection {SectionName = SelectedSection.Name};
                if (addSectionDialog.ShowDialog() != true) return;
                var newName = addSectionDialog.SectionName;
                // Check for same names.
                if (SelectedSection.Parent == null && _sections.Any(x => x.Name == newName) &&
                    SelectedSection.Name != newName)
                    MessageBox.Show("Введенное имя уже существует в этом разделе.");
                else if (SelectedSection.Parent != null && SelectedSection.Parent.Sections.Any(x => x.Name == newName))
                    MessageBox.Show("Введенное имя уже существует в этом разделе.");
                else SelectedSection.Name = newName;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Remove section from the warehouse.
        /// </summary>
        private void RemoveSection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSection == null) return;
                // Check for non-empty section.
                if ((SelectedSection.Sections.Count != 0 || SelectedSection.Products.Count != 0) &&
                    MessageBox.Show("Раздел не пуст, хотите продолжить?", "Удаление",
                        MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
                if (SelectedSection.Parent == null)
                    _sections.Remove(SelectedSection);
                else
                    SelectedSection.Parent.Sections.Remove(SelectedSection);
                // Update TreeView, list of product codes, context menu items.
                Registry.Items.Refresh();
                MakeCodes();
                if (_sections.Count == 0) Save.IsEnabled = false;
                if (Codes.Count == 0) Report.IsEnabled = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Process the change of the selected section.
        /// </summary>
        private void Registry_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MakeGrid();
        }

        /// <summary>
        ///     Updates the datagrid with products from the selected section and its subsections.
        /// </summary>
        private void MakeGrid()
        {
            try
            {
                SelectedSection = (Section) Registry.SelectedItem;
                if (SelectedSection == null) return;
                ProductGrid.ItemsSource = SelectedSection.Products.ToList();
                var count = SelectedSection.Sections.Sum(ProductCounter);
                if (count == 0) return;
                ((List<Product>) ProductGrid.ItemsSource).Add(new Product
                    {Name = "Товары из подкатегорий:", Number = count});
                foreach (var section in SelectedSection.Sections)
                foreach (var product in MakeGridRecurse(section))
                    ((List<Product>) ProductGrid.ItemsSource).Add(product);
                ProductGrid.Items.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        ///     Counts the number of products in a section and its subsections.
        /// </summary>
        private static int ProductCounter(Section section)
        {
            var count = section.Products.Count;
            count += section.Sections.Sum(ProductCounter);
            return count;
        }

        /// <summary>
        ///     Returns the number of products in a section and its subsections.
        /// </summary>
        private static List<Product> MakeGridRecurse(Section section)
        {
            var list = section.Products.ToList();
            list.AddRange(section.Sections.SelectMany(MakeGridRecurse));
            return list;
        }

        /// <summary>
        ///     Add a product to the selected section.
        /// </summary>
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSection == null) return;
                // Launch a dialog box for selecting a product info.
                var addProductDialog = new AddProduct();
                if (addProductDialog.ShowDialog() != true) return;
                SelectedSection.Products.Add(new Product
                {
                    Name = addProductDialog.PrName,
                    Code = addProductDialog.PrCode,
                    Price = addProductDialog.PrPrice,
                    Number = addProductDialog.PrNumber,
                    Description = addProductDialog.ProductDescription.Text,
                    Image = addProductDialog.PrImage
                });
                // Update list of product codes, context menu items.
                MakeCodes();
                Report.IsEnabled = true;
                MakeGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Edit a product in the selected section.
        /// </summary>
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSection == null || ProductGrid.SelectedItem == null) return;
                // Check where the product are.
                var rowIndex = ProductGrid.SelectedIndex;
                if (rowIndex >= SelectedSection.Products.Count) return;
                // Launch a dialog box for selecting a new product info.
                var selectedProduct = SelectedSection.Products[rowIndex];
                Codes.Remove(selectedProduct.Code);
                var addProductDialog = new AddProduct
                {
                    PrName = selectedProduct.Name,
                    PrCode = selectedProduct.Code,
                    PrPrice = selectedProduct.Price,
                    PrNumber = selectedProduct.Number,
                    PrDescription = selectedProduct.Description,
                    PrImage = selectedProduct.Image
                };
                if (addProductDialog.ShowDialog() != true) return;
                SelectedSection.Products[rowIndex] = new Product
                {
                    Name = addProductDialog.PrName,
                    Code = addProductDialog.PrCode,
                    Price = addProductDialog.PrPrice,
                    Number = addProductDialog.PrNumber,
                    Description = addProductDialog.ProductDescription.Text,
                    Image = addProductDialog.PrImage
                };
                // Update list of product codes, context menu items.
                MakeCodes();
                MakeGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Remove a product form the selected section.
        /// </summary>
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSection == null || ProductGrid.SelectedItem == null) return;
                // Check where the product are.
                var rowIndex = ProductGrid.SelectedIndex;
                if (rowIndex >= SelectedSection.Products.Count) return;
                SelectedSection.Products.RemoveAt(rowIndex);
                // Update list of product codes, context menu items.
                MakeCodes();
                if (Codes.Count == 0) Report.IsEnabled = false;
                MakeGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Open product picture in a new window.
        /// </summary>
        private void OpenPicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSection == null || ProductGrid.SelectedItem == null) return;
                // Check where the product are.
                var rowIndex = ProductGrid.SelectedIndex;
                if (rowIndex >= SelectedSection.Products.Count ||
                    SelectedSection.Products[rowIndex].Image == null) return;
                new ProductImage {PrImage = SelectedSection.Products[rowIndex].Image}.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Serialize data to xml file.
        /// </summary>
        private void XmlSerialize(string path = "sections.xml")
        {
            try
            {
                var formatter = new XmlSerializer(typeof(ObservableCollection<Section>));
                using var fs = File.Create(path);
                formatter.Serialize(fs, _sections);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        ///     Deserialize data from xml file.
        /// </summary>
        private void XmlDeserialize(string path = "sections.xml")
        {
            try
            {
                var formatter = new XmlSerializer(typeof(ObservableCollection<Section>));
                if (!File.Exists(path))
                {
                    MessageBox.Show(
                        "1. Для добавления раздела необходимо нажать пкм по левой части окна.\n2. Для " +
                        "изменения или удаления раздела необходимо перед нажатием пкм выбрать необхоимый раздел лкм" +
                        ".\n3. Для добавления товара, необходимо выбрать раздел и нажать пкм по правой части окна.\n" +
                        "4. Для просмотра фото, изменения или удаления товара, необходимо выбрать лкм товар, а затем" +
                        " нажать пкм.\n5. Действия с товаром производить можно только в том разделе, в котором он " +
                        "находится.\n6. Границы автогенерации составляют 1000 для разделов и 10000 для товаров.\n7." +
                        " В программе выполнен весь функционал, кроме пункта 8, то есть 3 из 4 дополнительных функций.");
                    return;
                }

                using var fs = new FileStream(path, FileMode.OpenOrCreate);
                _sections = (ObservableCollection<Section>) formatter.Deserialize(fs);
                // Update sections parents, list of product codes, context menu items.
                MakeCodes();
                foreach (var section in _sections)
                    MakeParents(section);
                if (_sections.Count != 0) Save.IsEnabled = true;
                if (Codes.Count != 0) Report.IsEnabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                _sections = new ObservableCollection<Section>();
                Codes = new List<string>();
            }
        }

        /// <summary>
        ///     Adds codes of all products in a section and its subsections to the list of codes.
        /// </summary>
        private static void RecursiveMakeCodes(Section section)
        {
            foreach (var product in section.Products)
                Codes.Add(product.Code);
            foreach (var s in section.Sections)
                RecursiveMakeCodes(s);
        }

        /// <summary>
        ///     Adds codes of all products to the list of codes.
        /// </summary>
        private void MakeCodes()
        {
            Codes = new List<string>();
            foreach (var section in _sections)
                RecursiveMakeCodes(section);
        }

        /// <summary>
        ///     Assigns a parent to all subsections of a section.
        /// </summary>
        private static void MakeParents(Section section)
        {
            foreach (var s in section.Sections)
            {
                s.Parent = section;
                MakeParents(s);
            }
        }

        /// <summary>
        ///     Serializes data in xml format to a user-selected file.
        /// </summary>
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog {Filter = "XML files (*.xml)|*.xml"};
                if (saveFileDialog.ShowDialog() == true)
                    XmlSerialize(saveFileDialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Deserializes data in xml format from a user-selected file.
        /// </summary>
        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog {Filter = "XML file (*.xml)|*.xml"};
                if (openFileDialog.ShowDialog() == true)
                    XmlDeserialize(openFileDialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Serializes data in xml format when the application on closing.
        /// </summary>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            XmlSerialize();
        }

        /// <summary>
        ///     Generate a report in csv format to a user-selected file.
        /// </summary>
        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var csv = new StringBuilder();
                csv.AppendLine("путь классификатора;артикул;название товара;количество на складе");
                // Launch a dialog box for selecting a number.
                var selectNumber = new SelectNumber();
                if (selectNumber.ShowDialog() != true) return;
                var number = selectNumber.Number;
                var saveFileDialog = new SaveFileDialog {Filter = "CSV file (*.csv)|*.csv"};
                if (saveFileDialog.ShowDialog() != true) return;
                foreach (var section in _sections.Where(x => x.Products.Count > 0 || x.Sections.Count > 0))
                    MakeReport(csv, "", number, section);
                File.WriteAllText(saveFileDialog.FileName, csv.ToString(), Encoding.UTF8);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Process product data in a section and its subsections.
        /// </summary>
        private static void MakeReport(StringBuilder csv, string path, int number, Section section)
        {
            foreach (var product in section.Products.Where(product => product.Number < number))
                csv.AppendLine($"{path}{section.Name};{product.Code};{product.Name};{product.Number}");
            foreach (var s in section.Sections.Where(x => x.Products.Count > 0 || x.Sections.Count > 0))
                MakeReport(csv, path + $"{section.Name} / ", number, s);
        }

        /// <summary>
        ///     Fill the warehouse with random sections and products.
        /// </summary>
        private void RandomFill_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var randomFillDialog = new RandomFill();
                // Launch a dialog box for selecting a number of sections and number of products.
                if (randomFillDialog.ShowDialog() != true) return;
                Codes = new List<string>();
                _sections = new ObservableCollection<Section>();
                var numberOfSections = randomFillDialog.Sections;
                if (numberOfSections > 0)
                {
                    var numberOfProducts = randomFillDialog.Products;
                    // Set the nesting depth of sections from 3 to 12.
                    var classifierDepth = Math.Min(
                        (int) Math.Round((_rnd.NextDouble() * 2 + 1) * (_rnd.NextDouble() * 2 + 2)),
                        numberOfSections / 3) + 1;
                    while (numberOfProducts != 0 || numberOfSections != 0)
                        RecursiveRandomFill(classifierDepth, ref numberOfSections,
                            ref numberOfProducts, _sections, 1);
                    // Update sections parents, list of product codes, context menu items.
                    foreach (var section in _sections)
                        MakeParents(section);
                }

                MakeCodes();
                Save.IsEnabled = _sections.Count != 0;
                Report.IsEnabled = Codes.Count != 0;
                Registry.ItemsSource = _sections;
                ProductGrid.Items.Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RecursiveRandomFill(int depth, ref int remainderOfSection, ref int remainderOfProducts,
            ObservableCollection<Section> sections, int count)
        {
            if (depth == 0 || remainderOfSection == 0 && remainderOfProducts == 0) return;
            // Set the maximum number of subsections of a section.
            var sectionNumber = remainderOfSection / (depth * count) + 3;
            // Create subsections in a section.
            for (var i = 0; i < _rnd.Next(1, sectionNumber); i++)
            {
                if (remainderOfSection == 0) break;
                sections.Add(new Section {Name = RandomName()});
                remainderOfSection -= 1;
            }

            if (sections.Count == 0) return;
            count *= sections.Count;
            // Set the maximum number of products of a section.
            var productNumber = remainderOfProducts / (depth * count) + 3;
            // Create products in a section.
            foreach (var section in sections)
                for (var i = 0; i < _rnd.Next(1, productNumber); i++)
                {
                    if (remainderOfProducts == 0) break;
                    section.Products.Add(RandomProduct());
                    remainderOfProducts -= 1;
                }

            depth -= 1;
            // Create subsections and products in subsections of this section.
            foreach (var section in sections)
                RecursiveRandomFill(depth, ref remainderOfSection, ref remainderOfProducts, section.Sections, count);
        }

        /// <summary>
        ///     Randomly select a Russian noun or create a word from Russian letters.
        /// </summary>
        private string RandomName()
        {
            if (_russianWordsCount > 0) return _russianWords[_rnd.Next(0, _russianWordsCount)];
            var result = RusFirst[_rnd.Next(0, RusFirst.Length)].ToString().ToUpper();
            for (var i = 0; i < _rnd.Next(3, 20); i++)
                result += Rus[_rnd.Next(0, Rus.Length)];
            return result;
        }

        /// <summary>
        ///     Create a product with random characteristics.
        /// </summary>
        private Product RandomProduct()
        {
            var name = RandomName();
            var code = RandomName();
            while (Codes.Contains(code))
                code = RandomName();
            Codes.Add(code);
            var price = Math.Round(_rnd.NextDouble() * _rnd.NextDouble() * _rnd.NextDouble() * _rnd.NextDouble() *
                Math.Sqrt(_rnd.Next(0, int.MaxValue)) + 0.01, 2);
            var number = (int) Math.Round(_rnd.NextDouble() * 1.2345 *
                                          Math.Sqrt(_rnd.Next(0, (int) Math.Round(Math.Sqrt(int.MaxValue)))));
            var description = "";
            // On average, every second product will have a description.
            if (_rnd.Next(1, 3) == 2)
                return new Product {Name = name, Code = code, Price = price, Number = number};
            var wordCount = _rnd.Next(2, 20);
            for (var i = 0; i < wordCount; i++)
            {
                if (i % 5 == 0 && i != 0 && i != wordCount - 1)
                    description += '\n';
                description += $"{RandomName()} ";
            }

            return new Product {Name = name, Code = code, Price = price, Number = number, Description = description};
        }

        /// <summary>
        ///     Set the visibility of the context menu object based on the current position of program elements.
        /// </summary>
        private void TreeViewContextMenu_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            EditSection.IsEnabled = SelectedSection != null;
            RemoveSection.IsEnabled = SelectedSection != null;
        }

        /// <summary>
        ///     Set the visibility of the context menu object based on the current position of program elements.
        /// </summary>
        private void DataGridContextMenu_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                AddProduct.IsEnabled = SelectedSection != null;
                if (ProductGrid.SelectedItem == null || SelectedSection == null)
                {
                    OpenPicture.IsEnabled = false;
                    RemoveProduct.IsEnabled = false;
                    EditProduct.IsEnabled = false;
                }
                else if (SelectedSection != null)
                {
                    var rowIndex = ProductGrid.SelectedIndex;
                    if (rowIndex >= SelectedSection.Products.Count)
                    {
                        OpenPicture.IsEnabled = false;
                        RemoveProduct.IsEnabled = false;
                        EditProduct.IsEnabled = false;
                    }
                    else
                    {
                        OpenPicture.IsEnabled = true;
                        RemoveProduct.IsEnabled = true;
                        EditProduct.IsEnabled = true;
                    }
                    if (rowIndex < SelectedSection.Products.Count && SelectedSection.Products[rowIndex].Image == null)
                        OpenPicture.IsEnabled = false;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}