using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common.Services
{
    public interface IAssetsComparer
    {
        Task CompareAsync(CancellationToken cancellationToken = default);
    }
}
