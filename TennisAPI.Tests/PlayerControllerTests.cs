using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using TennisAPI.BusinessLayer;
using TennisAPI.Controllers;

namespace TennisAPI.Tests
{
    [TestClass]
    public sealed class PlayerControllerTests
    {
        ILoggerFactory _loggerFactory = new NullLoggerFactory();
        
        [TestMethod]
        public void TestAll()
        {
            
            var logger = _loggerFactory.CreateLogger<PlayerController>();
            var depot = new TennisAPI.BusinessLayer.PlayerDepot();
            var controller = new PlayerController(logger, depot);
            var players = controller.All();

            Assert.IsNotNull(players, "La liste des joueurs ne doit pas être nulle.");
            Assert.IsTrue(players is Microsoft.AspNetCore.Mvc.OkObjectResult, "La réponse doit être un OkObjectResult.");
            var playersResponse = players as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.IsTrue(playersResponse?.Value is IEnumerable<Player>, "La valeur de la réponse doit être une collection de joueurs.");
            var playersList = playersResponse?.Value as IEnumerable<Player>;
            Assert.IsTrue(playersList?.Count() > 0, "La collection de joueurs doit contenir au moins un joueur.");
            var firstPlayer = playersList?.FirstOrDefault();
            Assert.IsTrue(firstPlayer?.data?.rank == 1, "Le premier joueur de la liste doit avoir un rang égal à 1.");
        }
        [TestMethod]
        public void Test
    }
}
