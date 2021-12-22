using EasyXrm.Plugins;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class EasyPluginContextExtensions
    {
        public static TEntity GetTargetEntity<TEntity>(this EasyPluginContext easyPluginContext,
            object value)
            where TEntity : Entity => easyPluginContext.TargetEntity?.ToEntity<TEntity>();

        public static TEntity GetPreImage<TEntity>(this EasyPluginContext easyPluginContext)
            where TEntity : Entity => easyPluginContext.PreImage?.ToEntity<TEntity>();

        public static TEntity GetPostImage<TEntity>(this EasyPluginContext easyPluginContext)
            where TEntity : Entity => easyPluginContext.PostImage?.ToEntity<TEntity>();
    }
}