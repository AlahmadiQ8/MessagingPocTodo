using System.Text.Json;
using System.Threading.Tasks;

namespace MessagingService.Model
{
    public class SqsOptions
    {
        public string QueueUrl { get; set; }
    }
}