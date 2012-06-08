using System.Collections.Generic;
using System.Linq;

namespace Hygia.API.Models.Operations.LaunchPad
{
    public static class LaunchPadStatusExtensions
    {
        public static IEnumerable<LaunchPadStatus> ToOutputModel(this IEnumerable<Hygia.Operations.Communication.Domain.LaunchPadStatus> domainLaunchPadStatus)
        {
            return domainLaunchPadStatus.Select(ToOutputModel);
        }

        public static LaunchPadStatus ToOutputModel(this Hygia.Operations.Communication.Domain.LaunchPadStatus domainLaunchPadStatus)
        {
            return new LaunchPadStatus
                       {
                           EnvironmentId = domainLaunchPadStatus.EnvironmentId,
                           Id = domainLaunchPadStatus.Id,
                           TimeOfLastHeartBeat = domainLaunchPadStatus.TimeOfLastHeartBeat,
                           Version = domainLaunchPadStatus.Version
                       };
        }
    }
}