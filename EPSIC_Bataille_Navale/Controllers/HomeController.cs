using EPSIC_Bataille_Navale.I18n;
using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Properties;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class HomeController
    {
        public SetupController setupP1;
        public SetupController setupP2;
        public GameType gameType;

        public void PlaySolo()
        {
            gameType = GameType.Solo;
            setupP1 = new SetupController(Settings.Default.size);
            setupP2 = new SetupController(Settings.Default.size);

            setupP2.AIChoise();
            setupP2.playerName = Strings.TheAI;
        }

        public void PlayDemo()
        {
            gameType = GameType.Demo;
            setupP1 = new SetupController(Settings.Default.size);
            setupP2 = new SetupController(Settings.Default.size);

            setupP1.AIChoise();
            setupP1.playerName = string.Format(Strings.AIn, 1);

            setupP2.AIChoise();
            setupP2.playerName = string.Format(Strings.AIn, 2);
        }
    }
}
