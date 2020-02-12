using BcFileTool.Library.Interfaces.Services;
using BcFileTool.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BcFileTool.Library.Services
{
    class RuleMatchingService : IMatchingService
    {
        List<Rule> _rules;


        public void Configure(Configuration configuration)
        {
            _rules = configuration.Rules;
        }

        public bool TryMatch(string file, out Rule matchedRule)
        {
            matchedRule = null;
            foreach (var rule in _rules)
            {
                if(rule.Test(file))
                {
                    matchedRule = rule;
                    return true;
                }
            }
            return false;
        }
    }
}
