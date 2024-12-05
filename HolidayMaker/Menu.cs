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
    Console.WriteLine("1. List all users");
    Console.WriteLine("2. Register new user");
    Console.WriteLine("===================");

    Console.WriteLine("\n===================");
    Console.WriteLine("Bookings:");
    Console.WriteLine("3. Search for available rooms between specified dates");
    Console.WriteLine("4. Create new booking");
    Console.WriteLine("5. Add guests to a booking");
    Console.WriteLine("6. Add features to booking");
    Console.WriteLine("7. Add services to booking");
    Console.WriteLine("8. Change details in a booking");
    Console.WriteLine("9. Show every person in a booking");
    Console.WriteLine("10. Cancel a booking");
    Console.WriteLine("===================");

    Console.WriteLine("\n===================");
    Console.WriteLine("Specified searches:");
    Console.WriteLine("11. Search accommodations based on distance to beach");
    Console.WriteLine("12. Search accommodations based on distance to center");
    Console.WriteLine("13. Rooms sorted by price (low to high)");
    Console.WriteLine("14. Hotels sorted by rating (high to low)");
    Console.WriteLine("15. Search for all rooms in one city sorted by specific criteria");
    Console.WriteLine("===================");

    Console.WriteLine("\n===================");
    Console.WriteLine("16. Quit");
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
          Console.WriteLine("Listing all users");
          _actions.ListAll();
          break;



        case ("2"): // 2. Register new user
          Console.WriteLine("Enter firstname:");
          var firstname = Console.ReadLine();

          Console.WriteLine("Enter lastname:");
          var lastname = Console.ReadLine();

          Console.WriteLine("Enter email:");
          var email = Console.ReadLine();

          Console.WriteLine("Enter phone number:");
          var phone_number = Console.ReadLine();

          Console.WriteLine("Enter birthday (YYYY-MM-DD):");
          var birthdayInput = Console.ReadLine();

          if (!string.IsNullOrWhiteSpace(firstname) &&
              !string.IsNullOrWhiteSpace(lastname) &&
              !string.IsNullOrWhiteSpace(email) &&
              !string.IsNullOrWhiteSpace(phone_number) &&
              DateOnly.TryParse(birthdayInput, out var birthday))
          {
            // lägg till new user
            _actions.AddNewUser(firstname, lastname, email, phone_number, birthday);

            // säg att det gick och visa vad man har skrivit in
            Console.WriteLine("\nUser registered successfully!");
            Console.WriteLine("Here are the details you entered:");
            Console.WriteLine($"Firstname: {firstname}");
            Console.WriteLine($"Lastname: {lastname}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Phone Number: {phone_number}");
            Console.WriteLine($"Birthday: {birthday:yyyy-MM-dd}\n");
          }
          else
          {
            Console.WriteLine("\nAll fields must be filled in and date must be in YYYY-MM-DD format.");
          }
          break;



        case ("3"): // 3. Search for available rooms between specified dates
          Console.WriteLine("Enter room id:");
          var roomId = Console.ReadLine();
          Console.WriteLine("Enter start date (YYYY-MM-DD):");
          var startDateInput = Console.ReadLine();
          Console.WriteLine("Enter end date (YYYY-MM-DD):");
          var endDateInput = Console.ReadLine();

          if (int.TryParse(roomId, out var roomIdParsed) &&
              DateOnly.TryParse(startDateInput, out var startDate) &&
              DateOnly.TryParse(endDateInput, out var endDate))
          {
            bool isAvailable = _actions.IsRoomAvailable(roomIdParsed, startDate, endDate).GetAwaiter().GetResult();

            if (isAvailable)
            {
              Console.WriteLine("This room is available for your requested dates.");
            }
            else
            {
              Console.WriteLine("This room is not available for your requested dates.");
            }
          }
          else
          {
            Console.WriteLine("Invalid input. Please try again.");
          }
          break;

        case ("4"): // 4. Create new booking
          Console.WriteLine("Enter user id:");
          var user_Id = Console.ReadLine();


          Console.WriteLine("total price:"); //???
          var total_price = Console.ReadLine();


          Console.WriteLine("Enter number of guests:");
          var number_of_guests = Console.ReadLine();

          if (int.TryParse(user_Id, out var userId) &&
              decimal.TryParse(total_price, out var price) &&
              int.TryParse(number_of_guests, out var guests))
          {
            _actions.AddNewBooking(userId, price, guests);
            Console.WriteLine("\nBooking registered successfully!");
            //Console.WriteLine($"Here is the booking id: {booking_id}"); //hämta booking id som skapas från databasen?
          }
          else
          {
            Console.WriteLine("\nAll fields must be filled in");
          }



          // länka bookningen till rummet
          Console.WriteLine("\nYou are now going to link the booking to the room:");
          Console.WriteLine("\nEnter booking id:");
          var booking_Id = Console.ReadLine();

          Console.WriteLine("Enter room id:");
          roomId = Console.ReadLine();

          Console.WriteLine("Enter start date (YYYY-MM-DD):");
          var inputStartDate = Console.ReadLine();

          Console.WriteLine("Enter end date (YYYY-MM-DD):");
          var inputBookingendDate = Console.ReadLine();


          if (int.TryParse(booking_Id, out var booking_id) &&
                  int.TryParse(roomId, out var room_Id) &&
                  DateOnly.TryParse(inputStartDate, out var bookingStartDate) &&
                  DateOnly.TryParse(inputBookingendDate, out var bookingEndDate))
          {
            _actions.LinkBookingRoom(booking_id, room_Id, bookingStartDate, bookingEndDate);
            Console.WriteLine("\nBooking registered successfully!");
            //Console.WriteLine($"Here is the booking id: {booking_id}"); //hämta booking id som skapas från databasen?
          }
          else
          {
            Console.WriteLine("\nAll fields must be filled in");
          }
          break;


        case ("5"): //5. Add guests to a booking
          Console.WriteLine("Enter booking id:");
          var guest_booking_id = Console.ReadLine();

          Console.WriteLine("Enter guest firstname:");
          var guest_firstname = Console.ReadLine();

          Console.WriteLine("Enter guest lastname:");
          var guest_lastname = Console.ReadLine();

          Console.WriteLine("Enter guest email:");
          var guest_email = Console.ReadLine();

          Console.WriteLine("Enter guest phone number:");
          var guest_phone_number = Console.ReadLine();

          Console.WriteLine("Enter guest birthday (YYYY-MM-DD):");
          var guest_birthdayInput = Console.ReadLine();

          if (int.TryParse(guest_booking_id, out var booking_ID) &&
              !string.IsNullOrWhiteSpace(guest_firstname) &&
              !string.IsNullOrWhiteSpace(guest_lastname) &&
              !string.IsNullOrWhiteSpace(guest_email) &&
              !string.IsNullOrWhiteSpace(guest_phone_number) &&
              DateOnly.TryParse(guest_birthdayInput, out var guest_birthday))
          {
            // lägg till new user
            _actions.AddGuestToBooking(booking_ID, guest_firstname, guest_lastname, guest_email, guest_phone_number, guest_birthday);

            // säg att det gick och visa vad man har skrivit in
            Console.WriteLine("\nGuest registered successfully!");
            Console.WriteLine("Here are the details you entered:");
            Console.WriteLine($"booking id: {booking_ID}");
            Console.WriteLine($"Firstname: {guest_firstname}");
            Console.WriteLine($"Lastname: {guest_lastname}");
            Console.WriteLine($"Email: {guest_email}");
            Console.WriteLine($"Phone Number: {guest_phone_number}");
            Console.WriteLine($"Birthday: {guest_birthday:yyyy-MM-dd}\n");
          }
          else
          {
            Console.WriteLine("\nAll fields must be filled in and date must be in YYYY-MM-DD format.");
          }
          break;

        case ("6"): //6. Add features to booking
          Console.WriteLine("\nEnter booking id:");
          booking_Id = Console.ReadLine();

          Console.WriteLine("\nEnter the id of the feature you want to have (1 - Pool, 2 - Evening Entertainment, 3 - Kids Club, 4 - Restaurant):");
          var feature_Id = Console.ReadLine();


          if (int.TryParse(booking_Id, out booking_id) &&
                  int.TryParse(feature_Id, out var feature_id))

          {
            _actions.addFeatureToBooking(booking_id, feature_id);
            Console.WriteLine("features registered successfully!");
          }
          else
          {
            Console.WriteLine("\nAll fields must be filled in");
          }
          break;


        case ("7"): //6. Add services to booking
          Console.WriteLine("\nEnter booking id:");
          booking_Id = Console.ReadLine();

          Console.WriteLine("\nEnter the id of the additional service you want to have (1 - Extra Bed, 2 - Half Board, 3 - Full Board, 4 - all-inclusive):");
          var additional_services_id = Console.ReadLine();


          if (int.TryParse(booking_Id, out booking_id) &&
                  int.TryParse(additional_services_id, out var service_id))

          {
            _actions.addServiceToBooking(booking_id, service_id);
            Console.WriteLine("Additional services registered successfully!");
          }
          else
          {
            Console.WriteLine("\nAll fields must be filled in");
          }
          break;

        case ("8"): // 8. Change details in a booking
          Console.WriteLine("Enter booking ID:");
          var bookingId = Console.ReadLine();
          if (bookingId is null) break;

          Console.WriteLine("Enter new total price (or press enter to skip):");
          var priceInput = Console.ReadLine();
          Console.WriteLine("Enter new number of guests (or press enter to skip):");
          var guestsInput = Console.ReadLine();

          if (!string.IsNullOrWhiteSpace(priceInput) || !string.IsNullOrWhiteSpace(guestsInput))
          {
            _actions.UpdateBookingDetails(bookingId, priceInput, guestsInput);
            Console.WriteLine("Update completed successfully!");
          }
          break;


        case ("9"):// Show every person in a booking
          Console.WriteLine("Enter booking ID to show details:");
          if (int.TryParse(Console.ReadLine(), out booking_id))
          {
            _actions.GetAllPersonsInBooking(booking_id); // Kör metoden i Actions
          }
          else
          {
            Console.WriteLine("Invalid booking ID. Please enter a valid number.");
          }
          break;



        case ("10"): // 10. Cancel a booking
          Console.WriteLine("Enter id to delete one");
          id = Console.ReadLine();
          if (id is not null)
          {
            _actions.DeleteOne(id);
          }
          break;






        case "11":
          // Ask for the max distance and room type in Menu.cs
          Console.WriteLine("Enter the maximum distance to the beach (in meters): ");
          if (int.TryParse(Console.ReadLine(), out int maxDistanceBeach))
          {
            Console.WriteLine("Enter the room type (e.g., Single, Double, Suite): ");
            string typeOfRoom = Console.ReadLine();

            // Call the method in Actions.cs with the parameters
            _actions.DistanceToBeach(maxDistanceBeach, typeOfRoom);
          }
          else
          {
            Console.WriteLine("Invalid input for max distance.");
          }
          break;




        case "12":
          // Ask for the max distance and room type in Menu.cs
          Console.WriteLine("Enter the maximum distance to the center (in meters): ");
          if (int.TryParse(Console.ReadLine(), out int maxDistanceCenter))
          {
            Console.WriteLine("Enter the room type (e.g., Single, Double, Suite): ");
            string typeOfRoom = Console.ReadLine();

            // Call the method in Actions.cs with the parameters
            _actions.DistanceToCenter(maxDistanceCenter, typeOfRoom);
          }
          else
          {
            Console.WriteLine("Invalid input for max distance.");
          }
          break;



        case ("13"): // 9. Rooms sorted by price (low to high)
          _actions.GetRoomsSortedByPrice();
          break;


        case ("14"): // 10. Hotels sorted by rating (high to low)
          _actions.GetHotelsSortedByRating();
          break;


        case "15":
          // Ask for the city name, price range, and room type
          Console.WriteLine("Enter the city name: ");
          string cityName = Console.ReadLine();

          Console.WriteLine("Enter the minimum price you can pay: ");
          if (decimal.TryParse(Console.ReadLine(), out decimal minPrice))
          {
            Console.WriteLine("Enter the maximum price you can pay: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
            {
              Console.WriteLine("Enter the type of room (Single, Double, Family, Suite): ");
              string roomType = Console.ReadLine();

              // Call the method in Actions.cs with the parameters
              _actions.SearchRoomsByPriceAndCity(cityName, minPrice, maxPrice, roomType);
            }
            else
            {
              Console.WriteLine("Invalid input for maximum price.");
            }
          }
          else
          {
            Console.WriteLine("Invalid input for minimum price.");
          }
          break;




        case ("16"): // 12. quit
          Console.WriteLine("Quitting");
          Environment.Exit(0);
          break;
      }

      PrintMenu();
    }

  }

}