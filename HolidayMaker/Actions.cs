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
    await using (var cmd = _db.CreateCommand("SELECT * FROM users"))
    await using (var reader = await cmd.ExecuteReaderAsync())
    {
      while (await reader.ReadAsync())
      {
        Console.WriteLine($"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)}");
      }
    }
  }

  public async void AddOne(string firstname, string lastname, string email, string phone_number, DateOnly birthday) //2. Register new user
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
 
  
  public async void ShowOne(string id) //ignorera denna
  {
    await using (var cmd = _db.CreateCommand("SELECT * FROM items WHERE id = $1")) //where distance is <= dollarsign (skicka in distance som en inparameter)
    {
      cmd.Parameters.AddWithValue(int.Parse(id));
      await using (var reader = await cmd.ExecuteReaderAsync())
      {
        while (await reader.ReadAsync())
        {
          Console.WriteLine($"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)} \t slogan: {reader.GetString(2)}");
        }
      }
    }
  }


  public async void UpdateOne(string id)
  {
    Console.WriteLine("Current entry:");
    ShowOne(id);
    Console.WriteLine("Enter updated name (required)");
    var name = Console.ReadLine(); // required
    Console.WriteLine("Enter updated slogan");
    var slogan = Console.ReadLine(); // not required
    if (name is not null)
    {
      // Update data
      await using (var cmd = _db.CreateCommand("UPDATE items SET name = $2, slogan = $3 WHERE id = $1"))
      {
        cmd.Parameters.AddWithValue(int.Parse(id));
        cmd.Parameters.AddWithValue(name);
        cmd.Parameters.AddWithValue(slogan);
        await cmd.ExecuteNonQueryAsync();
      }

    }
  }

  public async void DeleteOne(string id)  // 6. Cancel a booking
  {
    // Delete data
    await using (var cmd = _db.CreateCommand("DELETE FROM items WHERE id = $1"))
    {
      cmd.Parameters.AddWithValue(int.Parse(id));
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



  

  
