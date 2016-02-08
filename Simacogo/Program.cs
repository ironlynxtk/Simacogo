using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simacogo.DataModels;
using Simacogo.Helpers;

namespace Simacogo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Following is setup for AI and board
            Console.WriteLine("Welcome to Simacogo! You are Player 1, Color white.");
            Console.WriteLine("At any time, type 'exit' to leave the game!");
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("\n");
            Console.WriteLine("Enter # of plies for AI to search to? (numeric only)");
            var depth = Int32.Parse(Console.ReadLine()); //AI lookahead value

            Console.WriteLine("\n");
            Console.WriteLine("Enable Alpha Beta Pruning? (True or False)");
            var alphabeta = Boolean.Parse(Console.ReadLine()); //AI lookahead value

            Console.WriteLine("\n");
            Console.WriteLine("Enter board width? (numeric only)");
            var width = Int32.Parse(Console.ReadLine()); //Board width

            Console.WriteLine("Enter board height? (numeric only)");
            var height = Int32.Parse(Console.ReadLine()); //Board height

            Console.WriteLine("\n");
            Board board = new Board(width, height);
            bool humanTurn = true;
            bool runProgram = true;
            while (!board.IsFull() && runProgram)
            {
                if (humanTurn)
                {
                    Console.WriteLine("--------------------Begin Human Turn-----------------------------------");
                    Console.WriteLine("Please type the column you want to drop a piece into and press enter. (1-9)");
                    string enteredValue = Console.ReadLine();
                    if (enteredValue.Equals("exit"))
                    {
                        runProgram = false;
                        break;
                    }
                    int colValue = Int32.MinValue;
                    Int32.TryParse(enteredValue, out colValue);
                    if (colValue > 0 && colValue < 10 && board.ValidateMove(colValue-1))
                    {
                        //Get column from User && Create new piece and enter it in the board at deepest possible position in column.
                        board.AddToBoard(colValue-1, false);
                        board.PrintState();

                        var node = new Node();
                        node.Board = board;
                        var score = Utility.FinalScore(node);
                        Console.WriteLine("\n");
                        Console.WriteLine("Current Score: \n");
                        Console.WriteLine("Human: " + score.Item1);
                        Console.WriteLine("AI " + score.Item2);
                        Console.WriteLine("--------------------End Human Turn----------------------");
                        Console.WriteLine("\n");
                        humanTurn = !humanTurn;
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection, please try again.");
                    }

                    
                }
                else
                {
                    Console.WriteLine("--------------------Begin AI Turn--------------------");
                    //Let AI player pick position
                    //Update Board
                    //AI Runs either Miniax or MinimaxAB depending on user selection.
                    var bestMove = Utility.AiHelperMove(board, alphabeta, depth);

                    board.AddToBoard(bestMove, true);
                    board.PrintState();
                    var node = new Node();
                    node.Board = board;
                    var score = Utility.FinalScore(node);
                    Console.WriteLine("\n");
                    Console.WriteLine("Current Score: \n");
                    Console.WriteLine("Human: " + score.Item1);
                    Console.WriteLine("AI " + score.Item2);
                    Console.WriteLine("\n");
                    Console.WriteLine("--------------------End AI Turn----------------------");
                    humanTurn = !humanTurn;
                }
            }

            Console.WriteLine("\n");
            Console.WriteLine("Gameboard is Full!");
            Console.WriteLine("\n");
            var finalNode = new Node();
            var tempBoard = new Board(board.State);
            finalNode.Board = tempBoard;
            var finalScore = Utility.FinalScore(finalNode);
            Console.WriteLine("Final Board");
            Console.WriteLine("\n");
            board.PrintState();
            Console.WriteLine("\n");
            Console.WriteLine("Final Score Is: ");
            Console.WriteLine("Human: " + finalScore.Item1);
            Console.WriteLine("AI: " + finalScore.Item2);

            Console.WriteLine("Thank you for playing Simacogo! Application will close in 5 seconds.");

            
            System.Threading.Thread.Sleep(5000);



        }
    }
}

