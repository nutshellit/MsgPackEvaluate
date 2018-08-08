using System;
using Xunit;
using MsgPackLib;
using System.Linq;
using MessagePack;
using MessagePack.Resolvers;
using System.Diagnostics;

namespace MsgPackApiFixtures
{
    public class MsgPackSimpleFixtures
    {
        [Fact]
        public void Simple()
        {
            var ct = ComplexType1.Sample();
            SimpleType1 s1 = ct.Simple1.First();
            //MessagePackSerializer.SetDefaultResolver(ContractlessStandardResolver.Instance);
            var bytes = MessagePackSerializer.Serialize(s1);
            string test = MessagePackSerializer.ToJson(bytes);
            var s2 = MessagePackSerializer.Deserialize<SimpleType1>(bytes);
            Assert.Equal(s1.Id, s2.Id);
            Assert.Equal(s1.Address.Address1, s2.Address.Address1);
        }
        [Fact]
        public void Complex()
        {
            var ctOrg = ComplexType1.Sample();
            var bytes = MessagePackSerializer.Serialize(ctOrg);
            string test = MessagePackSerializer.ToJson(bytes);
            var ctDes = MessagePackSerializer.Deserialize<ComplexType1>(bytes);
            SimpleType1 stOrg = ctOrg.Simple1.First();
            SimpleType1 stDes = ctDes.Simple1.First();
            Assert.Equal(stOrg.Id, stOrg.Id);
            Assert.Equal(stDes.Address.Address1, stDes.Address.Address1);
        }
    }
}
