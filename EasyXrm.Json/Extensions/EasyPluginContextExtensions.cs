using EasyXrm.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EasyXrm.Extensions
{
    public static class EasyPluginContextExtensions
    {
       public static TValue GetJsonInputParameter<TValue>(this EasyPluginContext easyPluginContext, string name, TValue defaultValue = default)
        {
            if (easyPluginContext.PluginExecutionContext.InputParameters.ContainsKey(name))
            {
                var value = (string)easyPluginContext.PluginExecutionContext.InputParameters[name];
                if (value != null)
                    return JsonConvert.DeserializeObject<TValue>(value,
                        new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            }

            return defaultValue;
        }


        public static void SetJsonOutputParameter(this EasyPluginContext easyPluginContext,string name, object value)
        {
            if (value != null)
            {
                easyPluginContext.PluginExecutionContext.OutputParameters[name] =
                    JsonConvert.SerializeObject(value,
                        new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            }
        }
    }
}
