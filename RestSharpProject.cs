using RestSharp;
using System.Text.Json;

Console.WriteLine("********************TEST RESULT******************");

//configure the Rest Client
var options = new RestClientOptions("https://simple-books-api.glitch.me/");

//Create a RestClient object using the options configuration
var client = new RestClient(options);

//Prepare the request and set the relevant http request method
var request = new RestRequest("books", Method.Get);

//Execute the http request using the request prepared.
var response = client.ExecuteGet(request);

// Flag to track if all books have both 'name' and 'type' properties
bool allBooksHaveProperties = true;

// Check if the request was successful using the status code then
//check the request body contains 'name' and 'type'
if (response.StatusCode == System.Net.HttpStatusCode.OK)
{
    Console.WriteLine($"Successful with STATUS CODE: {(int)response.StatusCode} - {response.StatusCode}\n");
    // Deserialize the JSON response content into a JsonObject
    var jsonResponse = JsonSerializer.Deserialize<JsonDocument>(response.Content);

    //Loop through the Json object and check for 'name' and 'type' properties
    foreach (var book in jsonResponse.RootElement.EnumerateArray())
    {
        // Ensure 'name' and 'type' properties exist for each book
        if (!book.TryGetProperty("name", out _) || book.TryGetProperty("type", out _))
        {
            // Assertion successful, continue to the next book
            Console.WriteLine("Both 'name' and 'type' properties Not found.");
            
            // If either 'name' or 'type' property is missing for any book, set the flag to false
            allBooksHaveProperties = false;

            break;

        }
    }
    if (allBooksHaveProperties)
    {
        Console.WriteLine("Both 'name' and 'type' properties found in each item in the RESPONSE body - test PASSED!");
    }
    else
    {
        Console.WriteLine("Either 'name' or 'type' property is missing in one or more items in the body - test FAILED!");
    }
}

//If the request fails, output the error message
else
{
    Console.WriteLine($"Unsuccessful with STATUS CODE: {(int)response.StatusCode} - {response.StatusCode}");
}