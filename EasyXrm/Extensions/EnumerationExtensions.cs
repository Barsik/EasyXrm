using System.Collections.Generic;
using System.Linq;
using EasyXrm.Models;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class EnumerationExtensions
    {
        public static OptionSetValueCollection ToOptionSetValueCollection<TEnumeration>(
            this IEnumerable<TEnumeration> enums) where TEnumeration : Enumeration<TEnumeration>
        {
            return enums == null
                ? null
                : new OptionSetValueCollection(enums.Select(e => new OptionSetValue(e.Id)).ToArray());
        }

        public static bool EqualsToOptionSetValue<TEnumeration>(this TEnumeration enumeration,
            OptionSetValue optionSetValue) where TEnumeration : Enumeration<TEnumeration>
        {
            if (enumeration == null || optionSetValue == null) return false;

            return enumeration.Id == optionSetValue.Value;
        }
    }
}