using EPSIC_Bataille_Navale.Models;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class NetworkGameController : GameController
    {
        public OnlineController onlineController;

        public NetworkGameController(Player[] players, GameType gameType) : base(players)
        {
            if(gameType == GameType.Client)
            {
                InvertPlayer();
            }
        }

        public override void Click(int x = 0, int y = 0, ActionType action = ActionType.NormalShot)
        {
            if (playerTurn == 0)
            {
                switch (action)
                {
                    case ActionType.NormalShot:
                        ClickAt(x, y);
                        onlineController.message = Action.clic;
                        onlineController.objectToSend = new int[] { x, y };
                        onlineController.backgroundSender.RunWorkerAsync();
                        break;
                    case ActionType.Sonar:
                        int[] cible = Sonar();
                        if(cible.Length == 2)
                        {
                            onlineController.message = Action.sonar;
                            onlineController.objectToSend = new int[] { cible[0], cible[1] }; ;
                            onlineController.backgroundSender.RunWorkerAsync();
                        }
                        break;
                    case ActionType.NuclearBomb:
                        NuclearAttack(x, y);
                        onlineController.message = Action.nuclearBomb;
                        onlineController.objectToSend = new int[] { x, y };
                        onlineController.backgroundSender.RunWorkerAsync();
                        break;
                }
            }
            if (finish)
            {
                onlineController.Terminate();
                RaiseOnFinish(players[playerTurn].playerName);
            }
            else if (playerTurn == 1)
            {
                RaiseOnActiveGrid(false);
            }
        }

        public override bool SendMessage(string message, int player)
        {
            if(base.SendMessage(message, player))
            {
                if(player == 0)
                {
                    onlineController.message = Action.message;
                    onlineController.objectToSend = message;
                    onlineController.backgroundSender.RunWorkerAsync();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pour checker le win de l'adversaire
        /// </summary>
        public void CheckWinAndTurn()
        {
            if (finish)
            {
                onlineController.Terminate();
                RaiseOnFinish(players[playerTurn].playerName);
            }
            else if (playerTurn == 0)
            {
                RaiseOnActiveGrid(true);
            }
        }
    }
}
