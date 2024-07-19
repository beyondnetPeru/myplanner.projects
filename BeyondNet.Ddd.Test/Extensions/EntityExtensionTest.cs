using BeyondNet.Ddd.Extensions;
using BeyondNet.Ddd.Test.Stubs;
using BeyondNet.Ddd.ValueObjects;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeyondNet.Ddd.Test.Extensions
{
    [TestClass]
    public class EntityExtensionTest
    {
        [TestMethod]
        public void GetPropertiesBrokenRules_ShouldReturnEmptyList_WhenNoBrokenRulesExist()
        {
            var entity = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("foo"));

            entity.GetBrokenRules().Any().ShouldBeFalse();
        }

        [TestMethod]
        public void GetPropertiesBrokenRules_ShouldReturnBrokenRules_WhenBrokenRulesExist()
        {
            var entity = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            entity.GetBrokenRules().Any().ShouldBeTrue();
        }
    }

}
