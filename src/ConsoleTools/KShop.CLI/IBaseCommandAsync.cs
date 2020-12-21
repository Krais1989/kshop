using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketCLI
{
    public interface IBaseCommandAsync
    {
        Task ExecuteAsync(CancellationToken token);
    }
}
