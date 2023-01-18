using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
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

namespace Assignment2_KorbinDansie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When clicked roll the dice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            for (int i = 0; i < 7; i++)
            {
              

                //Uri uri= new Uri($"file:///{Environment.CurrentDirectory}/Images/die2.gif", UriKind.RelativeOrAbsolute);
                Uri uri= new Uri($"/Images/die{rnd.Next(1, 7)}.gif", UriKind.Relative);

                BitmapImage bitmap = new BitmapImage(uri);
                
                imageDie.Source = bitmap;
                //BitmapSource bitmapSource = new BitmapSource(bitmap);
                Thread.Sleep(300);
            }
        }
    }
}
