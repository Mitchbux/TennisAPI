# Pour installer la base de donnée

<p>Créer une base de donnée PostgreSQL tennisapi<br/>
Lancer le script de creation **DataLayer/Create.sql**<br/>
N'oubliez pas de renseigner les identifiants dans le connectionString : user-id et password</p>

# Si vous ne souhaitez pas utiliser PostGreSQL

Définissez **UseDatabase : false** dans appsettings.json au niveau de la section **Options**

# Pour lancer l'API 

utilisez Visual studio puis appuyer sur **debugger (IIS Express)**

# Par ligne de commande (linux) 
    git clone https://github.com/Mitchbux/TennisAPI.git
    dotnet restore TennisAPI/TennisAPI
    dotnet run --project TennisApi/TennisAPI
