using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Models
{
    public abstract class Enumeration<TEnumeration> : IComparable where TEnumeration : Enumeration<TEnumeration>
    {
        public string Name { get; }

        public string Description { get; }

        public int Id { get; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        protected Enumeration(int id, string name, string description) =>
            (Id, Name, Description) = (id, name, description);

        public override string ToString() => Name;

        private static IEnumerable<TEnumeration> GetAllOptions() =>
            typeof(TEnumeration).GetFields(BindingFlags.Public |
                                           BindingFlags.Static |
                                           BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<TEnumeration>();

        private static readonly Lazy<IEnumerable<TEnumeration>> Options =
            new Lazy<IEnumerable<TEnumeration>>(GetAllOptions);

        public override bool Equals(object obj)
        {
            var otherValue = obj as TEnumeration;
            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();


        public static TEnumeration FromValue(int value)
        {
            var matchingItem = Parse(item => item.Id == value);
            return matchingItem;
        }

        public static TEnumeration FromName(string name, bool ignoreCase = false)
        {
            var matchingItem =
                !ignoreCase
                    ? Parse(item => string.Equals(item.Name, name))
                    : Parse(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
            return matchingItem;
        }

        private static TEnumeration Parse(Func<TEnumeration, bool> predicate)
        {
            var matchingItem = Options.Value.FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"{typeof(TEnumeration)} option not found");

            return matchingItem;
        }

        public int CompareTo(object other) => Id.CompareTo(((TEnumeration)other).Id);

        public static bool operator ==(Enumeration<TEnumeration> left, Enumeration<TEnumeration> right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(Enumeration<TEnumeration> left, Enumeration<TEnumeration> right) =>
            !(left == right);

        public static implicit operator int(Enumeration<TEnumeration> e)
        {
            return e.Id;
        }

        public static implicit operator OptionSetValue(Enumeration<TEnumeration> e)
        {
            return e != null ? new OptionSetValue(e.Id) : null;
        }

        public static explicit operator Enumeration<TEnumeration>(int value) =>
            FromValue(value);

        public static explicit operator Enumeration<TEnumeration>(OptionSetValue optionSetValue) =>
            FromValue(optionSetValue.Value);
    }
}