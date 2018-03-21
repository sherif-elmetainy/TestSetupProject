using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsService1
{
	[RunInstaller(true)]
	public class Service1Installer : Installer
	{
		public Service1Installer()
		{
			var serviceProcessInstaller = new ServiceProcessInstaller
			{
				Account = ServiceAccount.LocalSystem
			};
			var serviceInstaller = new ServiceInstaller
			{
				Description = Constants.ServiceDescription,
				DisplayName = Constants.ServiceDisplayName,
				ServiceName = Constants.ServiceName,
				StartType = ServiceStartMode.Automatic
			};
			Installers.Add(serviceProcessInstaller);
			Installers.Add(serviceInstaller);
		}

		protected override void OnBeforeUninstall(IDictionary savedState)
		{
			try
			{
				var serviceController = new ServiceController(Constants.ServiceName);
				if (serviceController.Status == ServiceControllerStatus.Stopped)
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
				}
			}
			catch
			{
				// ignored
			}
			base.OnBeforeUninstall(savedState);
		}

		protected override void OnAfterInstall(IDictionary savedState)
		{
			base.OnAfterInstall(savedState);
			try
			{
				var serviceController = new ServiceController(Constants.ServiceName);
				serviceController.Start();
			}
			catch
			{
				// ignored
			}
		}
	}
}
