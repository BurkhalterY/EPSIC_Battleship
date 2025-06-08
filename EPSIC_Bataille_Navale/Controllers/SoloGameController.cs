using EPSIC_Bataille_Navale.Models;
using EPSIC_Bataille_Navale.Properties;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Threading;

namespace EPSIC_Bataille_Navale.Controllers
{
    public class SoloGameController : GameController
    {
        private AI ai;

        public SoloGameController(Player[] players) : base(players)
        {
            ai = new AI(this);
        }

        /// <summary>
        /// Manage turn by turn
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public override void Click(int x = 0, int y = 0, ActionType action = ActionType.NormalShot)
        {
            if (playerTurn == 0)
            {
                switch (action)
                {
                    case ActionType.NormalShot: ClickAt(x, y); break;
                    case ActionType.Sonar: Sonar(); break;
                    case ActionType.NuclearBomb: NuclearAttack(x, y); break;
                }
            }
            if (finish)
            {
                RaiseOnFinish(players[playerNotTurn].playerName);
            }
            else if (playerTurn == 1)
            {
                RaiseOnActiveGrid(false);
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Settings.Default.iaSleepTime) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    ai.AIPlay();
                    if (finish)
                    {
                        RaiseOnFinish(players[playerNotTurn].playerName);
                    }
                    else
                    {
                        RaiseOnActiveGrid(true);
                    }
                };
            }
        }

        public override bool SendMessage(string message, int player)
        {
            if (base.SendMessage(message, player))
            {
                if (player == 0)
                {
                    _ = Simsimi(message);
                }
                return true;
            }
            return false;
        }

        private async Task Simsimi(string message)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent("{\"utext\":\"" + message + "\", \"lang\":\"fr\"}", Encoding.UTF8, "application/json");
            content.Headers.Add("x-api-key", "f4oaBjotM/VSE5Rgx3akA3JjFX+g3sIJUBCWz/Fc"); // TODO: Remove this hardcoded thing
            HttpResponseMessage response = await client.PostAsync("https://wsapi.simsimi.com/190410/talk", content);
            string responseString = await response.Content.ReadAsStringAsync();

            Dictionary<string, object> values = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(responseString);
            if (values["status"].ToString() == "200")
            {
                SendMessage(values["atext"].ToString(), 1);
            }
        }
    }
}
