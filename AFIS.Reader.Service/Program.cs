﻿using System.ServiceProcess;

namespace AFIS.Reader.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FingerPrintService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
