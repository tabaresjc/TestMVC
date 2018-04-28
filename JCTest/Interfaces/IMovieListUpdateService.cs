using System;
using System.Threading;
using System.Threading.Tasks;

namespace JCTest.Interfaces
{
    public interface IMovieListUpdateService
    {
        void FetchAndSave(string language, DateTime minimumDate, Hangfire.IJobCancellationToken cancellationToken);
    }
}