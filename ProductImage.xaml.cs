using System.Windows.Media;

namespace _1C
{
    /// <summary>
    ///     Interaction logic for ProductImage.xaml
    /// </summary>
    public partial class ProductImage
    {
        public ProductImage()
        {
            InitializeComponent();
        }

        // The image shown in this window.
        public ImageSource PrImage
        {
            get => ProdImage.Source;
            set => ProdImage.Source = value;
        }
    }
}