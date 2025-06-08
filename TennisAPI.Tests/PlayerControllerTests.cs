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
            var depot = new PlayerDepot();
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
        public void TestGetById()
        {
            var logger = _loggerFactory.CreateLogger<PlayerController>();
            var depot = new PlayerDepot();
            var controller = new PlayerController(logger, depot);
            var player = controller.Get(52);
            Assert.IsNotNull(player, "Le joueur ne doit pas être nul.");
            Assert.IsTrue(player is Microsoft.AspNetCore.Mvc.OkObjectResult, "La réponse doit être un OkObjectResult.");
            var playerResponse = player as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.IsTrue(playerResponse?.Value is Player, "La valeur de la réponse doit être un joueur.");
            var playerData = playerResponse?.Value as Player;
            Assert.IsTrue(playerData?.id == 52, "Le joueur retourné doit avoir un ID égal à 52.");
        }

        [TestMethod]
        public void TestGetByIdNotFound()
        {
            var logger = _loggerFactory.CreateLogger<PlayerController>();
            var depot = new PlayerDepot();
            var controller = new PlayerController(logger, depot);
            var player = controller.Get(9999); // ID qui n'existe pas
            Assert.IsNotNull(player, "La réponse ne doit pas être nulle.");
            Assert.IsTrue(player is Microsoft.AspNetCore.Mvc.NotFoundObjectResult, "La réponse doit être un NotFoundObjectResult.");
            var playerResponse = player as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
        }

        [TestMethod]
        public void TestStatistics()
        {
            var logger = _loggerFactory.CreateLogger<PlayerController>();
            var depot = new PlayerDepot();
            var controller = new PlayerController(logger, depot);
            var stats = controller.Statistics();
            Assert.IsNotNull(stats, "Les statistiques ne doivent pas être nulles.");
            Assert.IsTrue(stats is Microsoft.AspNetCore.Mvc.OkObjectResult, "La réponse doit être un OkObjectResult.");
            var statsResponse = stats as Microsoft.AspNetCore.Mvc.OkObjectResult;
            Assert.IsTrue(statsResponse?.Value is Statistics, "La valeur de la réponse doit être une statistique de joueurs.");
            var statsData = statsResponse?.Value as Statistics;
            Assert.IsTrue(statsData?.CountryWithMostWins == "SRB", "Le pays avec le plus grand ratio de victoires est erronné.");
            Assert.IsTrue(statsData?.MeanWeightRatio == 23.357838995505837, "La valeur de la moyenne des IMC est erronée");
            Assert.IsTrue(statsData?.MedianeHeight == 185, "La valeur de la medianne des tailles des joueurs est erronée");
        }
    }
}
