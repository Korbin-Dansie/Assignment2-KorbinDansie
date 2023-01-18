using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
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

// Notes max 


namespace Assignment2_KorbinDansie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        private Random rnd = new Random();

        private int currentDieNumber = 1; // If I change the start die picture I should change this variable also
        private int guessedDieNumber = 0; // If zero then their is no guess
        private const int DIE_LOW_NUMBER = 1;
        private const int DIE_HIGH_NUMBER = 6;

        private const string DIE_IMAGE_PATH = "/Images/die"; // Relitave path to the images
        private const string DIE_IMAGE_EXTENTION = ".gif"; // Relitave path to the images

        #endregion Attributes


        #region Enum
        enum eGirdColumns
        {
            Face,
            Frequency,
            Percent,
            NumberOfTimesGuessed
        }
        #endregion Enum

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When clicked animate the roll of the dice then update the UI with the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            // Disable the UI
            setUIEnable(false);

            // Animate the die
            int nextDiceNumber;
            for (int i = 0; i < rnd.Next(2, 3); i++)
            {
                // Pick a different image for the die so its not the same (if two can't be two again)
                do
                {
                    nextDiceNumber = rnd.Next(1, 7);
                } while (currentDieNumber == nextDiceNumber);

                // Set the current die number to the one just rolled
                currentDieNumber = nextDiceNumber;

                // Update the UI with the new random Die image 
                Uri uri = new Uri(DIE_IMAGE_PATH + currentDieNumber + DIE_IMAGE_EXTENTION, UriKind.Relative);
                imageDie.Source = new BitmapImage(uri);
                await Task.Delay(300);

            }

            updateUI();

            // Enable the UI
            setUIEnable(true);

        }

        /// <summary>
        /// Disable or Enable the UI buttons
        /// </summary>
        /// <param name="enable">False = Disabled. True = Enabled</param>
        private void setUIEnable(bool enable)
        {
            btnReset.IsEnabled = enable;
            btnRoll.IsEnabled = enable;
            tbRollGuess.IsEnabled = enable;
        }

        /// <summary>
        /// Update the UI with the new results of a Die roll
        /// </summary>
        private void updateUI()
        {
            updateGameInfo();
            updategridDiceResults();
        }

        /// <summary>
        /// Updates the Game info at the top of the board
        /// </summary>
        private void updateGameInfo()
        {
            int value;

            // Add one to times played
            if (int.TryParse(tblockTimesPlayed.Text, out value))
            {
                value++;
                tblockTimesPlayed.Text = value.ToString();
            }

            // Determine if the user guess the right number
            if (guessedDieNumber == 0)
            {
                ; // Do nothing
            }
            // If correct add one to the times won
            else if (guessedDieNumber == currentDieNumber)
            {
                if (int.TryParse(tblockTimesWon.Text, out value))
                {
                    value++;
                    tblockTimesWon.Text = value.ToString();
                }
            }
            // If incorrect add one to the times lost
            else
            {
                if (int.TryParse(tblockTimesLost.Text, out value))
                {
                    value++;
                    tblockTimesLost.Text = value.ToString();
                }
            }
        }

        /// <summary>
        /// Updates the Grid with all the results at the bottom of the board
        /// </summary>
        private void updategridDiceResults()
        {
            // Get number of times played to be used in Percent calculations
            int numTimesPlayed;
            int value = 0;

            int.TryParse(tblockTimesPlayed.Text, out numTimesPlayed);

            // Convert int to spelled out version
            string[] single_digits = new string[] {
            "Zero", "One", "Two",   "Three", "Four",
            "Five", "Six", "Seven", "Eight", "Nine"
            };

            // Loop though the grid
            // Loop Columns 
            for (int col = 1; col < 4; col++)
            {
                for (int row = 1; row < 7; row++)
                {
                    switch (col)
                    {
                        case 1: // Frequency
                            // Add one to the row of the current die number
                            if (row == currentDieNumber)
                            {
                                // Children are aranged by Headers, Then Colums going down. Example (Number of Times Guessed = 3, Face 2 = 5)
                                TextBlock currentBlock = gridDiceResults.Children[(col + 1) *  5 + (row - 1)] as TextBlock;
                                int.TryParse(currentBlock.Text, out value);
                                value++;
                                currentBlock.Text = value.ToString();
                            }

                            break;
                        case 2: // Percent
                            break;
                        case 3: // Number Of Times Guessed
                            break;
                    }
                }
            }


        }

        /// <summary>
        /// Determines when the user update the guess feild
        /// If its a valid number add it to times one or lost
        /// If not a valid number does NOT count to wins or lost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRollGuess_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            if (int.TryParse(tbRollGuess.Text, out value))
            {

                // If entered a correct value update our guessed number
                if (value >= 1 && value <= 6)
                {
                    guessedDieNumber = value;
                    tbRollGuessErrorMessage.Visibility = Visibility.Hidden;
                }
                // Else they enterd a number not on the die
                else
                {
                    tbRollGuessErrorMessage.Visibility = Visibility.Visible;
                }
            }
            // If the text box is empty
            else if (tbRollGuess.Text == "")
            {
                guessedDieNumber = 0;
                tbRollGuessErrorMessage.Visibility = Visibility.Hidden;
            }
            // Else they did not enter a number
            else
            {
                tbRollGuessErrorMessage.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hide the error message when loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRollGuessErrorMessage_Initialized(object sender, EventArgs e)
        {
            tbRollGuessErrorMessage.Visibility = Visibility.Hidden;
        }
    }
}
