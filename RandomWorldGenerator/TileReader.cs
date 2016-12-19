using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace RandomWorldGenerator
{

    // Class used to read tiles from image
    public class TileReader
    {
        public Bitmap Image { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int TilesX { get; set; }
        public int TilesY { get; set; }
        public Bitmap[,] Tiles { get; set; }


        public TileReader()
        {

        }

        public TileReader(Bitmap image, int imageheight, int imagewidth, int tilesx, int tilesy)
        {
            Image = image;
            ImageHeight = imageheight;
            ImageWidth = imagewidth;
            TilesX = tilesx;
            TilesY = tilesy;
            Tiles = new Bitmap[TilesX, TilesY];
        }

        // Put the different parts of the image which contains the map tiles in a multidimensional array
        public void ListTilesFromImage()
        {

            System.Drawing.Rectangle rect;
            PixelFormat format = Image.PixelFormat;

            for (int y = 0; y < TilesY; y++)
            {
                for (int x = 0; x < TilesX; x++)
                {
                    rect = new System.Drawing.Rectangle(x * 32, y * 32, 32, 32);

                    Bitmap tile = Image.Clone(rect, format);

                    Tiles[x, y] = tile;

                }
            }
        }

        // Dispose of object
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        // Load BitmapSource from Bitmap, to be used in the UI
        public BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                    IntPtr.Zero, Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;

        }
    }
}
