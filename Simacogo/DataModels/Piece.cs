using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simacogo.DataModels
{
    public class Piece
    {
        public Position Position { get; set; } //"row/col format"
        public bool Visited { get; set; } //used for scoring
        public bool IsBlack { get; set; } //0 is white, 1 is black
        public string Color { get; set; } //"W" or "B"

        //Constructor
        public Piece(bool isBlack)
        {
            Position = new Position();
            Visited = false;
            IsBlack = isBlack;
            if (IsBlack)
            {
                Color = "B";
            }
            else
            {
                Color = "W";
            }
        }
    }
}
