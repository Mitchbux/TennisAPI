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
                if(players == null || players.players == null || players.players?.Count == 0)
                {
                    throw new InvalidOperationException($"Impossible de lire le fichier json {filename}");
                }else
                    return players?.players?.Find(p => p.id == id);
            }
            else
            {
                var playerTable = _dataLayer.Query("SELECT * FROM PLAYERS WHERE ID=@ID");
                var playerDataTable = _dataLayer.Query("SELECT * FROM PLAYER_DATA WHERE PLAYER_ID=@ID");
                var countryTable = _dataLayer.Query("SELECT * FROM COUNTRIES WHERE ID=@COUNTRYID");
                var playerResultTable = _dataLayer.Query("SELECT  * FROM PLAYER_RESULTS WHERE PLAYER_ID=@ID");
                var player = new Player();
                player.data = new PlayerData();
                player.country = new Country();
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
            if(_dataLayer == null)
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
                        var data = (row as IDictionary<string, object>);
                        if (data != null && data.ContainsKey("ID"))
                        {
                            var playerId = Convert.ToInt32(data["ID"]);
                            yield return GetById(playerId);
                        }
                    }
                }
            }
        }
    }
}
