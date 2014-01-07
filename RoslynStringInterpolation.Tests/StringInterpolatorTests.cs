using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Compilers.CSharp;

namespace RoslynStringInterpolation.Tests
{
    [TestClass]
    public class StringInterpolatorTests {
        private const string TestCode = @"
            using System;
            class Hello{ 
	            static void Main() {
                var value = 1;
	            Console.WriteLine(""hello, $value"");
	            }
            }
";
        
        [TestMethod]
        public void IntrepolateTest() {
            var syntaxNode = new StringInterpolator(SyntaxTree.ParseText(TestCode)).Intrepolate();

            var result = syntaxNode.GetText().ToString();

            Assert.IsTrue(result.Contains("string.Format(\"hello, {0}\", value)"));
        }
    }
}
