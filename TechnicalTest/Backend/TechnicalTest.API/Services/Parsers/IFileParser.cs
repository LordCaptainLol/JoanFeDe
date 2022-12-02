namespace TechnicalTest.API.Services.Parsers
{
    public interface IFileParser<T>
    {
        List<T> ParseFromFile(IFormFile formFile);
    }
}
