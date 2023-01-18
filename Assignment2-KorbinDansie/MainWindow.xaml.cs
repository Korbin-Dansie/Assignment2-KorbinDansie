using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        private async void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            for (int i = 0; i < 3; i++)
            {

                imageDie.Source = new BitmapImage(new Uri($"/Images/die{rnd.Next(1, 7)}.gif", UriKind.Relative));
                await Task.Delay(300);
                //Thread.Sleep(300);

            }
        }


        private void UIRollDice(object sender, RoutedEventArgs e)
        {

        }
    }
}
