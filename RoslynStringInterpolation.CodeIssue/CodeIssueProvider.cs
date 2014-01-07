using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.Editor;

namespace RoslynStringInterpolation.CodeIssue
{
    [ExportCodeIssueProvider("RoslynStringInterpolation.CodeIssue", LanguageNames.CSharp)]
    class CodeIssueProvider : ICodeIssueProvider
    {
        public IEnumerable<Roslyn.Services.CodeIssue> GetIssues(IDocument document, CommonSyntaxNode node, CancellationToken cancellationToken)
        {
            var variables = document.GetSyntaxRoot(cancellationToken).DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Select(syntax => syntax.Identifier.Value)
                .ToArray();

            foreach (var literal in node.DescendantNodes().OfType<LiteralExpressionSyntax>().Where(syntax => syntax.Kind == SyntaxKind.StringLiteralExpression)) {
                foreach (var variable in InterpolationUtils.EnumerateVariables(literal.GetText().ToString())) {
                    if(!variables.Contains(variable))
                        yield return new Roslyn.Services.CodeIssue(CodeIssueKind.Warning, literal.Span, string.Format("No variable {0}", variable));
                    }
            }
        }

        public IEnumerable<Type> SyntaxNodeTypes
        {
            get
            {
                yield return typeof(SyntaxNode);
            }
        }

        #region Unimplemented ICodeIssueProvider members

        public IEnumerable<Roslyn.Services.CodeIssue> GetIssues(IDocument document, CommonSyntaxToken token, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> SyntaxTokenKinds
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}
