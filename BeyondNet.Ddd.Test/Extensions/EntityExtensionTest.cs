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
            // Arrange
            var entity = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("foo"));
            var properties = new PropertyInfo[0];

            // Act
            var result = properties.GetPropertiesBrokenRules(properties);

            // Assert
            result.Count.ShouldBe(0);
        }

        [TestMethod]
        public void GetPropertiesBrokenRules_ShouldReturnBrokenRules_WhenBrokenRulesExist()
        {
            // Arrange
            var entity = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));
            var properties = entity.GetType().GetProperties();

            // Act
            var result = properties.GetPropertiesBrokenRules(entity);

            // Assert
            result.Count.ShouldBe(1);
            result.First().Property.ShouldBe("Value");
            result.First().Message.ShouldBe("Value cannot be null or empty");
        }
    }

}
