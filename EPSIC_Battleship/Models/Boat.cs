using System.Collections.Generic;

namespace EPSIC_Battleship.Models
{
    public class Boat
    {
        public int touchedCell = 0;
        public List<Cell> cells = new List<Cell>();
        public int[] startCell;
        public Direction orientation;
    }
}
