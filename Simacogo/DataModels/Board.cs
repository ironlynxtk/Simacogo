using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simacogo.DataModels
{
    public class Board
    {
        public Piece[,] State { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        #region Constructors
        public Board(int width, int height)
        {
            State = new Piece[width, height];
            Width = width;
            Height = height;
        }

        public Board(Piece[,] currentState)
        {
            State = ArrayCopy(currentState);
        }
        #endregion

        //Deep array copy, copies every value individually to new gameboard.
        private Piece[,] ArrayCopy(Piece[,] currentState)
        {
            var x = currentState.GetLength(0);
            var y = currentState.GetLength(1);
            var newState = new Piece[x, y];
            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    newState[i, j] = currentState[i, j];
                }
            }

            return newState;
        }

        //We reset all the visited pieces when we calculate the score for a gameboard.
        public void ResetVisited()
        {
            var x = State.GetLength(0);
            var y = State.GetLength(1);

            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    if (State[i, j] != null)
                    {
                        State[i, j].Visited = false;
                    }
                }
            }
        }

        //Add piece to board based on column and player, black = AI, white = human
        //If picks a column then starts from the bottom and goes up each row to find the first empty spot.
        public void AddToBoard(int col, bool isBlack)
        {
            var row = State.GetLength(1) - 1;
            //Find deepest open spot 
            for (var i = row; i >= 0; i--)
            {
                if (State[i, col] == null)
                {
                    var piece = new Piece(isBlack);
                    piece.Position.Row = i;
                    piece.Position.Col = col;
                    State[i, col] = piece;
                    break;
                }
            }

        }

        //Checks the 1st row of the array if every column has a piece, then the whole board is full, end game.
        public bool IsFull()
        {
            var isFull = false;
            var numberOfFullCols = 0;
            var cols = State.GetLength(1);
            for (var i = 0; i < cols; i++)
            {
                if (!ValidateMove(i))
                {
                    numberOfFullCols++;
                }
            }
            if (numberOfFullCols == State.GetLength(1))
            {
                isFull = true;
            }

            return isFull;
        }

        //Checks if a specified column is empty.
        public bool ValidateMove(int col)
        {
            if (State[0, col] != null)
            {
                return false;
            }
            return true;
        }

        //Prints the gamboard.
        public void PrintState()
        {
            var x = State.GetLength(0);
            var y = State.GetLength(1);

            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    if (State[i, j] == null)
                    {
                        Console.Write(".  ");
                    }
                    else
                    {
                        Console.Write(State[i, j].Color + "  ");
                    }

                }
                Console.WriteLine("\n");
            }

        }
    }
}
