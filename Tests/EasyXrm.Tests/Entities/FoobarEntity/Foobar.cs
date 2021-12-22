using System;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace EasyXrm.Tests.Entities.FoobarEntity
{
    [DataContract(Name = "Entity", Namespace = "http://schemas.microsoft.com/xrm/2011/Contracts")]
    [EntityLogicalName(EntityLogicalName)]
    public class Foobar : Entity
    {
        public const string EntityLogicalName = "foobar";

        [AttributeLogicalName("name")]
        public string Name
        {
            get => GetAttributeValue<string>("name");
            set => SetAttributeValue("name", value);
        }

        [AttributeLogicalName("parent_foobar_id")]
        public EntityReference ParentFoobarId
        {
            get => GetAttributeValue<EntityReference>("parent_foobar_id");
            set => SetAttributeValue("parent_foobar_id", value);
        }

        [AttributeLogicalName("age")]
        public int? Age
        {
            get => GetAttributeValue<int?>("age");
            set => SetAttributeValue("age", value);
        }

        [AttributeLogicalName("gender_code")]
        public OptionSetValue GenderCode
        {
            get => GetAttributeValue<OptionSetValue>("gender_code");
            set => SetAttributeValue("gender_code", value);
        }

        public Foobar() : base(EntityLogicalName)
        {
        }

        public Foobar(Guid id) : base(EntityLogicalName, id)
        {
        }
    }
}