namespace TechnicalTest.API.Services.Parsers
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JsonFileParser<T> : IJsonFileParser<T>
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,            
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        public List<T> ParseFromFile(IFormFile formFile)
        {
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                var file = reader.ReadToEnd();

                return JsonSerializer.Deserialize<List<T>>(file, _jsonSerializerOptions);
            }
        }
    }
}
