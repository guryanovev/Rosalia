﻿namespace Rosalia.TaskLib.Git.Tests
{
    using NUnit.Framework;
    using Rosalia.Core;
    using Rosalia.TaskLib.Standard.Tests;

    public class GetVersionTaskTests : ExternalToolTaskTestsBase<object, GetVersionInput, GetVersionOutput>
    {
        [Test]
        public void Execute_ValidOutput_SholdParse()
        {
            var task = new GetVersionTask<object>();
            AssertProcessOutputParsing(
                task,
                "v0.1.0-10-gcaa0502",
                (output, result) =>
                    {
                        Assert.That(result.ResultType, Is.EqualTo(ResultType.Success));
                        Assert.That(output, Is.Not.Null);
                        Assert.That(output.CommitsCount, Is.EqualTo(10));
                    });
        }

        [Test]
        public void Execute_NoTagOutput_SholdFail()
        {
            var task = new GetVersionTask<object>();
            AssertProcessOutputParsing(
                task,
                "fatal: No names found, cannot describe anything.",
                (output, result) =>
                    {
                        Assert.That(result.ResultType, Is.EqualTo(ResultType.Failure));
                        Assert.That(Logger.HasError((s, objects) => s == "fatal: No names found, cannot describe anything."));
                    });
        }
    }
}