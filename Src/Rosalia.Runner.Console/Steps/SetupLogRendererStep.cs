﻿namespace Rosalia.Runner.Console.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Rosalia.Core.Logging;
    using Rosalia.Core.Tasks;
    using Rosalia.Runner.Console.CommandLine;
    using Rosalia.Runner.Console.CommandLine.Support;

    public class SetupLogRendererStep : IProgramStep
    {
        public int UnhandledExceptionReturnCode
        {
            get { return ExitCode.Error.UnknownError; }
        }

        public int? Execute(ProgramContext context)
        {
            var logRenderers = new List<ILogRenderer>
            {
                new ColoredConsoleLogRenderer()
            };

            foreach (var path in context.Options.OutputFiles)
            {
                var currentPath = path;
                if (Path.GetExtension(path).Equals(".html", StringComparison.InvariantCultureIgnoreCase))
                {
                    logRenderers.Add(new HtmlLogRenderer(new Lazy<TextWriter>(() => File.CreateText(currentPath))));
                }
            }

            context.LogRenderer = new CompositeLogRenderer(logRenderers.ToArray());
            context.LogRenderer.Init();
            context.Log = new LogHelper(message => context.LogRenderer.Render(message, "Runner"));

            return null;
        }
    }
}