using System;
using System.Threading;
using System.Threading.Tasks;

namespace JCTest.Interfaces
{
    public interface IMovieListUpdateService
    {
        void FetchAndSave(int year, Hangfire.IJobCancellationToken cancellationToken);
    }
}