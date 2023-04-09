using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;

namespace AFIS.Reader.Service
{
    public partial class FingerPrintService : ServiceBase
    {
        public FingerPrintService()
        {
            InitializeComponent();
        }

        ServiceHost serviceHost;
        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
                serviceHost.Close();

            serviceHost = new ServiceHost(typeof(ReaderWCFService));
            serviceHost.OpenTimeout = TimeSpan.FromMinutes(15);

            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
        }
    }

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "AFIS.Reader.Service";
            service.DisplayName = "AFIS.Reader.Service";
            service.Description = "AFIS Reader Service WCF Finger Print Service Hosted by Windows NT Service";
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            Process[] processesByName = Process.GetProcessesByName("AFIS.Reader.Service");
            for (int i = 0; i < processesByName.Length; i++)
            {
                processesByName[i].Kill();
            }
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            using (ServiceController sc = new ServiceController("AFIS.Reader.Service"))
            {
                sc.Start();
            }
        }

    }

}
