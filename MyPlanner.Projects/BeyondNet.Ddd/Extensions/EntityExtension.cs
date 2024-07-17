using BeyondNet.Ddd.Helpers;
using BeyondNet.Ddd.Rules;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BeyondNet.Ddd.Extensions
{

    public static class EntityExtension
    {
        public static ReadOnlyCollection<BrokenRule> GetPropertiesBrokenRules(this Entity<object> entity, PropertyInfo[] properties) 
        {
            
            var result = new List<BrokenRule>();
         

            foreach (var property in properties)
            {
                var isValueObject = ReflectionHelper.IsSubclassOfRawGeneric(typeof(ValueObject<>), property.PropertyType);

                if (isValueObject)
                {
                    var method = property.PropertyType.GetMethod("GetBrokenRules");

                    if (method != null)
                    {
                        var valueObject = property.GetValue(entity);

                        if (valueObject == null)
                        {
                            continue;
                        }

                        var brokenRules = (ReadOnlyCollection<BrokenRule>)method.Invoke(valueObject, null)!;

                        if (brokenRules.Any())
                        {
                            foreach (var brokenRule in brokenRules)
                            {
                                if (!result.Any(x => x.Property.ToLower() == brokenRule.Property.ToLower()
                                                                       && x.Message.ToLower() == brokenRule.Message.ToLower()))
                                {
                                    result.Add(brokenRule);
                                }
                            }
                        }
                    }
                }
            }

            return result.AsReadOnly();
        }

        public static List<BrokenRule> GetPropertiesBrokenRules<TEntity>(this PropertyInfo[] properties, TEntity instance)
        {
            var result = new List<BrokenRule>();

            foreach (var property in properties)
            {
                var isValueObject = ReflectionHelper.IsSubclassOfRawGeneric(typeof(ValueObject<>), property.PropertyType);
                if (isValueObject)
                {
                    var method = property.PropertyType.GetMethod("GetBrokenRules");
                    if (method != null)
                    {
                        var valueObject = property.GetValue(instance);

                        if (valueObject == null)
                        {
                            continue;
                        }

                        var brokenRules = (ReadOnlyCollection<BrokenRule>)method.Invoke(valueObject, null)!;

                        result.AddRange(brokenRules);
                    }
                }
            }

            return result;
        }
    }
}
