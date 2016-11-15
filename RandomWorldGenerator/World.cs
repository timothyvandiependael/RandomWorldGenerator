using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomWorldGenerator
{
    public class World
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int TileSize { get; set; }
        public int BiomeSize { get; set; }

        public World(string name, int height, int width, int tilesize, int biomesize)
        {
            Name = name;
            Height = height;
            Width = width;
            TileSize = tilesize;
            BiomeSize = biomesize;
        }

    }
}
