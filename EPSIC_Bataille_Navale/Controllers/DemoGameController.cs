using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Views;
using System;
using System.Windows.Threading;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class DemoGameController : GameController
    {
        AI ai, ai2;

        public DemoGameController(Game view) : base(view)
        {
            ai = new AI(this);
            ai2 = new AI(this);
        }

        public override void Click(int x = 0, int y = 0)
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
                view.Finish(playersNames[playerTurn]);
            }
        }
    }
}
