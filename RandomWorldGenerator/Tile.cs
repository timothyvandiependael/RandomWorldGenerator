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

        private Color _kleur;
        public Color Kleur
        {
            get { return _kleur; }

            set
            {
                Inhoud.Fill = new SolidColorBrush(value);
                _kleur = value;
            }
        }
        public string CenterPoint { get; set; }
        public bool OnMap { get; set; }

        public Tile(Color kleur)
        {
            Inhoud = new Rectangle();
            Inhoud.Width = Afmeting;
            Inhoud.Height = Afmeting;
            Kleur = kleur;
            CenterPoint = "";
            OnMap = true;
        }

        public Tile(Color kleur, bool onboard)
        {
            Inhoud = new Rectangle();
            Inhoud.Width = Afmeting;
            Inhoud.Height = Afmeting;
            Kleur = kleur;
            CenterPoint = "";
            OnMap = onboard;
        }
    }
}
