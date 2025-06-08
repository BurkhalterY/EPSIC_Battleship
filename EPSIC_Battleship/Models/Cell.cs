namespace EPSIC_Battleship.Models
{
    public enum State
    {
        noActivity,
        noBoat,
        boat,
        fullBoat,
        revealed,
        noFind,
        partialFind,
        invalid
    }

    public class Cell
    {
        public State state = State.noActivity;
        public Boat boat = null;
        public int x, y;
    }
}
