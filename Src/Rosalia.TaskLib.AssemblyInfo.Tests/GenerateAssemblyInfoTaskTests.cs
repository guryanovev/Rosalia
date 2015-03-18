﻿namespace Rosalia.TaskLib.AssemblyInfo.Tests
{
    using System.Reflection;
    using NUnit.Framework;
    using Rosalia.TestingSupport.FileSystem;
    using Rosalia.TestingSupport.Helpers;

    [TestFixture]
    public class GenerateAssemblyInfoTaskTests 
    {
        [Test]
        public void Execute_ValidInput_ShouldGenerateOutput()
        {
            var destination = new FileStub("AssemblyInfo.cs");
            var task = new GenerateAssemblyInfo(destination)
            {
                Attributes =
                {
                    _ => new AssemblyVersionAttribute("1.0.42"),
                    _ => new AssemblyCompanyAttribute("Starfuckers, Inc.")
                }
            };

            task
                .Execute()
                .AssertSuccess();

            Assert.That(destination.Content.Trim(), Is.EqualTo(
@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Reflection;

[assembly: AssemblyVersionAttribute(""1.0.42"")]
[assembly: AssemblyCompanyAttribute(""Starfuckers, Inc."")]"));
        }
    }
}