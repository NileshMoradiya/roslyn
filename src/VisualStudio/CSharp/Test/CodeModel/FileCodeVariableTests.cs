// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using EnvDTE;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.VisualStudio.LanguageServices.CSharp.UnitTests.CodeModel
{
    public class FileCodeVariableTests : AbstractFileCodeElementTests
    {
        public FileCodeVariableTests()
            : base(@"using System;

public class A
{
    // This is a comment.
    public int intA;

   /// <summary>
   /// This is a summary.
   /// </summary>
    protected int intB;

    [Serializable]
    private int intC = 4;
    int intD;

    public static const int FORTYTWO = 42;
}

unsafe public struct DevDivBugs70194
{
    fixed char buffer[100];
}")
        {
        }

        private CodeVariable GetCodeVariable(params object[] path)
        {
            return (CodeVariable)GetCodeElement(path);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Access_Public()
        {
            CodeVariable testObject = GetCodeVariable("A", "intA");

            Assert.Equal(vsCMAccess.vsCMAccessPublic, testObject.Access);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Access_Protected()
        {
            CodeVariable testObject = GetCodeVariable("A", "intB");

            Assert.Equal(vsCMAccess.vsCMAccessProtected, testObject.Access);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Access_Private()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.Equal(vsCMAccess.vsCMAccessPrivate, testObject.Access);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Attributes_Count()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.Equal(1, testObject.Attributes.Count);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Children_Count()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.Equal(1, testObject.Children.Count);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Comment()
        {
            CodeVariable testObject = GetCodeVariable("A", "intA");

            Assert.Equal("This is a comment.\r\n", testObject.Comment);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void DocComment()
        {
            CodeVariable testObject = GetCodeVariable("A", "intB");

            string expected = "<doc>\r\n<summary>\r\nThis is a summary.\r\n</summary>\r\n</doc>";

            Assert.Equal(expected, testObject.DocComment);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void InitExpressions_NoExpression()
        {
            CodeVariable testObject = GetCodeVariable("A", "intB");

            Assert.Equal(null, testObject.InitExpression);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void InitExpression()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.Equal("4", testObject.InitExpression);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void InitExpression_FixedBuffer()
        {
            CodeVariable testObject = GetCodeVariable("DevDivBugs70194", "buffer");

            Assert.Equal(null, testObject.InitExpression);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void IsConstant_True()
        {
            CodeVariable testObject = GetCodeVariable("A", "FORTYTWO");

            Assert.True(testObject.IsConstant);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void IsConstant_False()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.False(testObject.IsConstant);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void IsShared_True()
        {
            CodeVariable testObject = GetCodeVariable("A", "FORTYTWO");

            Assert.True(testObject.IsShared);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void IsShared_False()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.False(testObject.IsShared);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Kind()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.Equal(vsCMElement.vsCMElementVariable, testObject.Kind);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Parent()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            CodeClass testObjectParent = testObject.Parent as CodeClass;

            Assert.Equal("A", testObjectParent.Name);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void Type()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            Assert.Equal("System.Int32", testObject.Type.AsFullName);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_Attributes()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartAttributes));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_AttributesWithDelimiter()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint startPoint = testObject.GetStartPoint(vsCMPart.vsCMPartAttributesWithDelimiter);

            Assert.Equal(13, startPoint.Line);
            Assert.Equal(5, startPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_Body()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<COMException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartBody));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_BodyWithDelimiter()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartBodyWithDelimiter));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_Header()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartHeader));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_HeaderWithAttributes()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartHeaderWithAttributes));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_Name()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartName));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_Navigate()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint startPoint = testObject.GetStartPoint(vsCMPart.vsCMPartNavigate);

            Assert.Equal(14, startPoint.Line);
            Assert.Equal(17, startPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_Whole()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetStartPoint(vsCMPart.vsCMPartWhole));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetStartPoint_WholeWithAttributes()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint startPoint = testObject.GetStartPoint(vsCMPart.vsCMPartWholeWithAttributes);

            Assert.Equal(13, startPoint.Line);
            Assert.Equal(5, startPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_Attributes()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartAttributes));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_AttributesWithDelimiter()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint endPoint = testObject.GetEndPoint(vsCMPart.vsCMPartAttributesWithDelimiter);

            Assert.Equal(13, endPoint.Line);
            Assert.Equal(19, endPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_Body()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<COMException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartBody));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_BodyWithDelimiter()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartBodyWithDelimiter));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_Header()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartHeader));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_HeaderWithAttributes()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartHeaderWithAttributes));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_Name()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartName));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_Navigate()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint endPoint = testObject.GetEndPoint(vsCMPart.vsCMPartNavigate);

            Assert.Equal(14, endPoint.Line);
            Assert.Equal(21, endPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_Whole()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");
            AssertEx.Throws<NotImplementedException>(() => testObject.GetEndPoint(vsCMPart.vsCMPartWhole));
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void GetEndPoint_WholeWithAttributes()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint endPoint = testObject.GetEndPoint(vsCMPart.vsCMPartWholeWithAttributes);

            Assert.Equal(14, endPoint.Line);
            Assert.Equal(26, endPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void StartPoint()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint startPoint = testObject.StartPoint;

            Assert.Equal(13, startPoint.Line);
            Assert.Equal(5, startPoint.LineCharOffset);
        }

        [ConditionalFact(typeof(x86))]
        [Trait(Traits.Feature, Traits.Features.CodeModel)]
        public void EndPoint()
        {
            CodeVariable testObject = GetCodeVariable("A", "intC");

            TextPoint endPoint = testObject.EndPoint;

            Assert.Equal(14, endPoint.Line);
            Assert.Equal(26, endPoint.LineCharOffset);
        }
    }
}
