using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RoslynStringInterpolation
{
    public static class InterpolationUtils
    {
        private static readonly Regex VariableRegex = new Regex(@"\$(@{0,1}[a-zA-Z_\.0-9]+)");

        public static IEnumerable<string> EnumerateVariables(string s) {
            var matchCollection = VariableRegex.Matches(s);

            for (int i = 0; i < matchCollection.Count; i++) {
                yield return matchCollection[i].Groups[1].Value;
            }
        }
    }
}
