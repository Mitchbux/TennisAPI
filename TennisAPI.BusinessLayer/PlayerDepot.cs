using System.Data;
using TennisAPI.DataLayer;

namespace TennisAPI.BusinessLayer
{
    public class PlayerDepot : IPlayerDepot
    {
        private readonly IDataLayer? _dataLayer = null;
        public PlayerDepot()
        {
        }
        public PlayerDepot(IDataLayer DataLayer)
        {
            _dataLayer = DataLayer ?? throw new ArgumentNullException(nameof(DataLayer), "Le data layer ne peut pas être null.");
        }
        public Player? GetById(int id)
        {
            if (_dataLayer == null)
            {
                var filename = "players.json";
                var json = File.ReadAllText(filename);
                var players = System.Text.Json.JsonSerializer.Deserialize<PlayersList>(json);
                if (players == null || players.players == null || players.players?.Count == 0)
                {
                    throw new InvalidOperationException($"Impossible de lire le fichier json {filename}");
                }
                else
                    return players?.players?.Find(p => p.id == id);
            }
            else
            {
                var player = new Player();
                var playerTable = _dataLayer.Query("SELECT * FROM PLAYERS WHERE ID=@ID", new P("ID",id));
                var countryId = 0;
                if (playerTable.Rows.Count == 0)
                {
                    return null; // Aucun joueur trouvé avec cet ID
                }else
                {
                    player.id = Convert.ToInt32(playerTable.Rows[0]["ID"]);
                    player.firstname = playerTable.Rows[0]["FIRSTNAME"]?.ToString() ?? string.Empty;
                    player.lastname = playerTable.Rows[0]["LASTNAME"]?.ToString() ?? string.Empty;
                    player.shortname = playerTable.Rows[0]["SHORTNAME"]?.ToString() ?? string.Empty;
                    player.sex = playerTable.Rows[0]["SEX"]?.ToString() ?? string.Empty;
                    player.picture = playerTable.Rows[0]["PICTURE"]?.ToString() ?? string.Empty;
                    countryId = Convert.ToInt32(playerTable.Rows[0]["COUNTRY_ID"]);
                }
                var playerDataTable = _dataLayer.Query("SELECT * FROM PLAYER_DATA WHERE PLAYER_ID=@ID", new P("ID", id));
                player.data = new PlayerData();
                if(playerDataTable.Rows.Count > 0)
                {
                    player.data.age = Convert.ToInt32(playerDataTable.Rows[0]["AGE"]);
                    player.data.rank = Convert.ToInt32(playerDataTable.Rows[0]["RANK"]);
                    player.data.height = Convert.ToInt32(playerDataTable.Rows[0]["HEIGHT"]);
                    player.data.weight = Convert.ToInt32(playerDataTable.Rows[0]["WEIGHT"]);
                    player.data.points = Convert.ToInt32(playerDataTable.Rows[0]["POINTS"]);
                }
                var countryTable = _dataLayer.Query("SELECT * FROM COUNTRIES WHERE ID=@COUNTRYID", new P("COUNTRYID", countryId));
                player.country = new Country();
                if(countryTable.Rows.Count > 0)
                {
                    player.country.code = countryTable.Rows[0]["CODE"]?.ToString() ?? string.Empty; 
                    player.country.picture = countryTable.Rows[0]["PICTURE"]?.ToString() ?? string.Empty;
                }
                var playerResultTable = _dataLayer.Query("SELECT  * FROM LAST_RESULTS WHERE PLAYER_ID=@ID", new P("ID", id));
                foreach (var row in playerResultTable.Rows)
                {
                    var data = (row as IDictionary<string, object>);
                    var results = new List<int>();
                    if (data != null)
                    {
                        results.Add(Convert.ToInt32(data["RESULT"]));
                    }
                    player.data.last = results;
                }
                return player;
            }
        }
        public IEnumerable<Player> GetAll()
        {
            if (_dataLayer == null)
            {
                var filename = "players.json";
                var json = File.ReadAllText(filename);
                var players = System.Text.Json.JsonSerializer.Deserialize<PlayersList>(json);
                if (players == null || players?.players == null || players?.players?.Count == 0)
                {
                    throw new InvalidOperationException($"Impossible de lire le fichier json {filename}");
                }
                else
                {
                    if (players != null)
                    {
                        foreach (var player in players.players.OrderBy(p => p.data.rank))
                        {
                            yield return player;
                        }
                    }
                }
            }
            else
            {
                var playerIdsTable = _dataLayer.Query("SELECT ID FROM PLAYERS");
                if (playerIdsTable != null)
                {
                    foreach (var row in playerIdsTable.Rows)
                    {
                        var data = row as DataRow;
                        if(data != null)
                        { 
                            var playerId = Convert.ToInt32(data?.ItemArray[0]);
                            var player = GetById(playerId);
                            if(player != null)
                            {
                                yield return player;
                            }
                        }
                    }
                }
            }
        }
        public Statistics GetStatistics()
        {
            if (_dataLayer == null)
            {
                var stats = new Statistics();
                var players = GetAll().ToList();
                var countriesWithWins = players.GroupBy(p => p.country.code)
                                            .Select(g => new { Country = g.Key, Wins = g.Sum(p => p.data.last.Count(r => r == 1)), Total = g.Sum(p => p.data.last.Count()) })
                                            .OrderByDescending(g => g.Wins / g.Total);
                stats.CountryWithMostWins = countriesWithWins.FirstOrDefault()?.Country ?? "N/A";
                stats.MeanWeightRatio = players.Average(p => p.data.WeightRatio());
                stats.MedianeHeight = players.OrderBy(p => p.data.height).ToArray()[(players.Count - 1) / 2].data.height;
                return stats;
            }else
            {
                var stats = new Statistics();
                var statisticsTable = _dataLayer.Query("EXEC PlayerStats()");
                if(statisticsTable != null && statisticsTable.Rows.Count == 1)
                {
                    stats.CountryWithMostWins = statisticsTable.Rows[0]["CountryWithMostWins"]?.ToString();
                    stats.MeanWeightRatio = Convert.ToDouble(statisticsTable.Rows[0]["MeanWeightRatio"]);
                    stats.MedianeHeight = Convert.ToDouble(statisticsTable.Rows[0]["MedianeHeight"]);
                }
                return stats;
            }
        }
    }
}
