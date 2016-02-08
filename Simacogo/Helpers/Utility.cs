using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simacogo.Algorithms;
using Simacogo.DataModels;

namespace Simacogo.Helpers
{
    public static class Utility
    {
        //This method will search all the columns and return the best move for the AI to make.
        //Will run AlphaBeta or regular Minimax depending on user selection.
        public static int AiHelperMove(Board board, bool alphabeta, int depth)
        {
            var rows = board.State.GetLength(1) - 1;
            int bestValue = Int32.MinValue;
            int bestColumn = -1;

           
            if (alphabeta)
            {
                MiniMaxAb miniMaxAb = new MiniMaxAb();
                for (var i = 0; i <= rows; i++)
                {
                    var node = new Node();
                    var tempBoard = new Board(board.State);
                    if (tempBoard.ValidateMove(i))
                    {
                        tempBoard.AddToBoard(i, true);
                        node.Board = tempBoard;
                        var currentValue = miniMaxAb.Search(node, depth, Int32.MinValue, Int32.MaxValue, false);
                        if (currentValue >= bestValue)
                        {
                            bestValue = currentValue;
                            bestColumn = i;
                        }
                    }
                }
            }
            else
            {
                MiniMax miniMax = new MiniMax();
                for (var i = 0; i <= rows; i++)
                {
                    var node = new Node();
                    var tempBoard = new Board(board.State);
                    if (tempBoard.ValidateMove(i))
                    {
                        tempBoard.AddToBoard(i, true);
                        node.Board = tempBoard;
                        var currentValue = miniMax.Search(node, depth, false);
                        if (currentValue >= bestValue)
                        {
                            bestValue = currentValue;
                            bestColumn = i;
                        }
                    }
                }
            }

            
            return bestColumn;
        }

        #region Algorithm Helpers
        public static List<Node> GenerateSuccessors(Board board, bool isBlack)
        {
            //Validate moves based on current state to generate new states

            var nodes = new List<Node>();


            //Generate # of valid moves
            var ValidMoves = GetNumberOfMoves(board);
            //Foreach valid move, generate a new node (evaluated in minimax)
            for (var i = 0; i < ValidMoves.Length; i++)
            {
                if (ValidMoves[i])
                {
                    var tempBoard = new Board(board.State);
                    //Make the move
                    tempBoard.AddToBoard(i, isBlack);
                    //Create a Node
                    var node = new Node();
                    node.Board = tempBoard;
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        public static bool[] GetNumberOfMoves(Board board)
        {// Check if each column is full
            var col = board.State.GetLength(1);
            var validColumnsToMoveTo = new bool[col];
            //Initialize boolean array, each cell represents a column 
            for (var i = 0; i < validColumnsToMoveTo.Length; i++)
            {
                validColumnsToMoveTo[i] = false;
            }

            for (var i = 0; i < col; i++)
            {
                if (board.State[0, i] == null)
                {
                    validColumnsToMoveTo[i] = true;
                }
            }


            return validColumnsToMoveTo;
        }
        #endregion

        #region Scoring and Scoring Helper methods

        //This method will get the score based on a given board state.
        //It looks at a piece in the gameboard, looks at adjacent pieces, then scores them if they are the same
        //color. If not it skips them. Adjacent pieces are worth 2 points, diagonal are worth 1 point.
        //If it scores a pair of pieces, it marks the piece as visited so it prevents repeat scoring when
        //it checks other pieces.
        public static int Score(Node node)
        {
            var row = node.Board.State.GetLength(0);
            var col = node.Board.State.GetLength(1);
            var BlackScore = 0;
            var WhiteScore = 0;
            node.Board.ResetVisited();
            //Console.WriteLine("\n");
            //Console.WriteLine("Scoring The following Board");
            //node.Board.PrintState();

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    //Get first piece found
                    if (node.Board.State[i, j] != null)
                    {
                        var piece = node.Board.State[i, j];

                        //Get list of Adjacent pieces
                        var adjacentPieces = GetAdjacentPieces(piece, node.Board);

                        //Compare piece to each adj piece, if its diag/or not, and add up score
                        foreach (var tuple in adjacentPieces)
                        {
                            //If piece has been visited or is another color, skip it.
                            if (tuple.Item2 != null && !tuple.Item2.Visited)
                            {
                                if (tuple.Item1) //if true, its a diagonal score
                                {
                                    if (piece.IsBlack && tuple.Item2.IsBlack)
                                    {
                                        BlackScore++;
                                        piece.Visited = true;
                                    }
                                    else if (!piece.IsBlack && !tuple.Item2.IsBlack)
                                    {
                                        WhiteScore++;
                                        piece.Visited = true;
                                    }
                                }
                                else //Its a normal position worth 2 points
                                {
                                    if (piece.IsBlack && tuple.Item2.IsBlack)
                                    {
                                        BlackScore += 2;
                                        piece.Visited = true;
                                    }
                                    else if (!piece.IsBlack && !tuple.Item2.IsBlack)
                                    {
                                        WhiteScore += 2;
                                        piece.Visited = true;
                                    }
                                }
                            }
                        }


                    }
                }
            }

            return BlackScore - WhiteScore;
        }

        //Same as above, except it returns a Tuple with both Black and White scores.
        public static Tuple<int, int> FinalScore(Node node)
        {
            var row = node.Board.State.GetLength(0);
            var col = node.Board.State.GetLength(1);
            var BlackScore = 0;
            var WhiteScore = 0;
            node.Board.ResetVisited();
            //Console.WriteLine("\n");
            //Console.WriteLine("Scoring The following Board");
            //node.Board.PrintState();

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    //Get first piece found
                    if (node.Board.State[i, j] != null)
                    {
                        var piece = node.Board.State[i, j];

                        //Get list of Adjacent pieces
                        var adjacentPieces = GetAdjacentPieces(piece, node.Board);

                        //Compare piece to each adj piece, if its diag/or not, and add up score
                        foreach (var tuple in adjacentPieces)
                        {
                            //If piece has been visited or is another color, skip it.
                            if (tuple.Item2 != null && !tuple.Item2.Visited)
                            {
                                if (tuple.Item1) //if true, its a diagonal score
                                {
                                    if (piece.IsBlack && tuple.Item2.IsBlack)
                                    {
                                        BlackScore++;
                                        piece.Visited = true;
                                    }
                                    else if (!piece.IsBlack && !tuple.Item2.IsBlack)
                                    {
                                        WhiteScore++;
                                        piece.Visited = true;
                                    }
                                }
                                else //Its a normal position worth 2 points
                                {
                                    if (piece.IsBlack && tuple.Item2.IsBlack)
                                    {
                                        BlackScore += 2;
                                        piece.Visited = true;
                                    }
                                    else if (!piece.IsBlack && !tuple.Item2.IsBlack)
                                    {
                                        WhiteScore += 2;
                                        piece.Visited = true;
                                    }
                                }
                            }
                        }


                    }
                }
            }
            return Tuple.Create(WhiteScore, BlackScore);
        }


       

        //Returns all adjacent pieces to a given piece, hardcoded the search, could have been better.
        public static List<Tuple<bool, Piece>> GetAdjacentPieces(Piece piece, Board board)
        {
            var row = board.State.GetLength(0) - 1;
            var col = board.State.GetLength(1) - 1;
            var listOfAdjacentPieces = new List<Tuple<bool, Piece>>();
            if (piece.Position.Row > 0) //Top Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(false, board.State[piece.Position.Row - 1, piece.Position.Col]));
            }
            if (piece.Position.Col < col && piece.Position.Row > 0) //TopRight Boundary Test
            {
                var adjPiece = board.State[piece.Position.Row - 1, piece.Position.Col + 1];
                listOfAdjacentPieces.Add(Tuple.Create(true, adjPiece));
            }
            if (piece.Position.Col < col) //Right Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(false, board.State[piece.Position.Row, piece.Position.Col + 1]));
            }
            if (piece.Position.Row < row && piece.Position.Col < col) //BottomRight Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(true, board.State[piece.Position.Row + 1, piece.Position.Col + 1]));
            }
            if (piece.Position.Row < row)//Bottom Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(false, board.State[piece.Position.Row + 1, piece.Position.Col]));
            }
            if (piece.Position.Row < row && piece.Position.Col > 0) //BottomLeft Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(true, board.State[piece.Position.Row + 1, piece.Position.Col - 1]));
            }
            if (piece.Position.Col > 0) //Left Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(false, board.State[piece.Position.Row, piece.Position.Col - 1]));
            }
            if (piece.Position.Row > 0 && piece.Position.Col > 0)//TopLeft Boundary Test
            {
                listOfAdjacentPieces.Add(Tuple.Create(true, board.State[piece.Position.Row - 1, piece.Position.Col - 1]));
            }

            return listOfAdjacentPieces;
        }

        #endregion

    }
}
