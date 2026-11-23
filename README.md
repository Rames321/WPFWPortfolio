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

------------

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

-------------

Opdracht 4

- Rechten: 
Ik heb gast editor en admin gast mag GET alleen editor en admin alles.
Nieuwe gebruiker krijgt gast.

Hoe start zie hierboven opdracht 3.

Hoe maak je editor of admin?
- Eerst maak je een post via register min 6 letters wachtwoord

- Ga naar DBeaver  voer deze query's uit.

- (alle id's zien)
SELECT Id, Email FROM AspNetUsers;

- (Id betekenissen)
Geef de rol (`'2'` = Editor, `'3'` = Admin):

- (Admin/editor maken)
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('ID_VAN_DE_GEBRUIKER', '3');

- (editor/admin verwijderen)
DELETE FROM AspNetUserRoles
WHERE UserId = 'ID_VANGEBRUIKER'
  AND RoleId = '3';


- Opnieuw inloggen en dan is het goed

Ik heb de coverage gemeten met coverlet via dotnet test. Ik tel de API-code mee,
maar niet de gegenereerde migraties, omdat die geen eigen logica bevatten.

Resultaat: 98,6% van de regels en 92,5% van de takken.

Alle CRUD-routes, controllers en de project- en blogpostservices zijn volledig gedekt.
Niet gedekt zijn drie veiligheidspaden in AuthService:
- het terugdraaien van een account als het toekennen van de rol Gast mislukt;
- een gebruiker die niet gevonden wordt in GetByEmailAsync;
- een gebruiker zonder e-mailadres.

Deze paden zijn via een normaal HTTP-verzoek niet te bereiken. Ze vangen alleen
onverwachte fouten op, dus ik heb er geen kunstmatige tests voor geschreven.

Een hoge coverage bewijst niet dat er geen fouten zijn. Daarom controleren mijn tests
ook de response, de data in de database en geweigerde verzoeken.

Migratie:
dotnet ef migrations add AddIdentity

dotnet ef database update

Tests:
- dotnet test backend/Portfolio.slnx


- rm -rf /tmp/portfolio-coverage

dotnet test Portfolio.slnx --collect:"XPlat Code Coverage" --results-directory /tmp/portfolio-coverage -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.ExcludeByFile="**/Migrations/*.cs"

reportgenerator -reports:"/tmp/portfolio-coverage/*/coverage.cobertura.xml" -targetdir:/tmp/portfolio-coverage/report -reporttypes:Html

open /tmp/portfolio-coverage/report/index.html