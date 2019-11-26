using EPSIC_Bataille_Navale.Models;
using System;
using System.Windows.Threading;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class DemoGameController : GameController
    {
        AI ai, ai2;

        public DemoGameController(Player[] players) : base( players)
        {
            ai = new AI(this);
            ai2 = new AI(this);
        }

        /// <summary>
        /// Methode qui gère le tour par tour entre les IA
        /// </summary>
        /// <param name="x">Coordonnée X</param>
        /// <param name="y">Coordonnée Y</param>
        public override void Click(int x = 0, int y = 0, ActionType action = ActionType.NormalShot)
        {
            if (!finish)
            {
                if (playerTurn == 0)
                {
                    DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Properties.Settings.Default.iaSleepTime) };
                    timer.Start();
                    timer.Tick += (sender, args) =>
                    {
                        timer.Stop();
                        ai2.AIPlay();
                        Click();
                    };
                }
                else if (playerTurn == 1)
                {
                    DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Properties.Settings.Default.iaSleepTime) };
                    timer.Start();
                    timer.Tick += (sender, args) =>
                    {
                        timer.Stop();
                        ai.AIPlay();
                        Click();
                    };
                }
            }
            else
            {
                RaiseOnFinish(players[playerTurn].playerName);
            }
        }
    }
}
