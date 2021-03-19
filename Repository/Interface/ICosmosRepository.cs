using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace RocketAnt.Repository
{
    public interface ICosmosRepository<T>
    {
        Task<ItemResponse<T>> CreateOrUpdate(T entity);
        Task<List<T>> GetAll();
        Task<List<T>> GetLatest(int count);
        Task RemoveAll();
    }
}