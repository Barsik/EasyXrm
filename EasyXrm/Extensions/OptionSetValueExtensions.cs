using EasyXrm.Models;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class OptionSetValueExtensions
    {
        public static TEnumeration ToEnumeration<TEnumeration>(this OptionSetValue optionSetValue)
            where TEnumeration : Enumeration<TEnumeration>
        {
            return Enumeration<TEnumeration>.FromValue(optionSetValue.Value);
        }
    }
}