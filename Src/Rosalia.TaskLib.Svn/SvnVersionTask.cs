﻿namespace Rosalia.TaskLib.Svn
{
    using System;
    using System.Linq;
    using System.Text;
    using Rosalia.Core.Context;
    using Rosalia.Core.Fluent;
    using Rosalia.TaskLib.Standard.Tasks;

    public class SvnVersionTask<T> : ExternalToolTask<T, SvnVersionInput, SvnVersionResult>
    {
        private static readonly char[] AllowedTrailChars = new[] { 'M', 'S', 'P' };

        private SvnVersion _min;
        private SvnVersion _max;

        public SvnVersionTask(Func<TaskContext<T>, SvnVersionInput> inputProvider)
            : base(inputProvider)
        {
        }

        protected override string DefaultToolPath
        {
            get { return "svnversion"; }
        }

        protected override SvnVersionResult CreateResult(int exitCode, ResultBuilder resultBuilder)
        {
            return new SvnVersionResult(_min, _max);
        }

        protected override void ProcessOnOutputDataReceived(string message, SvnVersionInput input, ResultBuilder result, TaskContext<T> context)
        {
            base.ProcessOnOutputDataReceived(message, input, result, context);

            var versionString = message;
            if (!string.IsNullOrEmpty(versionString))
            {
                if (versionString.Equals("Unversioned directory", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.AddError("Working copy {0} is not versioned!", input.WorkingCopyPath);
                    result.Fail();
                    return;
                }

                var parts = versionString.Split(':');
                if (parts.Length < 1 || parts.Length > 2)
                {
                    result.AddError("Unexpected tool output: {0}", versionString);
                    result.Fail();
                    return;
                }

                try
                {
                    _min = ParseVersion(parts[0]);
                    _max = parts.Length > 1 ? ParseVersion(parts[1]) : _min;
                }
                catch (FormatException ex)
                {
                    throw new Exception(string.Format("Unexpected tool output: {0}", versionString), ex);
                }
            }
        }

        protected override string GetToolArguments(SvnVersionInput input, TaskContext<T> context)
        {
            var builder = new StringBuilder();
            builder.Append(input.Commited ? "-c" : string.Empty);

            if (!string.IsNullOrEmpty(input.WorkingCopyPath))
            {
                builder.Append(" ");
                builder.Append(input.WorkingCopyPath);
                builder.Append(" ");
                builder.Append(input.TrailUrl);
            }

            return builder.ToString();
        }

        private SvnVersion ParseVersion(string version)
        {
            var trailBuilder = new StringBuilder();
            var trailRead = false;
            while ((!trailRead) && version.Length > 0)
            {
                var lastVersionChar = version[version.Length - 1];
                if (AllowedTrailChars.Any(trailChar => trailChar == lastVersionChar))
                {
                    version = version.Substring(0, version.Length - 1);
                    trailBuilder.Insert(0, lastVersionChar);
                }
                else
                {
                    trailRead = true;
                }
            }

            return new SvnVersion(int.Parse(version), trailBuilder.ToString());
        }
    }
}