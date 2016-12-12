using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RandomWorldGenerator
{
    public class Tile
    {
        public static int Afmeting { get; set; }
        public Rectangle Inhoud { get; set; }

        private Brush _kleur;
        public Brush Kleur
        {
            get { return _kleur; }

            set
            {
                Inhoud.Fill = value;
                _kleur = value;
            }
        }
        public Brush CenterPoint { get; set; }
        public bool OnMap { get; set; }

        public Tile(SolidColorBrush kleur)
        {
            Inhoud = new Rectangle();
            Inhoud.Width = Afmeting;
            Inhoud.Height = Afmeting;
            Kleur = kleur;
            CenterPoint = new ImageBrush();
            OnMap = true;
        }

        public Tile(ImageBrush brush)
        {
            Inhoud = new Rectangle();
            Inhoud.Width = Afmeting;
            Inhoud.Height = Afmeting;
            Kleur = brush;
            CenterPoint = new ImageBrush();
            OnMap = true;
        }

        public Tile(SolidColorBrush kleur, bool onboard)
        {
            Inhoud = new Rectangle();
            Inhoud.Width = Afmeting;
            Inhoud.Height = Afmeting;
            Kleur = kleur;
            CenterPoint = new ImageBrush();
            OnMap = onboard;
        }

        public Tile(ImageBrush brush, bool onboard)
        {
            Inhoud = new Rectangle();
            Inhoud.Width = Afmeting;
            Inhoud.Height = Afmeting;
            Kleur = brush;
            CenterPoint = new ImageBrush();
            OnMap = onboard;
        }
    }
}
