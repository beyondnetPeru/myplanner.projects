﻿using BeyondNet.Ddd.Rules.Interfaces;

namespace BeyondNet.Ddd.ValueObjects
{
    public class IdValueObject : ValueObject<string>
    {
        
        protected IdValueObject(string value) : base(value)
        {

        }

        public static IdValueObject Create()
        {
            return new IdValueObject(Guid.NewGuid().ToString());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static IdValueObject DefaultValue => new IdValueObject(Guid.Empty.ToString());

    }
}