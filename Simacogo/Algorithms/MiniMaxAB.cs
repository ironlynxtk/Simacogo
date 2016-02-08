using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simacogo.DataModels;
using Simacogo.Helpers;

namespace Simacogo.Algorithms
{
    public class MiniMaxAb
    {
        //Search with alpha beta pruning
        public int Search(Node node, int depth, int alpha, int beta, bool maxPlayer)
        {
            if (depth == 0 || node.IsTerminalNode())
            {
                return Utility.Score(node);
            }

            if (maxPlayer)
            {
                int bestValue = Int32.MinValue;
                node.Children = Utility.GenerateSuccessors(node.Board, false);
                for (var i = 0; i < node.Children.Count; i++)
                {
                    bestValue = Math.Max(bestValue, Search(node.Children[i], depth - 1, alpha, beta, false));
                    alpha = Math.Max(bestValue, bestValue);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return alpha;
            }

            else
            {
                int bestValue = Int32.MaxValue;
                node.Children = Utility.GenerateSuccessors(node.Board, true);
                for (var i = 0; i < node.Children.Count; i++)
                {
                    bestValue = Math.Min(bestValue, Search(node.Children[i], depth - 1, alpha, beta, true));
                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return beta;
            }
        }
    }
}
