using Microsoft.AspNetCore.Mvc;
using TennisAPI.BusinessLayer;

namespace TennisAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerDepot _playerDepot;
        public PlayerController(ILogger<PlayerController> logger, IPlayerDepot playerDepot)
        {
            _logger = logger;
            _playerDepot = playerDepot;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Le num�ro du joueur doit �tre sup�rieur � z�ro.", nameof(id));
                }
                var result = _playerDepot.GetById(id);
                if (result == null)
                {
                    _logger.LogWarning("Aucun joueur trouv� avec l'ID : {Id}", id);
                    return NotFound(new { Message = $"Aucun joueur trouv� avec l'ID {id}." });
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Num�ro de joueur invalide : {Id}", id);
                return BadRequest(new { Message = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors de la r�cup�ration du joueur avec l'ID : {Id}", id);
                return StatusCode(500, new { Message = "Une erreur s'est produite lors de la r�cup�ration du joueur." });
            }
        }

        [HttpGet]
        [Route("All")]
        public IActionResult All()
        {
            try
            {
                return Ok(_playerDepot.GetAll());

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors de la r�cup�ration de tous les joueurs.");
                return StatusCode(500, new { Message = "Une erreur s'est produite lors de la r�cup�ration de tous les joueurs." });
            }
                
        }
        [HttpGet]
        [Route("Statistics")]
        public IActionResult Statistics()
        {
           try
            {
                var stats = _playerDepot.GetStatistics();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors de la r�cup�ration des statistiques des joueurs.");
                return StatusCode(500, new { Message = "Une erreur s'est produite lors de la r�cup�ration des statistiques des joueurs." });
            }
        }
    }
}
