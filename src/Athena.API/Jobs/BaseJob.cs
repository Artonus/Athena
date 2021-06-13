using System;
using System.Threading.Tasks;
using Quartz;

namespace Athena.API.Jobs
{
    public abstract class BaseJob : IJob
    {
        public BaseJob()
        {
            
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await ExecuteJobAsync(context);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                Console.WriteLine(ex);
                throw;
            }
        }

        protected abstract Task ExecuteJobAsync(IJobExecutionContext context);
    }
}