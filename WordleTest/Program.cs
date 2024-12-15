using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WordleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] Allwords = File.ReadAllLines("wordspicked.txt");
            for (int i = 0; i < Allwords.Length; i++)
            {
                Allwords[i] = Allwords[i].ToUpper();
            }
            int WordNumber = new Random().Next(0, 15920);
            string CorWord = File.ReadLines("wordspicked.txt").ElementAt(WordNumber - 1);
            Console.OutputEncoding = Encoding.UTF8;
            Board board = new Board(CorWord.ToUpper(),Allwords);
            bool iswin = false;
            int turnc = 0;
            board.PrintBoard();
            while (iswin == false && turnc < 4)
            {
                board.InputLetter();
                board.PrintBoard();
                iswin = board.checkwins();
                turnc = board.ReturnTurn();
            }
            if (iswin == true)
            {
                Console.WriteLine($"Sveikiname, teisingas zodis yra {CorWord}!!!");
            }
            else
            {
                Console.WriteLine($"Baigesi ejimai. Teisingas zodis buvo {CorWord}");
            }
        }
    }
    class Letter
    {
        public char letter { get; set; }
        public bool isgreen { get; set; }
        public bool isyellow { get; set; }
        public Letter(char letter, bool isgreen, bool isyellow)
        {
            this.letter = letter;
            this.isgreen = isgreen;
            this.isyellow = isyellow;
        }
        public Letter(char letter)
        {
            this.letter = letter;
        }
        public void SetGreen(bool val)
        {
            isgreen = val;
        }
        public void SetYellow(bool val)
        {
            isyellow = val;
        }
    }
    class Board
    {
        string CorWord { get; set; }
        bool iswin = false;
        bool islose = false;
        int turnn = -1;
        private List<Turn> turns = new List<Turn>();
        string[] Allwords { get; set; }
        public Board(string corWord, string[] AllWords)
        {
            CorWord = corWord;
            Allwords = AllWords;
        }

        public void PrintBoard()
        {
            Console.WriteLine(new string('-', 7));
            for (int i = 0; i < 5; i++)
            {
                Console.Write("|");
                if (i <= turnn)
                {
                    turns[i].WriteWord();
                }
                else
                {
                    Console.Write(new string('•', 5));
                }
                Console.Write("|" + "\n");
            }
            Console.WriteLine(new string('-', 7));
        }
        public void InputLetter()
        {
            bool isvalid = false;
            Console.WriteLine("Input a 5 letter word: ");
            string word = Console.ReadLine();
            if (word.Length == 5)
            {
                isvalid = true;
            }
            if (validword(word, Allwords) == false)
            {
                isvalid = false;
            }
            while (isvalid == false)
            {
                Console.WriteLine("Invalid word, please enter a valid word!!!");
                word = Console.ReadLine();
                if (word.Length == 5)
                {
                    isvalid = true;
                }
                if (validword(word, Allwords) == false)
                {
                    isvalid = false;
                }
            }
            Turn turn = new Turn(word);
            turn.LetValues(CorWord);
            turns.Add(turn);
            this.turnn++;
        }
        public bool checkwins()
        {
            if (turns[turnn].iswin() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int ReturnTurn()
        {
            return turnn;
        }
        public bool validword(string word, string[] allwords)
        {
            if (allwords.Contains(word.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        class Turn
        {
            private List<Letter> letters = new List<Letter>();
            private string word { get; set; }

            public Turn(string word)
            {
                this.word = word.ToUpper();

                for (int i = 0; i < word.Length; i++)
                {
                    Letter letter = new Letter(word[i]);
                    letters.Add(letter);
                }
            }
            public void LetValues(string cword)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cword[i] == word[i])
                    {
                        letters[i].SetGreen(true);
                    }
                }
                foreach (Letter letter in letters)
                {
                    if (cword.Contains(char.ToUpper(letter.letter)))
                    {
                        letter.SetYellow(true);
                    }
                }
            }
            public string getWord()
            {
                return this.word;
            }
            public void WriteWord()
            {
                for (int i = 0; i < 5; i++)
                {
                    if (letters[i].isgreen == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(char.ToUpper(letters[i].letter));
                        Console.ResetColor();
                    }
                    else if (letters[i].isyellow == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(char.ToUpper(letters[i].letter));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(char.ToUpper(letters[i].letter));
                    }
                }
            }
            public bool iswin()
            {
                bool iswin = true;
                foreach (Letter letter in letters)
                {
                    if (letter.isgreen == false)
                    {
                        iswin = false;
                    }
                }
                return iswin;
            }
        }
    }
}