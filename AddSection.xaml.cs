using System.Windows;

namespace _1C
{
    /// <summary>
    ///     Interaction logic for AddSection.xaml
    /// </summary>
    public partial class AddSection
    {
        public AddSection()
        {
            InitializeComponent();
        }

        // Name of new section.
        public string SectionName
        {
            get => sectionName.Text;
            set => sectionName.Text = value;
        }

        /// <summary>
        ///     Check for the correctness of the filled in fields and close dialog box.
        /// </summary>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (sectionName.Text.Length == 0)
                MessageBox.Show("Введите название");
            else DialogResult = true;
        }
    }
}