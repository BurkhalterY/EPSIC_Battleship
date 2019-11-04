using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPSIC_Bataille_Navale.Models
{
    public enum State
    {
        noActivity,
        noBoat,
        boat,
        fullBoat,
        invalid
    }

    public class Cell
    {
        public State state = State.noActivity;
        public Boat boat = null;
    }
}
