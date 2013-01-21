﻿namespace Rosalia.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using Rosalia.Core.Result;

    public class FailureTask<T> : ITask<T>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<IIdentifiable> IdentifiableChildren { get; private set; }

        public bool HasChildren { get; set; }

        public IEnumerable<ITask<T>> Children { get; set; }

        public ExecutionResult Execute(ExecutionContext<T> context)
        {
            return new ExecutionResult(ResultType.Failure, null);
        }
    }
}