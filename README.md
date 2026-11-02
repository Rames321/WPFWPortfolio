Mijn werkwijze gaat als volgt zijn.

Ik ga een 
Index.html
Projecten.html
blog.html
README.md
CSS/
    style.css
Images/
    Projecten/
    Blog/

DIt zal voornu opdracht 1 mijn begin zijn.

Voor opdracht 3
Heb ik de volgende keuzes gemaakt:
.net versie 10
ASP.NET web API met controllers
SWAGGER

- Ik ga controllers gebruiken 
omdat ik dit vorig jaar heb gehad en de lagenmodel hier goed op aansluiten. Bij controllers staat alles netjes gegroepeerd en niet klat onderelkaar zoals minimal api. En bij de controllers is de validatie automatisch terwijl bij minimal api je dat zelf moet toevoegen. 


- Ik kies voor migratie en niet voor ensurecreated 
omdat bij ensurecreate de database bij een nieuwveld alles verwijderd en opnieuw erin doet. Terwijl bij migratie je alle data bewaart en bij een nieuw veld hij de data houd maar de nieuwe erbij voegt. De vaste waardes bij migratie herinnnert die waardoor het niet verwijderd word. Dit is veel beter omdat ik dan niet alle blogposts en projects kwijt ben bij een nieuw veld.


- En nu beginnen aan de API bouwen.

- Hoe zou je migratie kunnen gebruiken: stel je doet kolum toevoegen in project.cs dan doe je 
dotnet ef migrations add (verzinnaam)
dotnet ef database update

- uitleg van de backend:
Wanneer persoon bij swagger iets aanklikt gaat het eerst naar de controller.
De controller stuurt de melding door naar de service en van service naar Data en dan naar de DTO / SQL server. 

- Mijn lagen:
 /Controllers: Hier komen de handelingen binnen en ook weer naar buiten met codes bv 404
 /Services: Hier zit de logica is de schakel tussen de database en client
 /Data: Hier word er contact gemaakt met de SQL server om alles duidelijk door tegeven
 /Models: Zijn de tabellen in de SQL server
 /Dto's: CRUD word hier gedaan per type dto
 /Migraties: Hier worden migraties en versies bijgehouden 
 /program.cs: Hier word alles geactiveerd bv swagger.

 - Hoe starten
 Stap 1: 
 Start de docker SQL server. Ik gebruik migraties dus de data blijft bestaan.

 Stap 2:
 cd backend/Api
dotnet user-secrets set "ConnectionStrings:Default" 'Server=localhost,1433;Database=PortfolioDb;User Id=sa;Password=WACHTWOORD;TrustServerCertificate=True'
Daarna vul je dat in terminal in met je docker wachtwoord

Stap 3: 
dotnet ef database update (Word de database aangemaakt)

Stap 4:
Dotnet run
http://localhost:5064/swagger Kun je dan openen en is het klaar.

- Nieuwe veld toevoegen:
1.Veld toevoegen in models
2.`dotnet ef migrations add (Naam)
3.`dotnet ef database update
4.Het veld ook toevoegen aan de DTO's en de mapping in de service

