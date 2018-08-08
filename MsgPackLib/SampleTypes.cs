using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MsgPackLib
{
    [MessagePackObject]
    public class SimpleType1
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public Address Address { get; set; }
    }
    [MessagePackObject]
    public class SimpleType2
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string Preference { get; set; }
        [Key(2)]
        public Address Location { get; set; }
    }
    [MessagePackObject]
    public class SimpleType3 : SimpleType2
    {
        [Key(3)]
        public bool IsValid { get; set; }
    }
    [MessagePackObject]
    public class ComplexType1
    {
        static readonly Guid SampleGuid1 = Guid.Parse("4e7f1ea4-c3c5-478c-920b-81582ae81663");
        [Key(0)]
        public List<SimpleType1> Simple1 { get; set; } = new List<SimpleType1>();
        [Key(1)]
        public List<SimpleType2> Simple2 { get; set; } = new List<SimpleType2>();
        [Key(2)]
        public List<SimpleType3> Simple3 { get; set; } = new List<SimpleType3>();


        public static ComplexType1 Sample()
        {
            Address GetAdd()
            {
                return new Address { Address1 = "add1", Address2 = "add2" };
            }
            SimpleType1 Type1( string name)
            {
                return new SimpleType1 { Id = SampleGuid1, Name = name, Address = GetAdd() };
            }

            SimpleType2 Type2(string pref) {
                return new SimpleType2 { Id = SampleGuid1, Preference = pref, Location = GetAdd() };
            }

            SimpleType3 Type3(string pref)
            {
                return new SimpleType3 { Id = SampleGuid1, Preference = pref, Location = GetAdd(), IsValid = true };

            }

            return new ComplexType1
            {
                Simple1 = (new[] { Type1("name1"), Type1("name2"), Type1("name3") }).ToList(),
                Simple2 = new[] { Type2("pref1"), Type2("pref2"), Type2("pref3") }.ToList(),
                Simple3 = new[] { Type3("pref4"), Type3("pref5") }.ToList()
            };
        }
    }
    [MessagePackObject]
    public class Address
    {
        [Key(0)]
        public string Address1 { get; set; }
        [Key(1)]
        public string Address2 { get; set; }
    }




}
