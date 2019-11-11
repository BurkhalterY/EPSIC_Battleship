namespace EPSIC_Bataille_Navale.Models
{
    // Classe qui définie l'état des cellules
    public enum State
    {
        // Différent type d'état pour les cellules
        noActivity,
        noBoat,
        boat,
        fullBoat,
        invalid
    }

    // Création de la classe "Cell"
    public class Cell
    {
        public State state = State.noActivity;
        public Boat boat = null;
    }
}
