using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace pokemonDuel.classi.Logicagioco
{
    class Nodo
    {
        public int indice;
        public List<int> vicini;
        public Button button;
        Brush bSelezionato, bNSelezionato;
        public bool IsSelezionato { get { return button.Background == bSelezionato; } 
            set {
                if (value)
                    button.Background = bSelezionato;
                else
                    button.Background = bNSelezionato;
            } }
        public Nodo()
        {
            vicini = new List<int>();
            button = new Button();
            button.Click += Button_Click;
            bSelezionato = Brushes.LightGreen;
            bNSelezionato = Brushes.White;
        }
        public Nodo(int i)
        {
            vicini = new List<int>();
            button = new Button();
            button.Click += Button_Click;
            bSelezionato = Brushes.LightGreen;
            bNSelezionato = Brushes.White;
            indice = i;
        }
        public void AddVicino(int n)
        {
            vicini.Add(n);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DatiCondivisi.Instance();
        }
    }
}
