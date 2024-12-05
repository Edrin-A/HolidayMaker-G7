using Npgsql;
namespace HolidayMaker;

public class Actions
{
  NpgsqlDataSource _db;

  public Actions(NpgsqlDataSource db)
  {
    _db = db;
  }

  public async void ListAll() // 1. List all users
  {
    await using (var cmd = _db.CreateCommand("SELECT * FROM list_all_users"))
    await using (var reader = await cmd.ExecuteReaderAsync())
    {
      while (await reader.ReadAsync())
      {
        Console.WriteLine($"id: {reader.GetInt32(0)} \t firstname: {reader.GetString(1)} \t lastname: {reader.GetString(2)} \t email: {reader.GetString(3)} \t phone number: {reader.GetString(4)} \t birthdate: {reader.GetDateTime(5)}");
      }
    }
  }

  public async void AddNewUser(string firstname, string lastname, string email, string phone_number, DateOnly birthday) //2. Register new user
  {
    // Insert data
    await using (var cmd = _db.CreateCommand("INSERT INTO users (firstname, lastname, email, phone_number, birthdate) VALUES ($1, $2, $3, $4, $5)"))
    {
      cmd.Parameters.AddWithValue(firstname);
      cmd.Parameters.AddWithValue(lastname);
      cmd.Parameters.AddWithValue(email);
      cmd.Parameters.AddWithValue(phone_number);
      cmd.Parameters.AddWithValue(birthday);
      await cmd.ExecuteNonQueryAsync();
    }
  }


  public async Task<bool> IsRoomAvailable(int roomId, DateOnly startDate, DateOnly endDate) // 3. Search for available rooms between specified dates
  {
    // kolla om rummet är tillgänglig på mellan ett viss datum
    await using (var cmd = _db.CreateCommand("SELECT COUNT(*) FROM booked_rooms WHERE room_id = @room_id AND (start_date <= @end_date AND end_date >= @start_date)"))
    {
      cmd.Parameters.AddWithValue("@room_id", roomId);
      cmd.Parameters.AddWithValue("@start_date", startDate);
      cmd.Parameters.AddWithValue("@end_date", endDate);
      var count = await cmd.ExecuteScalarAsync();
      return Convert.ToInt32(count) == 0;
    }
  }


  public async void AddNewBooking(int user_Id, decimal total_price, int number_of_guests) //4. Register new booking
  {
    await using (var cmd = _db.CreateCommand("INSERT INTO bookings (user_id, total_price, number_of_guests) VALUES ($1, $2, $3) RETURNING id"))
    {
      cmd.Parameters.AddWithValue(user_Id);
      cmd.Parameters.AddWithValue(total_price);
      cmd.Parameters.AddWithValue(number_of_guests);

      // få det nya boknings id utskrivet
      var newBookingId = await cmd.ExecuteScalarAsync();
      Console.WriteLine($"New booking created with ID: {newBookingId}");
    }
  }

  public async void LinkBookingRoom(int booking_id, int room_Id, DateOnly startDate, DateOnly endDate) //4. Register new booking (länka bokningen till rummet)
  {
    // Insert data
    await using (var cmd = _db.CreateCommand("INSERT INTO booked_rooms (booking_id, room_id, start_date, end_date) VALUES ($1, $2, $3, $4)"))
    {
      cmd.Parameters.AddWithValue(booking_id);
      cmd.Parameters.AddWithValue(room_Id);
      cmd.Parameters.AddWithValue(startDate);
      cmd.Parameters.AddWithValue(endDate);
      await cmd.ExecuteNonQueryAsync();
    }
  }


  public async void AddGuestToBooking(int guest_booking_id, string guest_firstname, string guest_lastname, string guest_email, string guest_phone_number, DateOnly birthday) //5. Add guests to a booking
  {
    // Insert data
    await using (var cmd = _db.CreateCommand("INSERT INTO guests (booking_id, firstname, lastname, email, phone_number, birthdate) VALUES ($1, $2, $3, $4, $5, $6)"))
    {
      cmd.Parameters.AddWithValue(guest_booking_id);
      cmd.Parameters.AddWithValue(guest_firstname);
      cmd.Parameters.AddWithValue(guest_lastname);
      cmd.Parameters.AddWithValue(guest_email);
      cmd.Parameters.AddWithValue(guest_phone_number);
      cmd.Parameters.AddWithValue(birthday);
      await cmd.ExecuteNonQueryAsync();
    }
  }




  public async void addFeatureToBooking(int booking_id, int features_id) //6. Add features to booking
  {
    // Insert data
    await using (var cmd = _db.CreateCommand("INSERT INTO booking_features (booking_id, features_id) VALUES ($1, $2)"))
    {
      cmd.Parameters.AddWithValue(booking_id);
      cmd.Parameters.AddWithValue(features_id);
      await cmd.ExecuteNonQueryAsync();
    }
  }

  public async void addServiceToBooking(int booking_id, int additional_services_id) //7. Add services to booking
  {
    // Insert data
    await using (var cmd = _db.CreateCommand("INSERT INTO booked_services (booking_id, additional_services_id) VALUES ($1, $2)"))
    {
      cmd.Parameters.AddWithValue(booking_id);
      cmd.Parameters.AddWithValue(additional_services_id);
      await cmd.ExecuteNonQueryAsync();
    }
  }

  public async void UpdateBookingDetails(string bookingId, string? priceInput, string? guestsInput) // 8. Change details in a booking
  {
    await using (var cmd = _db.CreateCommand("UPDATE bookings SET total_price = $1, number_of_guests = $2 WHERE id = $3"))
    {
      // Lägg till parametrar
      cmd.Parameters.AddWithValue(decimal.Parse(priceInput ?? "0"));
      cmd.Parameters.AddWithValue(int.Parse(guestsInput ?? "0"));
      cmd.Parameters.AddWithValue(int.Parse(bookingId));

      await cmd.ExecuteNonQueryAsync();
    }
  }




  public async void GetAllPersonsInBooking(int booking_id) // 9. SHow all persons in a booking "klar"
  {
    // Prepare and execute the query from the view with parameters
    await using (var cmd = _db.CreateCommand("SELECT * FROM view_persons_booking WHERE booking_id = @booking_id"))
    {
      // Add the parameters (max distance and room type)
      cmd.Parameters.AddWithValue("@booking_id", booking_id);


      // Execute the query and process the results
      await using (var reader = await cmd.ExecuteReaderAsync())
      {
        bool found = false;
        while (await reader.ReadAsync())
        {
          found = true;
          // Print results
          Console.WriteLine($"Booking ID: {reader.GetInt32(2)} \t Booker: {reader.GetString(0)} \t Guests: {reader.GetString(1)}");

        }
        if (!found)
        {
          Console.WriteLine("No persons matching this booking ID.");
        }
      }
    }
  }





  public async void DeleteOne(string id)  // 6. Cancel a booking
  {
    // Delete data
    await using (var cmd = _db.CreateCommand("DELETE FROM bookings WHERE id = @id"))
    {
      cmd.Parameters.AddWithValue("@id", int.Parse(id));
      await cmd.ExecuteNonQueryAsync();
    }
  }




  public async void DistanceToBeach(int maxDistanceBeach, string typeOfRoom) // 7. Search accommodations based on distance to beach
  {
    // Prepare and execute the query from the view with parameters
    await using (var cmd = _db.CreateCommand("SELECT * FROM DistanceToBeach WHERE distance_to_beach <= @maxDistanceBeach AND room_type = @typeOfRoom"))
    {
      // Add the parameters (max distance and room type)
      cmd.Parameters.AddWithValue("@maxDistanceBeach", maxDistanceBeach);
      cmd.Parameters.AddWithValue("@typeOfRoom", typeOfRoom);

      // Execute the query and process the results
      await using (var reader = await cmd.ExecuteReaderAsync())
      {
        bool found = false;
        while (await reader.ReadAsync())
        {
          found = true;
          // Print results
          Console.WriteLine($"Room ID: {reader.GetInt32(0)} \t Hotel: {reader.GetString(1)} \t Room Type: {reader.GetString(2)} \t Distance to Beach: {reader.GetInt32(3)} meters \t Price per Night: {reader.GetDecimal(4)}");
          //Console.WriteLine($"Room ID: {reader.GetInt32(0),-10} Hotel: {reader.GetString(1),-25} Room Type: {reader.GetString(2),-25} Distance to Beach: {reader.GetInt32(3),-25} meters Price per Night: {reader.GetDecimal(4),-10}");
        }
        if (!found)
        {
          Console.WriteLine("No rooms found matching the criteria.");
        }
      }
    }
  }


  public async void DistanceToCenter(int maxDistanceCenter, string typeOfRoom) // 8. Search accommodations based on distance to center
  {
    // Prepare and execute the query from the view with parameters
    await using (var cmd = _db.CreateCommand("SELECT * FROM DistanceToCenter WHERE distance_to_center <= @maxDistanceCenter AND room_type = @typeOfRoom"))
    {
      // Add the parameters (max distance and room type)
      cmd.Parameters.AddWithValue("@maxDistanceCenter", maxDistanceCenter);
      cmd.Parameters.AddWithValue("@typeOfRoom", typeOfRoom);

      // Execute the query and process the results
      await using (var reader = await cmd.ExecuteReaderAsync())
      {
        bool found = false;
        while (await reader.ReadAsync())
        {
          found = true;
          // Print results
          Console.WriteLine($"Room ID: {reader.GetInt32(0)} \t Hotel: {reader.GetString(1)} \t Room Type: {reader.GetString(2)} \t Distance to Center: {reader.GetInt32(3)} meters \t Price per Night: {reader.GetDecimal(4)}");

        }
        if (!found)
        {
          Console.WriteLine("No rooms found matching the criteria.");
        }
      }
    }
  }


  public async void GetRoomsSortedByPrice() // "9. Rooms sorted by price (low to high)"); // nami
  {
    await using (var cmd = _db.CreateCommand("SELECT h.hotel_name, rt.type, r.price_per_night FROM rooms AS r LEFT JOIN hotels h ON r.hotel_id = h.id LEFT JOIN room_types rt ON r.room_type = rt.id ORDER BY r.price_per_night"))
    await using (var reader = await cmd.ExecuteReaderAsync())
    {
      while (await reader.ReadAsync())
      {
        Console.WriteLine($"Hotel: {reader.GetString(0)} \t Room Type: {reader.GetString(1)} \t Price: {reader.GetFloat(2)}");
      }
    }
  }
  public async void GetHotelsSortedByRating() // "10. Rooms sorted by rating (high to low)"); // nami
  {
    await using (var cmd = _db.CreateCommand("SELECT h.hotel_name, h.rating FROM hotels h ORDER BY h.rating DESC"))
    await using (var reader = await cmd.ExecuteReaderAsync())
    {
      while (await reader.ReadAsync())
      {
        Console.WriteLine($"Hotel: {reader.GetString(0)} \t Rating: {reader.GetFloat(1)}");
      }
    }
  }




  public async void SearchRoomsByPriceAndCity(string cityName, decimal minPrice, decimal maxPrice, string roomType) // 12. Search for all rooms in one city sorted by specific criteria
  {
    // Sanitize inputs to prevent invalid encoding issues
    // cityName = cityName?.Replace("\0", "").Trim();  // Remove null bytes and trim extra spaces
    // roomType = roomType?.Replace("\0", "").Trim(); // Remove null bytes and trim extra spaces

    await using (var cmd = _db.CreateCommand("SELECT * FROM PriceByCityAndPrice WHERE city_name = @city AND price_per_night >= @minPrice AND price_per_night <= @maxPrice AND room_type = @roomType"))
    {
      cmd.Parameters.AddWithValue("@city", cityName);
      cmd.Parameters.AddWithValue("@minPrice", minPrice);
      cmd.Parameters.AddWithValue("@maxPrice", maxPrice);
      cmd.Parameters.AddWithValue("@roomType", roomType);

      await using (var reader = await cmd.ExecuteReaderAsync())
      {
        bool found = false;
        while (await reader.ReadAsync())
        {
          found = true;
          Console.WriteLine($"Hotel: {reader.GetString(1)} \t Address: {reader.GetString(2)} \t City: {reader.GetString(3)} \t Rating: {reader.GetDecimal(4)} \t Room Type: {reader.GetString(5)} \t Price per Night: {reader.GetDecimal(6)}");
        }
        if (!found)
        {
          Console.WriteLine("No rooms found matching the criteria.");
        }
      }
    }
  }
}






