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
    Console.WriteLine("User related options:");
    Console.WriteLine("1. List all users"); // klar
    Console.WriteLine("2. Register new user"); // klar (edrin - try catch error för att ha varje parameter med?)

    Console.WriteLine("\n===================");
    Console.WriteLine("Bookings:");
    Console.WriteLine("3. Create new booking"); // edrin
    Console.WriteLine("4. Change details in a booking"); //  edrin kolla om man bara vill ändra 1 tex?
    Console.WriteLine("5. Show every person in a booking"); // farzad
    Console.WriteLine("6. Cancel a booking"); // klar

    Console.WriteLine("\n===================");
    Console.WriteLine("Specified searches:");
    Console.WriteLine("7. Search accommodations based on distance to beach"); // edrin (dynamisk sökning)
    Console.WriteLine("8. Search accommodations based on distance to center"); // abdel (dynamisk sökning 
    Console.WriteLine("9. Rooms sorted by price (low to high)"); // nami
    Console.WriteLine("10. Rooms sorted by rating (high to low)"); // nami
    Console.WriteLine("11. Search for available rooms between specified dates"); // edrin (dynamisk sökning) (lägg till startdate-enddate i booked_rooms)
    Console.WriteLine("12. Search for all rooms in one city sorted by specific criteria"); // abdel

    Console.WriteLine("\n===================");
    Console.WriteLine("13. Quit");
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
        case ("1"): // 1. List all users
          Console.WriteLine("Listing all");
          _actions.ListAll();
          break;



        case ("2"): // 2. Register new user
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



        case ("3"): // "3. Create new booking"
          Console.WriteLine("Enter id to show details about one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.ShowOne(id);
          }
          break;



        case ("4"): // 4. Change details in a booking
          Console.WriteLine("Enter id to update one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.UpdateOne(id);
          }
          break;



        case ("5"): // 5. Show every person in a booking
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("6"): // 6. Cancel a booking
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("7"): // 7. Search accommodations based on distance to beach ((hur långt borta får boendet max ligga ifrån en strand) GÖR EN QUERIE FÖR DETTA. Dynamisk sökning? )
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("8"): // 8. Search accommodations based on distance to center (hur långt borta får boendet max ligga ifrån ett centrum) GÖR EN QUERIE FÖR DETTA. Dynamisk sökning? )
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("9"): // 9. Rooms sorted by price (low to high)
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("10"): // 10. Rooms sorted by rating (high to low)
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("11"): // 11. Search for available rooms between specified dates
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("12"): // 12. Search for all rooms in one city sorted by specific criteria
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;



        case ("13"): // 12. quit
          Console.WriteLine("Quitting");
          Environment.Exit(0);
          break;
      }

      PrintMenu();
    }

  }

}