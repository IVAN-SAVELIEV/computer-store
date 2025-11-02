using ComputerStoreClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStoreClean.Domain.Common
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<int> GetProductCountAsync(int categoryId);
    }
}
