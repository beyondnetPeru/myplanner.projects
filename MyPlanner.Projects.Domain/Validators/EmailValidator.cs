﻿using BeyondNet.Ddd;
using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Rules.Impl;
using MyPlanner.Shared.Helpers;

namespace MyPlanner.Projects.Domain.Validators
{
    public class EmailValidator : AbstractRuleValidator<ValueObject<string>>
    {
        public EmailValidator(ValueObject<string> subject) : base(subject)
        {
        }

        public override void AddRules(RuleContext context)
        {
            var value = Subject!.Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                AddBrokenRule("Email", "Email is required");
            }

            if (!string.IsNullOrWhiteSpace(value) && !EmailHelper.IsValidEmail(value))
            {
                AddBrokenRule("Email", "Email is invalid");
            }
        }
    }
}
