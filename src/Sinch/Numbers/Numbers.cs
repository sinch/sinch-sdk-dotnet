using System;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Numbers
{
    /// <summary>
    ///     You can use the Active Number API to manage numbers you own. Assign numbers to projects, release numbers from
    ///     projects, or list all numbers assigned to a project.
    /// </summary>
    public interface ISinchNumbers
    {
        /// <summary>
        ///     You can use the Available Regions API to list all of the regions that have numbers assigned to a project.
        /// </summary>
        public ISinchNumbersRegions Regions { get; }

        /// <summary>
        ///     You can use the Available Number API to search for available numbers or activate an available number.
        /// </summary>
        public ISinchNumbersAvailable Available { get; }

        /// <summary>
        ///     You can use the Active Number API to manage numbers you own. Assign numbers to projects,
        ///     release numbers from projects, or list all numbers assigned to a project.
        /// </summary>
        public ISinchNumbersActive Active { get; }
    }

    public sealed class Numbers : ISinchNumbers
    {
        internal Numbers(string projectId, Uri baseAddress,
            LoggerFactory loggerFactory, IHttp http)
        {
            Regions = new AvailableRegions(projectId, baseAddress,
                loggerFactory?.Create<AvailableRegions>(), http);
            Active = new ActiveNumbers(projectId, baseAddress,
                loggerFactory?.Create<ActiveNumbers>(), http);
            Available = new AvailableNumbers(projectId, baseAddress,
                loggerFactory?.Create<AvailableNumbers>(), http);
        }

        public ISinchNumbersRegions Regions { get; }

        public ISinchNumbersActive Active { get; }

        public ISinchNumbersAvailable Available { get; }
    }
}
