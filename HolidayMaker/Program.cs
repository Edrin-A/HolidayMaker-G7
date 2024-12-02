namespace HolidayMaker;

class Program
{

  static void Main(string[] args)
  {
    // anslut till databasen
    Database database = new();
    // hämta anslutningen (db) att göra queries med
    var db = database.Connection();
    // skapa actions och skicka in anslutningen, så att vi kan köra queries till databasen där
    var actions = new Actions(db);
    // skapa en meny och skicka in våra actions i den, så vi kan anropa dem
    new Menu(actions);
  }
}