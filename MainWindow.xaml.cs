/*
 *Josh Degazio
 *April 17th, 2018
 *Hangman Supreme
 *User attempts to guess the word which was randomly chosen by the computer, in a limited amount of guesses.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using System.Net;

namespace U3_Josh_SummativeHangman
{
    //Global Variables
    public static class Globals
    {
        public static int LettersSet = 5;
        public static int CurrentLines = 0;
        public static int DrawnLines = 0;
        public static int GuessesRemaining = 9;
        public static int PointsEarned;
        public static int SavedWins;
        public static string LettersText = "5";
        public static string PlayLetters = "5";
        public static string PlaybtnText = "Play (Word Length: ";
        public static string ReadWords;
        public static string Threeltr_Word;
        public static string Fourltr_Word;
        public static string Fiveltr_Word;
        public static string Sixltr_Word;
        public static Line[] myline = new Line[6];
        public static Random rnd = new Random();
    }
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            //Sets the text inside button "play" to equal the normal text + how many letters were selected.
            btn_Play.Content = Globals.PlaybtnText + Globals.PlayLetters + ")";

            //Runs method "CheckFiles"
            CheckFiles();
            //Runs method "RefreshWins"
            RefreshWins();
        }

        //Method involving checking that all files required to run the game can be located.
        private static void CheckFiles()
        {

            //If files can't be located, the program downloads them from my github repository
            if (!File.Exists("3ltr_Words.txt"))
            {
                WebClient client = new WebClient();

                //Sets stream data to be equal to that of the website text file data.
                Stream ThreeStream = client.OpenRead("https://raw.githubusercontent.com/JoshDegazio/U3_Josh_HangmanSummative/master/3ltr_Words.txt");
                using (StreamWriter ThreeWriter = new StreamWriter("3ltr_Words.txt"))
                {
                    using (StreamReader ThreeReader = new StreamReader(ThreeStream))
                    {
                        String ThreeWords = ThreeReader.ReadToEnd();

                        ThreeWriter.Write(ThreeWords);
                        ThreeWriter.Flush();
                        ThreeWriter.Close();
                    }
                }

                Stream FourStream = client.OpenRead("https://raw.githubusercontent.com/JoshDegazio/U3_Josh_HangmanSummative/master/4ltr_Words.txt");
                using (StreamWriter FourWriter = new StreamWriter("4ltr_Words.txt"))
                {
                    using (StreamReader FourReader = new StreamReader(FourStream))
                    {
                        String FourWords = FourReader.ReadToEnd();

                        FourWriter.Write(FourWords);
                        FourWriter.Flush();
                        FourWriter.Close();
                    }
                }

                Stream FiveStream = client.OpenRead("https://raw.githubusercontent.com/JoshDegazio/U3_Josh_HangmanSummative/master/5ltr_Words.txt");
                using (StreamWriter FiveWriter = new StreamWriter("5ltr_Words.txt"))
                {
                    using (StreamReader FiveReader = new StreamReader(FiveStream))
                    {
                        String FiveWords = FiveReader.ReadToEnd();

                        FiveWriter.Write(FiveWords);
                        FiveWriter.Flush();
                        FiveWriter.Close();
                    }
                }

                Stream SixStream = client.OpenRead("https://raw.githubusercontent.com/JoshDegazio/U3_Josh_HangmanSummative/master/6ltr_Words.txt");
                using (StreamWriter SixWriter = new StreamWriter("6ltr_Words.txt"))
                {
                    using (StreamReader SixReader = new StreamReader(SixStream))
                    {
                        String SixWords = SixReader.ReadToEnd();

                        SixWriter.Write(SixWords);
                        SixWriter.Flush();
                        SixWriter.Close();
                    }
                }

                Stream WinStream = client.OpenRead("https://raw.githubusercontent.com/JoshDegazio/U3_Josh_HangmanSummative/master/wins.txt");
                using (StreamWriter WinWriter = new StreamWriter("wins.txt"))
                {
                    using (StreamReader WinReader = new StreamReader(WinStream))
                    {
                        String WinNumber = WinReader.ReadToEnd();

                        WinWriter.Write(WinNumber);
                        WinWriter.Flush();
                        WinWriter.Close();
                    }
                }
            }
        }

        private void click_SetLetters(object Sender, RoutedEventArgs e)
        {
            //Creates integer i
            int i;
            //Sets value of integer i to be equal to what was entered in the set letters textbox
            int.TryParse(inpt_Letters.Text, out i);
            //Checks if the value that was entered (Which is now "i") is above or equal to 3 or below or equal to 6
            if (i >= 3 && i <= 6)
            {
                SetLetterVariables();
            }
            else
            {
                MessageBox.Show("You may NOT enter a number larger than 6 or less than 3.\nPlease try again");
            }
        }

        private void SetLetterVariables()
        {
            Globals.LettersText = inpt_Letters.Text;
            int.TryParse(Globals.LettersText, out Globals.LettersSet);
            Globals.PlayLetters = Globals.LettersText;
            btn_Play.Content = Globals.PlaybtnText + Globals.PlayLetters + ")";
        }

        private void click_Play(object Sender, RoutedEventArgs e)
        {
            pg_Title.Visibility = Visibility.Hidden;
            pg_Play.Visibility = Visibility.Visible;

            ResetGame();
            CreateLines();

            StreamWritingAndReading();

            DrawLetters();

            for (int i = 0; i < Globals.LettersSet; i++)
            {
                can_Word.Children.Add(Globals.myline[i]);
            }
        }

        private void DrawLetters()
        {
            if (Globals.LettersSet == 3)
            {
                Letter_1.Text = Globals.Threeltr_Word.Substring(0, 1).ToUpper();
                Letter_1.Visibility = Visibility.Hidden;

                Letter_2.Text = Globals.Threeltr_Word.Substring(1, 1).ToUpper();
                Letter_2.Visibility = Visibility.Hidden;

                Letter_3.Text = Globals.Threeltr_Word.Substring(2, 1).ToUpper();
                Letter_3.Visibility = Visibility.Hidden;
            }
            else if (Globals.LettersSet == 4)
            {
                Letter_1.Text = Globals.Fourltr_Word.Substring(0, 1).ToUpper();
                Letter_1.Visibility = Visibility.Hidden;

                Letter_2.Text = Globals.Fourltr_Word.Substring(1, 1).ToUpper();
                Letter_2.Visibility = Visibility.Hidden;

                Letter_3.Text = Globals.Fourltr_Word.Substring(2, 1).ToUpper();
                Letter_3.Visibility = Visibility.Hidden;

                Letter_4.Text = Globals.Fourltr_Word.Substring(3, 1).ToUpper();
                Letter_4.Visibility = Visibility.Hidden;

            }
            else if (Globals.LettersSet == 5)
            {
                Letter_1.Text = Globals.Fiveltr_Word.Substring(0, 1).ToUpper();
                Letter_1.Visibility = Visibility.Hidden;

                Letter_2.Text = Globals.Fiveltr_Word.Substring(1, 1).ToUpper();
                Letter_2.Visibility = Visibility.Hidden;

                Letter_3.Text = Globals.Fiveltr_Word.Substring(2, 1).ToUpper();
                Letter_3.Visibility = Visibility.Hidden;

                Letter_4.Text = Globals.Fiveltr_Word.Substring(3, 1).ToUpper();
                Letter_4.Visibility = Visibility.Hidden;

                Letter_5.Text = Globals.Fiveltr_Word.Substring(4, 1).ToUpper();
                Letter_5.Visibility = Visibility.Hidden;

            }
            else if (Globals.LettersSet == 6)
            {
                Letter_1.Text = Globals.Sixltr_Word.Substring(0, 1).ToUpper();
                Letter_1.Visibility = Visibility.Hidden;

                Letter_2.Text = Globals.Sixltr_Word.Substring(1, 1).ToUpper();
                Letter_2.Visibility = Visibility.Hidden;

                Letter_3.Text = Globals.Sixltr_Word.Substring(2, 1).ToUpper();
                Letter_3.Visibility = Visibility.Hidden;

                Letter_4.Text = Globals.Sixltr_Word.Substring(3, 1).ToUpper();
                Letter_4.Visibility = Visibility.Hidden;

                Letter_5.Text = Globals.Sixltr_Word.Substring(4, 1).ToUpper();
                Letter_5.Visibility = Visibility.Hidden;

                Letter_6.Text = Globals.Sixltr_Word.Substring(5, 1).ToUpper();
                Letter_6.Visibility = Visibility.Hidden;

            }
            else { MessageBox.Show("Whoops, Something went wrong, please close the program and try again."); }
        }

        private static void StreamWritingAndReading()
        {
            if (Globals.LettersSet == 3)
            {
                {
                    int threernd = Globals.rnd.Next(0, 500);
                    string str_threernd = threernd.ToString() + ' ';
                    using (StreamReader ThreeWord_Read = new StreamReader("3ltr_Words.txt"))
                    {
                        while (true)
                        {
                            Globals.ReadWords = ThreeWord_Read.ReadLine();
                            if (Globals.ReadWords.Contains(str_threernd))
                            {
                                Console.WriteLine(Globals.ReadWords);
                                Globals.Threeltr_Word = Globals.ReadWords.Replace(str_threernd, "");
                                Console.WriteLine(Globals.Threeltr_Word);
                                break;
                            }
                        }
                    }
                }
            }
            else if (Globals.LettersSet == 4)
            {
                int fourrnd = Globals.rnd.Next(0, 501);
                string str_fourrnd = fourrnd.ToString() + ' ';
                using (StreamReader FourWord_Read = new StreamReader("4ltr_Words.txt"))
                {
                    while (true)
                    {
                        Globals.ReadWords = FourWord_Read.ReadLine();
                        if (Globals.ReadWords.Contains(str_fourrnd))
                        {
                            Console.WriteLine(Globals.ReadWords);
                            Globals.Fourltr_Word = Globals.ReadWords.Replace(str_fourrnd, "");
                            Console.WriteLine(Globals.Fourltr_Word);
                            break;
                        }
                    }
                }
            }
            else if (Globals.LettersSet == 5)
            {
                int fivernd = Globals.rnd.Next(0, 5758);
                string str_fivernd = fivernd.ToString() + ' ';
                using (StreamReader FiveWord_Read = new StreamReader("5ltr_Words.txt"))
                {
                    while (true)
                    {
                        Globals.ReadWords = FiveWord_Read.ReadLine();
                        if (Globals.ReadWords.Contains(str_fivernd))
                        {
                            Console.WriteLine(Globals.ReadWords);
                            Globals.Fiveltr_Word = Globals.ReadWords.Replace(str_fivernd, "");
                            Console.WriteLine(Globals.Fiveltr_Word);
                            break;
                        }
                    }
                }
            }
            else if (Globals.LettersSet == 6)
            {
                int sixrnd = Globals.rnd.Next(0, 1648);
                string str_sixrnd = sixrnd.ToString() + '.' + ' ';
                using (StreamReader SixWord_Read = new StreamReader("6ltr_Words.txt"))
                {
                    while (true)
                    {
                        Globals.ReadWords = SixWord_Read.ReadLine();
                        if (Globals.ReadWords.Contains(str_sixrnd))
                        {
                            Console.WriteLine(Globals.ReadWords);
                            Globals.Sixltr_Word = Globals.ReadWords.Replace(str_sixrnd, "");
                            Console.WriteLine(Globals.Sixltr_Word);
                            break;
                        }
                    }
                }
            }
        }

        private void ResetGame()
        {
            //Reset Lines
            if (Globals.LettersSet < Globals.CurrentLines)
            {
                for (int i = 0; i < Globals.CurrentLines; i++)
                {
                    can_Word.Children.Remove(Globals.myline[i]);
                    Console.WriteLine("Line " + (i + 1) + " removed successfully!");
                }
                Globals.CurrentLines = 0;
            }
            else
            {
                Globals.CurrentLines = 0;
            }

            //Reset Letters
            Letter_1.Text = "";
            Letter_1.Visibility = Visibility.Hidden;

            Letter_2.Text = "";
            Letter_2.Visibility = Visibility.Hidden;

            Letter_3.Text = "";
            Letter_3.Visibility = Visibility.Hidden;

            Letter_4.Text = "";
            Letter_4.Visibility = Visibility.Hidden;

            Letter_5.Text = "";
            Letter_5.Visibility = Visibility.Hidden;

            Letter_6.Text = "";
            Letter_6.Visibility = Visibility.Hidden;


            //Reset Variables
            Globals.ReadWords = "";
            Globals.GuessesRemaining = 9;
            Globals.PointsEarned = 0;

            //Reset Inputs and Outputs
            inpt_GuessLtr.Text = "Input Your Guess Here (Ex: A)";
            txt_ErrorOutput.Text = "Errors in your guess \nwill appear here.";
            txt_GuessOutput.Text = "If you guess a \nletter correctly or \nhave previously guessed \nthat letter it \nwill appear here.";

            //Resets Man
            man_Head.Visibility = Visibility.Hidden;
            man_Body.Visibility = Visibility.Hidden;
            man_Arm1.Visibility = Visibility.Hidden;
            man_Arm2.Visibility = Visibility.Hidden;
            man_Eye1.Visibility = Visibility.Hidden;
            man_Eye2.Visibility = Visibility.Hidden;
            man_Leg1.Visibility = Visibility.Hidden;
            man_Leg2.Visibility = Visibility.Hidden;
            man_Mouth.Visibility = Visibility.Hidden;
        }

        private static void CreateLines()
        {
            for (int i = 0; i <= Globals.LettersSet-1; i++)
            {
                Globals.myline[i] = new Line();
                Globals.myline[i].X1 = 5 + (50 * i);
                Globals.myline[i].X2 = 50 + (50 * i);
                Globals.myline[i].Y1 = 50;
                Globals.myline[i].Y2 = 50;
                Globals.myline[i].Stroke = Brushes.Black;
                Globals.myline[i].StrokeThickness = 3;
                Globals.CurrentLines++;
                Console.WriteLine("Line " + (i + 1) + " was successfully created!");

            }
        }

        private void click_Exit(object Sender, RoutedEventArgs e)
        {
            pg_Title.Visibility = Visibility.Visible;
            pg_Play.Visibility = Visibility.Hidden;
        }

        private void click_Guess(object Sender, RoutedEventArgs e)
        {
            CheckGuess();
            CheckIfWin();

        }

        private void CheckGuess()
        {
            string str_Guess = inpt_GuessLtr.Text;

            if (str_Guess == str_Guess.ToUpper())
            {
                if (str_Guess.Length == 1)
                {
                    if (str_Guess.Any(char.IsLetter))
                    {
                        GuessWasRight();
                        GuessWasWrong();
                    }
                    else if (str_Guess.Any(char.IsDigit))
                    {
                        txt_ErrorOutput.Text = "The guess you have\nentered has a \ndigit in it. \nPlease try again.";
                        txt_GuessOutput.Text = "";
                    }
                }
                else if (str_Guess.Length > 1)
                {
                    txt_ErrorOutput.Text = "The guess you entered \nis larger than \none character. \nPlease try again.";
                    txt_GuessOutput.Text = "";
                }
                else if (str_Guess.Length < 1)
                {
                    txt_ErrorOutput.Text = "You did not \nenter a guess. \nPlease try again.";
                    txt_GuessOutput.Text = "";
                }
            }
            else
            {
                txt_ErrorOutput.Text = "You need to enter your \nguess as an UPPERCASE. \nPlease try again.";
                txt_GuessOutput.Text = "";
            }
        }

        private void GuessWasRight()
        {
            if (inpt_GuessLtr.Text == Letter_1.Text)
            {
                if (Letter_1.Visibility == Visibility.Hidden)
                {
                    Letter_1.Visibility = Visibility.Visible;
                    txt_GuessOutput.Text = "You guessed correctly! \nYou still have \n" + (Globals.GuessesRemaining) + " guesses remaining.";
                    txt_ErrorOutput.Text = "";
                    Globals.PointsEarned++;
                }
                else { txt_GuessOutput.Text = "You have already entered \nthis letter, please \nenter a different letter. "; }

            }
            if (inpt_GuessLtr.Text == Letter_2.Text)
            {
                if (Letter_2.Visibility == Visibility.Hidden)
                {
                    Letter_2.Visibility = Visibility.Visible;
                    txt_GuessOutput.Text = "You guessed correctly! \nYou still have \n" + (Globals.GuessesRemaining) + " guesses remaining.";
                    txt_ErrorOutput.Text = "";
                    Globals.PointsEarned++;
                }
                else { txt_GuessOutput.Text = "You have already entered \nthis letter, please \nenter a different letter. "; }
            }
            if (inpt_GuessLtr.Text == Letter_3.Text)
            {
                if (Letter_3.Visibility == Visibility.Hidden)
                {
                    Letter_3.Visibility = Visibility.Visible;
                    txt_GuessOutput.Text = "You guessed correctly! \nYou still have \n" + (Globals.GuessesRemaining) + " guesses remaining.";
                    txt_ErrorOutput.Text = "";
                    Globals.PointsEarned++;
                }
                else { txt_GuessOutput.Text = "You have already entered \nthis letter, please \nenter a different letter. "; }
            }
            if (inpt_GuessLtr.Text == Letter_4.Text)
            {
                if (Letter_4.Visibility == Visibility.Hidden)
                {
                    Letter_4.Visibility = Visibility.Visible;
                    txt_GuessOutput.Text = "You guessed correctly! \nYou still have \n" + (Globals.GuessesRemaining) + " guesses remaining."; ;
                    txt_ErrorOutput.Text = "";
                    Globals.PointsEarned++;
                }
                else { txt_GuessOutput.Text = "You have already entered \nthis letter, please \nenter a different letter. "; }

            }
            if (inpt_GuessLtr.Text == Letter_5.Text)
            {
                if (Letter_5.Visibility == Visibility.Hidden)
                {
                    Letter_5.Visibility = Visibility.Visible;
                    txt_GuessOutput.Text = "You guessed correctly! \nYou still have \n" + (Globals.GuessesRemaining) + " guesses remaining."; ;
                    txt_ErrorOutput.Text = "";
                    Globals.PointsEarned++;
                }
                else { txt_GuessOutput.Text = "You have already entered \nthis letter, please \nenter a different letter. "; }

            }
            if (inpt_GuessLtr.Text == Letter_6.Text)
            {
                if (Letter_6.Visibility == Visibility.Hidden)
                {
                    Letter_6.Visibility = Visibility.Visible;
                    txt_GuessOutput.Text = "You guessed correctly! \nYou still have \n" + (Globals.GuessesRemaining) + " guesses remaining."; ;
                    txt_ErrorOutput.Text = "";
                    Globals.PointsEarned++;
                }
                else { txt_GuessOutput.Text = "You have already entered \nthis letter, please \nenter a different letter. "; }

            }
        }

        private void GuessWasWrong()
        {
            if (inpt_GuessLtr.Text != Letter_1.Text &&
                                        inpt_GuessLtr.Text != Letter_2.Text &&
                                        inpt_GuessLtr.Text != Letter_3.Text &&
                                        inpt_GuessLtr.Text != Letter_4.Text &&
                                        inpt_GuessLtr.Text != Letter_5.Text &&
                                        inpt_GuessLtr.Text != Letter_6.Text)
            {
                if (Globals.GuessesRemaining != 0)
                {
                    txt_GuessOutput.Text = "Your guess was incorrect! \nYou only have \n" + (Globals.GuessesRemaining - 1) + " guesses left.";

                    if (Globals.GuessesRemaining == 9)
                    {
                        man_Head.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 8)
                    {
                        man_Body.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 7)
                    {
                        man_Arm1.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 6)
                    {
                        man_Arm2.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 5)
                    {
                        man_Leg1.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 4)
                    {
                        man_Leg2.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 3)
                    {
                        man_Eye1.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 2)
                    {
                        man_Eye2.Visibility = Visibility.Visible;
                    }
                    else if (Globals.GuessesRemaining == 1)
                    {
                        man_Mouth.Visibility = Visibility.Visible;
                        
                        if (Globals.Sixltr_Word.Length == 6)
                        {
                            MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Sixltr_Word);
                        }
                        else if (Globals.Fiveltr_Word.Length == 5)
                        {
                            MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Fiveltr_Word);
                        }
                        else if (Globals.Fourltr_Word.Length == 4)
                        {
                            MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Fourltr_Word);
                        }
                        else if (Globals.Threeltr_Word.Length == 3)
                        {
                            MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Threeltr_Word);
                        }

                    }
                    Globals.GuessesRemaining--;
                }
                else
                {
                    if (Globals.Sixltr_Word.Length == 6)
                    {
                        MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Sixltr_Word);
                    }
                    else if (Globals.Fiveltr_Word.Length == 5)
                    {
                        MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Fiveltr_Word);
                    }
                    else if (Globals.Fourltr_Word.Length == 4)
                    {
                        MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Fourltr_Word);
                    }
                    else if (Globals.Threeltr_Word.Length == 3)
                    {
                        MessageBox.Show("Your friend just painfully watched you fail to guess a " + Globals.LettersSet + " letter word and died in the process.\nCongratulations, you played yourself. \nThe correct word was " + Globals.Threeltr_Word);
                    }
                }
            }
        }

        private void CheckIfWin()
        {
            if (Globals.PointsEarned == Globals.LettersSet)
            {
                AddWin();
                RefreshWins();
                if (Globals.GuessesRemaining == 9)
                {
                    MessageBox.Show("You just won the most epic game of Supreme Hangman EVER! \nYou got every guess correct! Great job!");
                    pg_Title.Visibility = Visibility.Visible;
                    pg_Play.Visibility = Visibility.Hidden;
                }
                else if (Globals.GuessesRemaining > 6)
                {
                    MessageBox.Show("Congrazzles! \nYou won a game of Supreme Hangman and only guessed " + (9 - Globals.GuessesRemaining) + " letters wrong.. \nTry to beat your record next time!");
                    pg_Title.Visibility = Visibility.Visible;
                    pg_Play.Visibility = Visibility.Hidden;
                }
                else if (Globals.GuessesRemaining > 3)
                {
                    MessageBox.Show("Dominance isn't for everyone. You won and only guessed " + (9 - Globals.GuessesRemaining) + " letters wrong. \nTry to beat your record next time!");
                    pg_Title.Visibility = Visibility.Visible;
                    pg_Play.Visibility = Visibility.Hidden;
                }
                else if (Globals.GuessesRemaining > 0)
                {
                    MessageBox.Show("Oh wow. That was a close one!\n You barely scraped by, guessing " + (9 - Globals.GuessesRemaining) + " letters wrong. \nBetter luck next time!");
                    pg_Title.Visibility = Visibility.Visible;
                    pg_Play.Visibility = Visibility.Hidden;
                }

            }
        }

        //Method involving adding a win to "wins.txt"
        private static void AddWin()
        {
            //Uses a stream reader to read data from the text file "wins.txt"
            using (StreamReader WinReader = new StreamReader("wins.txt"))
            {
                //Reads from "wins.txt and outputs an integer to Globals.SavedWins.
                int.TryParse(WinReader.ReadLine(), out Globals.SavedWins);
                //Closes the stream reader.
                WinReader.Close();
                //Writes a line to the console displaying the amount of saved wins in "wins.txt"
                Console.WriteLine("(Pre-win) Current saved wins in wins.txt = " + Globals.SavedWins);
            }

            //Uses a stream writer to write data to the text file "wins.txt"
            using (StreamWriter WinWriter = new StreamWriter("wins.txt"))
            {
                //Writes to "wins.txt" the amount stored in Globals.SavedWins + 1
                WinWriter.Write((Globals.SavedWins + 1));
                WinWriter.Flush();
                WinWriter.Close();
                Console.WriteLine("(Post-win) Current saved wins in wins.txt = " + (Globals.SavedWins + 1));
            }
        }

        //Method involving refreshing the amount of wins displayed at main menu.
        private void RefreshWins()
        {
            //Uses a stream reader to read data from the text file "wins.txt"
            using (StreamReader WinDisplay = new StreamReader("wins.txt"))
            {
                //Displays the amount of wins at the main menu based upon the value stored in "wins.txt"
                txt_WinCounter.Text = "Games Won Of Hangman Supreme: " + WinDisplay.ReadLine();
            }
        }
    }
}
