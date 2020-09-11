using System.Text.Json;

namespace MessagingService.Model
{
    public class SnsOptions
    {
        public string TopicArn { get; set; }
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}