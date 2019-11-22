using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Windows.Threading;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SoloGameController : GameController
    {
        AI ai;

        public SoloGameController(Game view) : base(view) {
            ai = new AI(this);
        }

        /// <summary>
        /// Gère le tour par tour
        /// </summary>
        /// <param name="x">Coordonnée X</param>
        /// <param name="y">Coordonnée Y</param>
        public override void Click(int x = 0, int y = 0)
        {
            if (playerTurn == 0)
            {
                ClickAt(x, y);
            }
            if (finish)
            {
                view.Finish(playersNames[playerTurn]);
            }
            else if (playerTurn == 1)
            {
                view.clickable = false;
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Properties.Settings.Default.iaSleepTime) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    ai.AIPlay();
                    if (finish)
                    {
                        view.Finish(playersNames[playerTurn]);
                    }
                    else
                    {
                        view.clickable = true;
                    }
                };
            }
        }
    }
}
