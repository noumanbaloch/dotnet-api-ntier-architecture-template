using Breeze.DbCore.GenericRepository;
using Dapper;
using System.Data;

namespace Breeze.DbCore.UnitOfWork;

public interface IUnitOfWork
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    Task<int> CommitAsync();
}