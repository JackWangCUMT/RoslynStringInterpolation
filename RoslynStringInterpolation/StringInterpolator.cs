using System.Linq;
using Roslyn.Compilers.CSharp;

namespace RoslynStringInterpolation {
    public class StringInterpolator {
        private readonly SyntaxTree syntaxTree;

        class Rewriter : SyntaxRewriter {
            public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node) {
                if (node.Kind != SyntaxKind.StringLiteralExpression)
                    return node;

                var variables = InterpolationUtils.EnumerateVariables(node.GetText().ToString()).Distinct().ToArray();
                var arguments = string.Join(", ", variables);
                var formatString = node.GetText().ToString();

                for (int i = 0; i < variables.Length; i++) {
                    formatString = formatString.Replace("$" + variables[0], "{" + i + "}");
                }

                return Syntax.ParseExpression(string.Format("string.Format({0}, {1})", formatString, arguments));
            }
        }

        public StringInterpolator(SyntaxTree syntaxTree) {
            this.syntaxTree = syntaxTree;
        }

        public SyntaxNode Intrepolate() {
            return new Rewriter().Visit(syntaxTree.GetRoot());
        }
    }
}