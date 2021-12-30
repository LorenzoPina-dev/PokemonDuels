using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace pokemonDuel.classi.Logicagioco
{
    public class Pokemon :ICloneable
    {
        public int id;
        public string Nome;
        public int Salti;
        public List<int> Mosse;
        private string urlTexture;
        public string UrlTexture { get { return path + urlTexture; } }
        public Nodo posizione;
        public bool mio;
        private static int idArrivato=0;
        string path = Directory.GetCurrentDirectory() + "/file/Pedine/";
        public Pokemon(string csv)
        {
            

        }
        public bool Instance(string csv)
        {

            string[] campi = csv.Split(';');
            id = int.Parse(campi[0]);
            Nome = campi[1];
            Salti = int.Parse(campi[2]);
            Mosse = new List<int>();
            for (int i = 0; i < int.Parse(campi[3]); i++)
                Mosse.Add(idArrivato++);
            urlTexture = id + ".png";
            posizione = null;
            if (!File.Exists(path + id + ".png"))
                return false;
            return true;
             //   throw new Exception("Manca img pokemon " + id);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public ImageBrush Render()
        {
            Uri uri = new Uri(path + urlTexture);
            ImageBrush myBrush = new ImageBrush(new BitmapImage(uri));
            return myBrush;
        }
    }
}
