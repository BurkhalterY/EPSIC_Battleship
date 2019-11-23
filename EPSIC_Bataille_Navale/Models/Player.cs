using EPSIC_Bataille_Navale.Properties;

namespace EPSIC_Bataille_Navale.Models
{
    public class Player
    {
        public GridModel grid;
        public string playerName;
        public int sonars;
        public int nuclearBombs;

        public Player(GridModel grid, string playerName)
        {
            this.grid = grid;
            this.playerName = playerName;
            sonars = Settings.Default.nbSonars;
            nuclearBombs = Settings.Default.nbNuclearBombs;
        }
    }
}
