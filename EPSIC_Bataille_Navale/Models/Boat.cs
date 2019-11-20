using System.Collections.Generic;

namespace EPSIC_Bataille_Navale.Models
{
    public class Boat
    {
        public int touchedCell = 0;
        public List<Cell> cells = new List<Cell>();
        public Directions orientation;
    }
}
