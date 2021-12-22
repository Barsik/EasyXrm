using System;
using System.Activities;
using System.Diagnostics;

namespace EasyXrm.Workflows
{
    public abstract class EasyWorkflowBase : CodeActivity
    {
        protected sealed override void Execute(CodeActivityContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var watch = new Stopwatch();
            watch.Start();
            var localContext = new EasyWorkflowContext(context);

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

        public abstract void Execute(EasyWorkflowContext context);
    }
}