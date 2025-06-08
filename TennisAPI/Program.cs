using Microsoft.Extensions.DependencyInjection;
using TennisAPI;
using TennisAPI.BusinessLayer;
using TennisAPI.DataLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var conf = builder.Configuration;
var options = builder.Configuration.GetSection("Options").Get<Options>() ?? throw new InvalidOperationException("La section options n'est pas pr�sente dans le fichier appsettings.json.");
if (options.UseDataBase.HasValue && options.UseDataBase.Value)
{
    try
    {
        var connection = conf.GetSection("ConnectionStrings").GetChildren().ToDictionary(c => c.Key, c => c.Value)
            .Where(p => p.Key == options.ConnectionString).Select(p => p.Value).FirstOrDefault();
        if (connection != null)
        {
            var _playerDepot = new PlayerDepot(new DataLayer(connection));
            if (_playerDepot != null)
            {
                builder.Services.AddSingleton<IPlayerDepot, PlayerDepot>((x) => _playerDepot);
            }
        }
        else
        {
            throw new Exception("La cha�ne de connexion n'a pas �t� trouv�e dans le fichier appsettings.json.");
        }
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("Impossible de cr�er le d�p�t de joueurs avec la base de donn�es.", ex);
    }
}
else
{
    builder.Services.AddSingleton<IPlayerDepot, PlayerDepot>();
}

var app = builder.Build();

    app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
