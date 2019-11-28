using EPSIC_Bataille_Navale.Controllers;
using EPSIC_Bataille_Navale.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPSIC_Bataille_Navale.Tests
{
    [TestClass]
    public class SetupControllerTests
    {
        [TestMethod]
        public void PlaceBoat_B3_B5_True()
        {
            SetupController setup = new SetupController(10);
            setup.Click(setup.grid.grid[1, 2]);
            setup.Click(setup.grid.grid[1, 4]);
            Assert.AreEqual(1, setup.boats.Count);
            Boat boat = setup.boats[0];
            Assert.AreEqual(Direction.Down, boat.orientation);
            Assert.AreEqual(3, boat.cells.Count);
        }

        [TestMethod]
        public void PlaceBoat_B3_C5_False()
        {
            SetupController setup = new SetupController(10);
            setup.Click(setup.grid.grid[1, 2]);
            setup.Click(setup.grid.grid[2, 4]);
            Assert.AreEqual(0, setup.boats.Count);
        }
    }
}
