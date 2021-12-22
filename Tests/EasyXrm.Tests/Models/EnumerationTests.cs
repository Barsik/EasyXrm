using System;
using EasyXrm.Models;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace EasyXrm.Tests.Models
{
    public class EnumerationTests
    {
        [Fact]
        public void Enumeration_WhenEquals()
        {
            var e1 = EnumA.Option1;
            var e2 = EnumA.Option1;

            Assert.True(e1 == e2);
        }

        [Fact]
        public void Enumeration_Implicit()
        {
            var enumA = EnumA.Option1;
            int i = enumA;
            OptionSetValue o = enumA;

            Assert.Equal(1, i);
            Assert.Equal(1, o.Value);
        }

        [Fact]
        public void Enumeration_Explicit()
        {
            var i = (EnumA)1;
            var o = (EnumA)new OptionSetValue(1);
            

            Assert.Equal(1, i.Id);
            Assert.Equal(1, o.Id);
        }

        [Fact]
        public void Enumeration_FromValue()
        {
            var enumA = EnumA.FromValue(1);
            Assert.Equal(1, enumA.Id);
        }

        [Fact]
        public void Enumeration_FromName()
        {
            var enumA = EnumA.FromName("Option1");
            Assert.Equal(1, enumA.Id);
            Assert.Throws<InvalidOperationException>(() => EnumA.FromName("OPTION1"));
        }

        [Fact]
        public void Enumeration_FromNameIgnoreCase()
        {
            var enumA = EnumA.FromName("OPTION1", true);
            Assert.Equal(1, enumA.Id);
        }
    }

    class EnumA : Enumeration<EnumA>
    {
        public static EnumA Option1 =
            new EnumA(1, "Option1");

        public static EnumA Option2 =
            new EnumA(2, "Option2");

        public static EnumA Option3 =
            new EnumA(3, "Option3");

        public static EnumA Option4 =
            new EnumA(4, "Option4");

        public EnumA(int id, string name) : base(id, name)
        {
        }
    }

    class EnumB : Enumeration<EnumB>
    {
        public static EnumB Option1 =
            new EnumB(1, "Option1");

        public static EnumB Option2 =
            new EnumB(2, "Option2");

        public EnumB(int id, string name) : base(id, name)
        {
        }
    }
}