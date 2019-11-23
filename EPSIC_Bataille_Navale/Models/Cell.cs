namespace EPSIC_Bataille_Navale.Models
{
    public enum State
    {
        noActivity,
        noBoat,
        boat,
        fullBoat,
        revealed,
        invalid
    }

    public class Cell
    {
        public State state = State.noActivity;
        public Boat boat = null;
    }
}
