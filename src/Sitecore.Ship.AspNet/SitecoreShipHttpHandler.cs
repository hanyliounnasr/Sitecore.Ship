using System.Web;
using Sitecore.Ship.AspNet.Package;
using Sitecore.Ship.AspNet.Publish;
using Sitecore.Ship.Infrastructure.Configuration;
using Sitecore.Ship.Core.Domain;

namespace Sitecore.Ship.AspNet
{
    public class SitecoreShipHttpHandler : BaseHttpHandler
    {
        private readonly CommandHandler _commandChain;
        private readonly PackageInstallationSettings _settings;

        public SitecoreShipHttpHandler()
        {
            // TODO move this construction logic out of here ...
            _settings = new PackageInstallationConfigurationProvider().Settings;

            var aboutCommand = new AboutCommand();

            var installPackageCommand = new InstallPackageCommand();

            var installUploadPackageCommand = new InstallUploadPackageCommand();

            var latestVersionCommand = new LatestVersionCommand();

            var invokePublishingCommand = new InvokePublishingCommand();

            var publishingLastCompletedCommand = new PublishingLastCompletedCommand();

            var unhandledCommand = new UnhandledCommand();

            aboutCommand.SetSuccessor(installPackageCommand);

            installPackageCommand.SetSuccessor(installUploadPackageCommand);

            installUploadPackageCommand.SetSuccessor(latestVersionCommand);

            latestVersionCommand.SetSuccessor(invokePublishingCommand);

            invokePublishingCommand.SetSuccessor(publishingLastCompletedCommand);

            publishingLastCompletedCommand.SetSuccessor(unhandledCommand);

            _commandChain = aboutCommand;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            context.Server.ScriptTimeout = _settings.ExecutionTimeout;
            _commandChain.HandleRequest(context);
        }
    }
}