// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.IIS.FunctionalTests.Utilities;
using Microsoft.AspNetCore.Testing;
using Xunit;

namespace Microsoft.AspNetCore.Server.IIS.FunctionalTests.InProcess
{
    [Collection(PublishedSitesCollection.Name)]
    public class EventLogTests : IISFunctionalTestBase
    {
        public EventLogTests(PublishedSitesFixture fixture) : base(fixture)
        {
        }

        [ConditionalFact]
        [MaximumOSVersion(OperatingSystems.Windows, WindowsVersions.Win10_20H1, SkipReason = "Shutdown hangs https://github.com/dotnet/aspnetcore/issues/25107")]
        public async Task CheckStartupEventLogMessage()
        {
            var deploymentParameters = Fixture.GetBaseDeploymentParameters();
            var deploymentResult = await DeployAsync(deploymentParameters);
            await deploymentResult.AssertStarts();

            StopServer();

            EventLogHelpers.VerifyEventLogEvent(deploymentResult, EventLogHelpers.InProcessStarted(deploymentResult), Logger);
        }

        [ConditionalFact]
        [MaximumOSVersion(OperatingSystems.Windows, WindowsVersions.Win10_20H1, SkipReason = "Shutdown hangs https://github.com/dotnet/aspnetcore/issues/25107")]
        public async Task CheckShutdownEventLogMessage()
        {
            var deploymentParameters = Fixture.GetBaseDeploymentParameters();
            var deploymentResult = await DeployAsync(deploymentParameters);
            await deploymentResult.AssertStarts();

            StopServer();

            EventLogHelpers.VerifyEventLogEvent(deploymentResult, "Application '.+' has shutdown.", Logger);
        }
    }
}
