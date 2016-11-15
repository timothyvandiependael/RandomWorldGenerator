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

        public RandomWorldGeneratorWindow GenerateMapGrid()
        {

            // Generate map grid in scrollviewer based on user parameters

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

        public void GenerateEmptyTilesAndCenterPoints()
        {
            Tiles = new Tile[CurrentWorld.Width, CurrentWorld.Height];
            CenterTiles = new Tile[CurrentWorld.Width, CurrentWorld.Height];

            Brushes = new List<Brush>();

            Brushes.Add(new SolidColorBrush(Colors.Yellow));
            Brushes.Add(new SolidColorBrush(Colors.Green));
            Brushes.Add(new SolidColorBrush(Colors.LawnGreen));
            Brushes.Add(new SolidColorBrush(Colors.Brown));
            Brushes.Add(new SolidColorBrush(Colors.Blue));
            Brushes.Add(new SolidColorBrush(Colors.White));

            UsedBrushes = new List<Brush>();

            Rng = new Random();

            for (int x = 0; x < CurrentWorld.Width; x++)
            {
                for (int y = 0; y < CurrentWorld.Height; y++)
                {
                    Tile tegel = new Tile(Colors.Black);
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

                        tegel.CenterPoint = tempKleur.ToString();
                        tegel.Kleur = ((SolidColorBrush)tempKleur).Color;

                        CenterTiles[x, y] = tegel;

                    }
                }
            }
        }

        public void FillTilesBasedOnCenterPoints()
        {
            for (int x = 0; x < CurrentWorld.Width; x++)
            {
                for (int y = 0; y < CurrentWorld.Height; y++)
                {
                    int kortsteAfstand = 0;
                    Color kortsteAfstandKleur = Colors.Black;

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

        public void AddRiversToMap()
        {

            Brush blauw = new SolidColorBrush(Colors.Blue);

            for (int x = 0; x < CurrentWorld.Width; x++)
            {
                for (int y = 0; y < CurrentWorld.Height; y++)
                {
                    if (Tiles[x, y].CenterPoint == blauw.ToString())
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
                if (Tiles[goX, goY].Kleur == Colors.Blue)
                    return;
            }

            if (Tiles[goX, goY].Kleur != Colors.Blue)
                OutOfOriginalLake = true;

            Tiles[goX, goY].Kleur = Colors.Blue;

            if (!((goDirection == 0 || goDirection == 2) && goY == grens) &&
                !((goDirection == 1 || goDirection == 3) && goX == grens))
            {
                DrawRiver(Tiles[goX, goY], sourceDirection, originalSourceDirection, goX, goY);
            }
   
        }

    }
}
