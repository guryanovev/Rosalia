﻿namespace Rosalia.Core.Api
{
    using Rosalia.Core.Tasks;

    public static partial class Extensions
    {
        public static ITask<T> ToTask<T>(this ITaskRegistry<T> registry) where T : class
        {
            return new SubflowTask<T>(registry, Identities.Empty);
        }

        public static ITask<TResult> AsTask<TResult>(this ITask<TResult> task) where TResult : class
        {
            return task;
        }
    }
}