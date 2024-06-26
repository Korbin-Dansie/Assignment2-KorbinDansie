﻿/*
 * Author : Korbin Dansie
 * Created On : 1/16/2023
 * Last Modified On : 
 * Description : A die rolling game. If the user inputs nothing or incorrect value for the guess then SHOULD NOT roll
*/

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace Assignment2_KorbinDansie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributes
        /// <summary>
        /// Variable for randomness
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Current Die displayed on the screen
        /// </summary>
        private int currentDieNumber = 1; // If I change the start die picture I should change this variable also
        /// <summary>
        /// Current number that the user has guessed
        /// </summary>
        private int guessedDieNumber = 0; // If zero then their is no guess

        /// <summary>
        /// Lowest number on the die
        /// </summary>
        private const int DIE_LOW_NUMBER = 1;
        /// <summary>
        /// Highest number on the die
        /// </summary>
        private const int DIE_HIGH_NUMBER = 6;

        /// <summary>
        /// Start of the relative path to find images plus name
        /// </summary>
        private const string DIE_IMAGE_PATH = "/Images/die"; // Relitave path to the images
        /// <summary>
        /// Ending file extention for die images
        /// </summary>
        private const string DIE_IMAGE_EXTENTION = ".gif"; // Relitave path to the images

        /// <summary>
        /// Number of rows in grid counting header row
        /// </summary>
        private const int NUMBER_OF_GRID_ROWS = 7;
        /// <summary>
        /// Number of columns in the gird
        /// </summary>
        private const int NUMBER_OF_GRID_COLUMNS = 4;

        #endregion Attributes

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
            //Check if user has inputed a valid number if not display an error
            if (!checkUserInput())
            {
                return;
            }

            // Disable the UI
            setUIEnable(false);

            // Animate the die
            int nextDiceNumber;
            for (int i = 0; i < rnd.Next(4, 7) /*How many animated die rolls*/; i++)
            {
                // Pick a different image for the die so its not the same (if two can't be two again)
                do
                {
                    nextDiceNumber = rnd.Next(DIE_LOW_NUMBER, DIE_HIGH_NUMBER + 1);
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
            updateGridDiceResults(); // Needs to come after updateGameInfo so Total times played is the correct number.
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
        private void updateGridDiceResults()
        {
            // Get number of times played to be used in Percent calculations
            int numTimesPlayed;
            int value = 0;

            int.TryParse(tblockTimesPlayed.Text, out numTimesPlayed);

            // Create an Textblock element to hold the child of the grid
            TextBlock currentBlock;

            // Loop though the grid
            for (int row = 1; row < NUMBER_OF_GRID_ROWS; row++)
            {
                for (int col = 1; col < NUMBER_OF_GRID_COLUMNS; col++)
                {

                    // Update the Textblocks info
                    switch (col)
                    {
                        case 1: // Frequency

                            // Add one to the row of the current die number
                            if (row == currentDieNumber)
                            {
                                // Get the Textblock in the grid
                                currentBlock = getGridElementAt(row, col);

                                int.TryParse(currentBlock.Text, out value);
                                value++;
                                currentBlock.Text = value.ToString();
                            }
                            break;

                        case 2: // Percent
                            // Get the Textblock in the grid in col frequency
                            currentBlock = getGridElementAt(row, 1);
                            int.TryParse(currentBlock.Text, out value);

                            // Get percent by frequency / totalTimesPlayed
                            double percent = (double)value / numTimesPlayed * 100;

                            // Set it to percent col
                            currentBlock = getGridElementAt(row, col);
                            currentBlock.Text = String.Format("{0:0.00}%", percent);
                            break;

                        case 3: // Number Of Times Guessed
                            if (row == guessedDieNumber)
                            {
                                // Get the Textblock in the grid
                                currentBlock = getGridElementAt(row, col);

                                int.TryParse(currentBlock.Text, out value);
                                value++;
                                currentBlock.Text = value.ToString();
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Get the element in the grid at a certain row and col
        /// </summary>
        /// <param name="row">Which row of the body to return 1 - 6</param>
        /// <param name="col">Which colum to return 1 - 4</param>
        /// <returns></returns>
        TextBlock getGridElementAt(int row, int column)
        {
            // Children are aranged by Headers, Then Colums going down. Example (Number of Times Guessed = 3, Face 2 = 5)
            // Col for the correct colum, NUMBER_OF_GRID_ROWS - the header row, row for the correct row, NUMBER_OF_GRID_COLUMNS - 1 for number of headers

            // Cannot update the header rows. Expect of the furthest one on the right which is (0,0)

            /* Example: (Row, Col)
             *  Face    Frequency
             *  (1,0)   (1,1)
             *  (2,0)   (2,1)
             *  (3,0)   (3,1)
             *  (4,0)   (4,1)
             *  (5,0)   (5,1)
             *  (6,0)   (6,1)
             */
            return gridDiceResults.Children[(column * (NUMBER_OF_GRID_ROWS)) + row] as TextBlock;
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
            // If user deletes the a valid number dont display an error message
            if(tbRollGuess.Text == String.Empty)
            {
                return;
            }
            checkUserInput();
        }

        /// <summary>
        /// Clear text box when in focus. If tab was pressed only select the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRollGuess_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.Tab))
            {
                tbRollGuess.Text = String.Empty;
            }

            tbRollGuess.SelectAll();
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

        /// <summary>
        /// Check user input from tbRollGuess. If invalid display error message
        /// </summary>
        /// <returns>If valid return true</returns>
        private bool checkUserInput()
        {
            int value;
            if (int.TryParse(tbRollGuess.Text, out value))
            {

                // If entered a correct value update our guessed number
                if (value >= DIE_LOW_NUMBER && value <= DIE_HIGH_NUMBER)
                {
                    guessedDieNumber = value;
                    tbRollGuessErrorMessage.Visibility = Visibility.Hidden;
                    return true;
                }
            }

            tbRollGuessErrorMessage.Visibility = Visibility.Visible;
            guessedDieNumber = 0;
            return false;
        }



        /// <summary>
        /// On click reset the board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetBoard();
        }

        /// <summary>
        /// Reset the board
        /// </summary>
        private void resetBoard()
        {
            resetGameInfo();
            resetDie();
            resetGrid();
        }

        /// <summary>
        /// Reset the gameInfo top section
        /// </summary>
        private void resetGameInfo()
        {
            tblockTimesPlayed.Text = "0";
            tblockTimesWon.Text = "0";
            tblockTimesLost.Text = "0";
        }

        /// <summary>
        /// Reset the die image and input
        /// </summary>
        private void resetDie()
        {
            tbRollGuess.Text = "";
            tbRollGuessErrorMessage.Visibility = Visibility.Hidden;

            currentDieNumber = 1;
            guessedDieNumber = 0;

            Uri uri = new Uri(DIE_IMAGE_PATH + currentDieNumber + DIE_IMAGE_EXTENTION, UriKind.Relative);
            imageDie.Source = new BitmapImage(uri);
        }

        /// <summary>
        /// Reset the grid info
        /// </summary>
        private void resetGrid()
        {
            int value = 0;

            // Create an Textblock element to hold the child of the grid
            TextBlock currentBlock;

            for (int row = 1; row < NUMBER_OF_GRID_ROWS; row++)
            {
                for (int col = 1; col < NUMBER_OF_GRID_COLUMNS; col++)
                {
                    switch (col)
                    {
                        case 1: // Frequency
                        case 3: // Number Of Times Guessed
                            currentBlock = getGridElementAt(row, col);
                            currentBlock.Text = value.ToString();
                            break;

                        case 2: // Percent
                            currentBlock = getGridElementAt(row, col);
                            currentBlock.Text = String.Format("{0:0.00}%", 0);
                            break;
                    }
                }
            }
        }
    }
}
