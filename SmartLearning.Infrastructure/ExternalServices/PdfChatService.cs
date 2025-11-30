
namespace SmartLearning.Infrastructure.ExternalServices
{
    public class PdfChatService : IPdfChatService
    {
        public async Task<string> ExtractTextAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            return await Task.Run(() =>
            {
                var sb = new StringBuilder();

                using var stream = file.OpenReadStream();
                using var reader = new PdfReader(stream);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    sb.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return sb.ToString();
            });
        }
    }
}
