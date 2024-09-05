using Domain.Entities;
using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository TodoRepository { get; }
        Task<int> CompleteAsync();

    }
}
