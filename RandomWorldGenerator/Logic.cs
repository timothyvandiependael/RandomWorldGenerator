using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RandomWorldGenerator
{
    public partial class SetupWorld
    {

        public World CurrentWorld;
        public Grid GrdMap;
        public ScrollViewer ScrViewer;
        public Tile[,] Tiles;
        public Tile[,] CenterTiles;

        public List<Brush> Brushes;
        public List<Brush> UsedBrushes;

        public Random Rng;
        public int RngResult;

        public bool OutOfOriginalLake;

        private ImageBrush blauw;

        // Generate map grid in scrollviewer based on user parameters from startup window
        public RandomWorldGeneratorWindow GenerateMapGrid()
        {

            RandomWorldGeneratorWindow generatedWorld = new RandomWorldGeneratorWindow();
            CurrentWorld = new World(TxtName.Text, (int)SldHeight.Value, (int)SldWidth.Value, (int)SldTileSize.Value, (int)SldBiomeSize.Value);

            GrdMap = new Grid();
            GrdMap.SnapsToDevicePixels = true;

            Tile.Afmeting = CurrentWorld.TileSize;

            for (int i = 1; i <= CurrentWorld.Width; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(CurrentWorld.TileSize);
                GrdMap.ColumnDefinitions.Add(colDef);
            }

            for (int i = 1; i <= CurrentWorld.Height; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(CurrentWorld.TileSize);
                GrdMap.RowDefinitions.Add(rowDef);
            }

            ScrViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Content = GrdMap
            };


            generatedWorld.StpContainer.Children.Add(ScrViewer);



            ScrViewer.Height = CurrentWorld.Height*CurrentWorld.TileSize;
            ScrViewer.Width = CurrentWorld.Width*CurrentWorld.TileSize;

            if (ScrViewer.Height > 900)
                ScrViewer.Height = 900;

            if (ScrViewer.Width > 1480)
                ScrViewer.Width = 1480;

            generatedWorld.StpContainer.Height = ScrViewer.Height + 5;
            generatedWorld.StpContainer.Width = ScrViewer.Width + 5;

            generatedWorld.Height = ScrViewer.Height + 100;
            generatedWorld.Width = ScrViewer.Width + 20;

            generatedWorld.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            return generatedWorld;

        }

        // Read partial image from image file containing map tiles
        private ImageBrush GenerateMapTileImage(int x, int y)
        {
            TileReader test = new TileReader(new System.Drawing.Bitmap(@"..\..\img\tilea5.png"), 512, 256, 8, 16);
            test.ListTilesFromImage();
            ImageBrush im = new ImageBrush();
            im.ImageSource =
                test.LoadBitmap(test.Tiles[x, y]);

            return im;
        }

        // Create center points on the map which will receive a random terrain type and which will serve as centres for biomes
        public void GenerateEmptyTilesAndCenterPoints()
        {
            Tiles = new Tile[CurrentWorld.Width, CurrentWorld.Height];
            CenterTiles = new Tile[CurrentWorld.Width, CurrentWorld.Height];

            Brushes = new List<Brush>();

            blauw = GenerateMapTileImage(7, 2);

            Brushes.Add(GenerateMapTileImage(0,6));
            Brushes.Add(GenerateMapTileImage(4,6));
            Brushes.Add(GenerateMapTileImage(6,10));
            Brushes.Add(GenerateMapTileImage(5,2));
            Brushes.Add(blauw);
            Brushes.Add(GenerateMapTileImage(3,6));


            UsedBrushes = new List<Brush>();

            Rng = new Random();

            for (int x = 0; x < CurrentWorld.Width; x++)
            {
                for (int y = 0; y < CurrentWorld.Height; y++)
                {
                    Tile tegel = new Tile(new SolidColorBrush(Colors.Black));
                    Tiles[x, y] = tegel;

                    RngResult = Rng.Next(0, CurrentWorld.BiomeSize + 1);

                    if (RngResult == CurrentWorld.BiomeSize)
                    {
                        if (Brushes.Count == 0)
                        {
                            Brushes.AddRange(UsedBrushes);
                            UsedBrushes.Clear();
                        }

                        int welkeKleur = Rng.Next(0, Brushes.Count);

                        Brush tempKleur = Brushes[welkeKleur];

                        UsedBrushes.Add(tempKleur);
                        Brushes.RemoveAt(welkeKleur);

                        tegel.CenterPoint = tempKleur;
                        tegel.Kleur = tempKleur;

                        CenterTiles[x, y] = tegel;

                    }
                }
            }
        }

        // Tiles will receive the terrain type of the closest center point
        public void FillTilesBasedOnCenterPoints()
        {
            for (int x = 0; x < CurrentWorld.Width; x++)
            {
                for (int y = 0; y < CurrentWorld.Height; y++)
                {
                    int kortsteAfstand = 0;
                    Brush kortsteAfstandKleur = new SolidColorBrush(Colors.Black);

                    for (int xc = 0; xc < CurrentWorld.Width; xc++)
                    {
                        for (int yc = 0; yc < CurrentWorld.Height; yc++)
                        {
                            if (CenterTiles[xc, yc] != null)
                            {
                                if (x == xc && y == yc)
                                {
                                    Tiles[x, y].Kleur = CenterTiles[xc, yc].Kleur;
                                }
                                else if (x == xc || Math.Abs(y - yc) > Math.Abs(x - xc))
                                {
                                    if (kortsteAfstand == 0)
                                    {
                                        kortsteAfstand = Math.Abs(y - yc);
                                        kortsteAfstandKleur = CenterTiles[xc, yc].Kleur;
                                    }
                                    else
                                    {
                                        if (Math.Abs(y - yc) < kortsteAfstand)
                                        {
                                            kortsteAfstand = Math.Abs(y - yc);
                                            kortsteAfstandKleur = CenterTiles[xc, yc].Kleur;
                                        }
                                    }
                                }
                                else
                                {
                                    if (kortsteAfstand == 0)
                                    {
                                        kortsteAfstand = Math.Abs(x - xc);
                                        kortsteAfstandKleur = CenterTiles[xc, yc].Kleur;
                                    }
                                    else
                                    {
                                        if (Math.Abs(x - xc) < kortsteAfstand)
                                        {
                                            kortsteAfstand = Math.Abs(x - xc);
                                            kortsteAfstandKleur = CenterTiles[xc, yc].Kleur;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Tiles[x, y].Kleur = kortsteAfstandKleur;

                    Grid.SetColumn(Tiles[x,y].Inhoud, x);
                    Grid.SetRow(Tiles[x,y].Inhoud, y);
                    GrdMap.Children.Add(Tiles[x, y].Inhoud);
                }
            }
        }

        // Add random rivers to the map
        public void AddRiversToMap()
        {

            for (int x = 0; x < CurrentWorld.Width; x++)
            {
                for (int y = 0; y < CurrentWorld.Height; y++)
                {
                    if (Equals(((ImageBrush)Tiles[x, y].CenterPoint).ImageSource, blauw.ImageSource))
                    {
                        RngResult = Rng.Next(0, 1);

                        if (RngResult == 0)
                        {
                            RngResult = Rng.Next(0, 4);
                            OutOfOriginalLake = false;
                            DrawRiver(Tiles[x, y], RngResult, RngResult, x, y);
                        }
                    }
                }
            } 
        }

        // Draw one river which will flow in random directions
        public void DrawRiver(Tile startTile, int sourceDirection, int originalSourceDirection, int x, int y)
        {

            int goDirection = -1;
            int grens = -2;
            while (goDirection == -1 || goDirection == sourceDirection || goDirection == originalSourceDirection)
            {
                goDirection = Rng.Next(0, 4);
            }

            int goX = x, goY = y;

            switch (goDirection)
            {
                case 0:
                    grens = 0;
                    if (goY != grens)
                        goY -= 1;
                    sourceDirection = 2;
                    break;
                case 1:
                    grens = CurrentWorld.Width - 1;
                    if (goX != grens)
                        goX += 1;
                    sourceDirection = 3;
                    break;
                case 2:
                    grens = CurrentWorld.Height - 1;
                    if (goY != grens)
                        goY += 1;
                    sourceDirection = 0;
                    break;
                case 3:
                    grens = 0;
                    if (goX != grens)
                        goX -= 1;
                    sourceDirection = 1;
                    break;
            }

            if (OutOfOriginalLake)
            {
                if (Equals(((ImageBrush)Tiles[goX, goY].Kleur).ImageSource, blauw.ImageSource))
                    return;
            }

            if (Equals(((ImageBrush)Tiles[goX, goY].Kleur).ImageSource, blauw.ImageSource))
                OutOfOriginalLake = true;

            Tiles[goX, goY].Kleur = blauw;

            if (!((goDirection == 0 || goDirection == 2) && goY == grens) &&
                !((goDirection == 1 || goDirection == 3) && goX == grens))
            {
                DrawRiver(Tiles[goX, goY], sourceDirection, originalSourceDirection, goX, goY);
            }
   
        }

    }
}
