using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoslynStringInterpolation.Tests
{
    [TestClass]
    public class RoslynStringInterpolationUtilsTests
    {
        [TestMethod]
        public void EnumerateVariablesTest() {
            var variables = InterpolationUtils.EnumerateVariables("I have $apples apples $@variable.number $variable11").ToArray();

            Assert.AreEqual(variables.Length, 3);
            Assert.AreEqual(variables[0], "apples");
            Assert.AreEqual(variables[1], "@variable.number");
            Assert.AreEqual(variables[2], "variable11");
        }
    }
}
