﻿using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pokemonDuel
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DatiCondivisi.Instance();
            ThreadClient c = new ThreadClient();
            Thread tc = new Thread(new ThreadStart(c.run));
            ThreadServer s = new ThreadServer();
            Thread ts = new Thread(new ThreadStart(s.run));
            DatiCondivisi.Instance().M = new classi.Logicagioco.Mappa(this);
            HashSet<int> ris=DatiCondivisi.Instance().M.Raggiungibili(DatiCondivisi.Instance().M.mappa[0], 2);
            DatiCondivisi.Instance().M.Disegna();
            foreach (int r in ris)
                Console.WriteLine(r);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DatiCondivisi.Instance().M.Disegna();
        }
    }
}
