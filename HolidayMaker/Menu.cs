namespace HolidayMaker;

public class Menu
{
  Actions _actions;
  public Menu(Actions actions)
  {
    // constructorn tar emot actions 
    _actions = actions;
    // och startar menyn
    PrintMenu();
  }

  private void PrintMenu()
  {
    // skriver ut menyn i konsolen
    Console.WriteLine("===================");
    Console.WriteLine("User related options");
    Console.WriteLine("1. List all users");
    Console.WriteLine("2. Register new user");

    Console.WriteLine("===================");
    Console.WriteLine("Bookings");
    Console.WriteLine("3. Create new booking");
    Console.WriteLine("4. Change details in a booking");
    Console.WriteLine("5. Show every person in a booking");
    Console.WriteLine("6. Cancel a booking");

    Console.WriteLine("===================");
    Console.WriteLine("Specified searches");
    Console.WriteLine("7. Search accommodations based on distance to beach");
    Console.WriteLine("8. Search accommodations based on distance to center");
    Console.WriteLine("9. Rooms sorted by price (low to high)");
    Console.WriteLine("10. Rooms sorted by rating (high to low)");
    Console.WriteLine("11. Search for available rooms between specified dates");

    Console.WriteLine("===================");
    Console.WriteLine("12. Quit");
    Console.WriteLine("===================");
    // lyssnar på användaren
    AskUser();
  }

  private async void AskUser()
  {
    // tar emot vad användaren skriver
    var response = Console.ReadLine();
    if (response is not null)
    {
      string? id; // define for multiple use below

      // kör olika actions beroende på vad användaren skrivit
      switch (response)
      {
        case ("1"):
          Console.WriteLine("Listing all");
          _actions.ListAll();
          break;
        case ("2"):
          Console.WriteLine("Enter id to show details about one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.ShowOne(id);
          }
          break;
        case ("3"):
          Console.WriteLine("Enter firstname");
          var firstname = Console.ReadLine(); // required

          Console.WriteLine("Enter lastname");
          var lastname = Console.ReadLine(); // required

          Console.WriteLine("Enter email");
          var email = Console.ReadLine(); // required

          Console.WriteLine("Enter phone number");
          var phone_number = Console.ReadLine(); // required

          Console.WriteLine("Enter birthday (YYYY-MM-DD)");
          var birthdayInput = Console.ReadLine(); // required

          if (firstname is not null && DateOnly.TryParse(birthdayInput, out var birthday)) // konvertera och validera
          {
            _actions.AddOne(firstname, lastname, email, phone_number, birthday);
          }
          else
          {
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
          }
          break;
        case ("4"):
          Console.WriteLine("Enter id to update one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.UpdateOne(id);
          }
          break;
        case ("5"):
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;
        case ("9"):
          Console.WriteLine("Quitting");
          Environment.Exit(0);
          break;
      }

      PrintMenu();
    }

  }

}