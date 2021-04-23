using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace _1C
{
    [Serializable]
    public class Product
    {
        // Name of product.
        public string Name { get; set; }

        // Code of product.
        public string Code { get; set; }

        // Price of product.
        public double Price { get; set; }

        // Number of product.
        public int Number { get; set; }

        // Description of product.
        public string Description { get; set; }

        // Image of product.
        [XmlIgnore] public ImageSource Image { get; set; }

        // Serialize and deserialize an image.
        [XmlElement("Image")]
        public byte[] ImageBuffer
        {
            get
            {
                if (Image == null) return null;
                using var stream = new MemoryStream();
                var encoder = new PngBitmapEncoder(); // or some other encoder
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource) Image));
                encoder.Save(stream);
                var imageBuffer = stream.ToArray();
                return imageBuffer;
            }
            set
            {
                if (value == null)
                {
                    Image = null;
                }
                else
                {
                    using var stream = new MemoryStream(value);
                    var decoder = BitmapDecoder.Create(stream,
                        BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    Image = decoder.Frames[0];
                }
            }
        }
    }
}