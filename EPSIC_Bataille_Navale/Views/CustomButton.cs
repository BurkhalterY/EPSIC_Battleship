using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Classe héritée de Button qui permet le stockage des positions X et Y des boutons
    /// </summary>
    public class CustomButton : Button
    {
        public int x, y;
        public CustomButton(int x, int y) : base()
        {
            this.x = x;
            this.y = y;
        }
    }
}
