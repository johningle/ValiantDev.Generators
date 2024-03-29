﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace ValiantDev.Generators;

internal class NestedStaticReadOnlyStringGenerator
{
    private SyntaxTree _tree;
    private string _projectRootPath = string.Empty;
    private string _accessorClassName = string.Empty;
    private string _originFileExtension = string.Empty;

    public NestedStaticReadOnlyStringGenerator(string accessorClassName, string originFileExtension)
    {
        _accessorClassName = accessorClassName;
        _originFileExtension = originFileExtension;

        // start with an empty, static class syntax tree
        _tree = SyntaxFactory.SyntaxTree(
            SyntaxFactory.CompilationUnit()
                .WithMembers(
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                        SyntaxFactory.FileScopedNamespaceDeclaration(
                                SyntaxFactory.QualifiedName(
                                    SyntaxFactory.IdentifierName("ValiantDev"),
                                    SyntaxFactory.IdentifierName("Generators")))
                            .WithNamespaceKeyword(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.Comment("// <auto-generated/>")),
                                    SyntaxKind.NamespaceKeyword,
                                    SyntaxFactory.TriviaList()))
                            .WithMembers(
                                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                                    SyntaxFactory.ClassDeclaration(_accessorClassName)
                                        .WithModifiers(
                                            SyntaxFactory.TokenList(
                                                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                                                SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                                                SyntaxFactory.Token(SyntaxKind.PartialKeyword))))))));
    }

    public void Execute(IncrementalGeneratorInitializationContext context)
    {
        var projectPath = context.AnalyzerConfigOptionsProvider.Select((provider, _) =>
            provider.GlobalOptions.TryGetValue("build_property.projectdir", out var projectDir) ?
                Path.GetDirectoryName(projectDir) : string.Empty);

        var files = context.AdditionalTextsProvider
            .Where(file => file.Path.EndsWith(_originFileExtension))
            .Select((additionalText, _) => new TextDetail
            {
                Path = additionalText.Path,
                Content = additionalText.GetText()!.ToString()
            });

        var collection = files.Collect();
        var combination = collection.Combine(projectPath);

        context.RegisterSourceOutput(combination, (sourceProductionContext, tuple) =>
        {
            _projectRootPath = tuple.Right!;

            foreach (var textDetail in tuple.Left)
                AddStringMember(textDetail.Path, textDetail.Content);

            sourceProductionContext.AddSource($"{_accessorClassName}.generated.cs", _tree.ToString());
        });
    }

    private void AddStringMember(string filePath, string content)
    {
        if (TryGetClassDeclaration(_accessorClassName, out var parent))
        {
            var pathComponents = GetLocalPathComponents(filePath);

            foreach (var component in pathComponents)
            {
                if (TryGetClassDeclaration(component, out var nested))
                    parent = nested;
                else
                    parent = AddClassDeclaration(parent!, component);
            }

            var memberName = Path.GetFileNameWithoutExtension(filePath);

            if (HasStringMemberAsChildNode(parent!, memberName))
                parent = RemoveStringMemberChildNode(parent!, memberName, content);

            AddStringMemberAsChildNode(parent!, memberName, content);
        }
    }

    private ClassDeclarationSyntax RemoveStringMemberChildNode(ClassDeclarationSyntax parentNode, string memberName, string content)
    {
        var existing = parentNode.DescendantNodes(_ => true)
            .OfType<VariableDeclaratorSyntax>()
            .First(decl => decl.Identifier.ToString().Equals(memberName, StringComparison.InvariantCultureIgnoreCase));

        return parentNode.RemoveNode(existing, SyntaxRemoveOptions.KeepNoTrivia)!;
    }

    private bool HasStringMemberAsChildNode(ClassDeclarationSyntax parent, string memberName)
    {
        return parent.DescendantNodes(_ => true)
            .OfType<VariableDeclaratorSyntax>()
            .Any(decl => decl.Identifier.ToString().Equals(memberName, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool TryGetClassDeclaration(string identifier, out ClassDeclarationSyntax? node)
    {
        node = _tree.GetRoot()
            .DescendantNodes(_ => true)
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(decl => decl.Identifier.ToString().Equals(
                identifier, StringComparison.InvariantCultureIgnoreCase));

        return node != null;
    }

    private ImmutableArray<string> GetLocalPathComponents(string sqlFilePath)
    {
        var fileName = Path.GetFileName(sqlFilePath);
        var result = ImmutableArray<string>.Empty;

        if (Path.Combine(_projectRootPath, fileName).Length < sqlFilePath.Length)
        {
            var path = sqlFilePath.Substring(
                _projectRootPath.Length,
                sqlFilePath.Length - fileName.Length - _projectRootPath.Length);

            result = path
                .Split(Path.DirectorySeparatorChar)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToImmutableArray();
        }

        return result;
    }

    private ClassDeclarationSyntax? AddClassDeclaration(ClassDeclarationSyntax parentNode, string childIdentifier)
    {
        var newClassDeclaration = parentNode.AddMembers(
            SyntaxFactory.ClassDeclaration(childIdentifier)
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                            SyntaxFactory.Token(SyntaxKind.StaticKeyword))));

        MutateTree(parentNode, newClassDeclaration);

        TryGetClassDeclaration(childIdentifier, out var result);
        return result;
    }

    private void AddStringMemberAsChildNode(ClassDeclarationSyntax parentNode, string memberName, string content)
    {
        // add a const string member for the sql file name and content
        var newClassDeclaration = parentNode.AddMembers(
            SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(memberName))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(content)))))))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                        SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword))));

        MutateTree(parentNode, newClassDeclaration);
    }

    private void MutateTree(ClassDeclarationSyntax oldClassDeclaration, ClassDeclarationSyntax newClassDeclaration)
    {
        _tree = SyntaxFactory.SyntaxTree(
            _tree.GetRoot()
            .ReplaceNode(oldClassDeclaration, newClassDeclaration)
            .NormalizeWhitespace());
    }

    private class TextDetail
    {
        public string Path { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
