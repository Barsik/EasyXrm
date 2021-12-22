using System;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Plugins
{
    public abstract class EasyPluginBase : IPlugin
    {
        void IPlugin.Execute(IServiceProvider serviceProvider)
        {
            serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            var watch = new Stopwatch();
            watch.Start();
            var localContext = new EasyPluginContext(serviceProvider);

            try
            {
                Execute(localContext);
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

        public abstract void Execute(EasyPluginContext context);
    }
}