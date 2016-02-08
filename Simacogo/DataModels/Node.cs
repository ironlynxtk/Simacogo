using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simacogo.DataModels
{
    public class Node
    {
        public Board Board { get; set; }
        public List<Node> Children { get; set; }
        public Node()
        {
            Children = new List<Node>();
        }
        
        //Check if leaf node
        public bool IsTerminalNode()
        {
            var children = Children.Count();
            if (children > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
