using System;
using System.Collections.Generic;
using System.Windows;
using log4net;

namespace TDV.Client.Shell
{
    public partial class App
    {
        private Bootstrapper _bootstrapper;
        private static readonly ILog Log;

        static App()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = LogManager.GetLogger(typeof(App));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            //dispose the disposables
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            try
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                _bootstrapper = new Bootstrapper();
                _bootstrapper.Run();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Oops", ex);
            }
        }
    }
}
