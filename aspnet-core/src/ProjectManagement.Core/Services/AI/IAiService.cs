using System.Threading;
using System.Threading.Tasks;

namespace ProjectManagement.Services.AI
{
    public interface IAiService
    {
        Task<string> GenerateAsync(
            string instructions,
            string inputText,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GenerateTextAsync(
            string instructions,
            string inputText,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
