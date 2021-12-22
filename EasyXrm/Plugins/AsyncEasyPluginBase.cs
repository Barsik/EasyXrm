using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Plugins
{
    public abstract class AsyncEasyPluginBase : IPlugin
    {
        void IPlugin.Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var watch = new Stopwatch();
            watch.Start();
            var localContext = new EasyPluginContext(serviceProvider);

            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                ExecuteAsync(localContext, token).Wait(token);
            }
            catch (Exception exception)
            {
                localContext.Trace($"Exception in {GetType()}: {exception}");
                throw;
            }
            finally
            {
                watch.Stop();
                localContext.Trace($"{GetType()} execution time: {watch.ElapsedMilliseconds} ms");
            }
        }

        public abstract Task ExecuteAsync(EasyPluginContext context, CancellationToken cancellationToken);
    }
}