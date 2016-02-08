using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simacogo.DataModels;
using Simacogo.Helpers;

namespace Simacogo.Algorithms
{
    public class MiniMax
    {
        public int Search(Node node, int depth, bool maxPlayer)
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
                    var currentValue = Search(node.Children[i], depth - 1, false);
                    bestValue = Math.Max(bestValue, currentValue);
                }

                return bestValue;
            }

            else
            {
                int bestValue = Int32.MaxValue;
                node.Children = Utility.GenerateSuccessors(node.Board, true);
                for (var i = 0; i < node.Children.Count; i++)
                {
                    var currentValue = Search(node.Children[i], depth - 1, true);
                    bestValue = Math.Max(bestValue, currentValue);
                }
                return bestValue;
            }
        }

       


      
    }
}
