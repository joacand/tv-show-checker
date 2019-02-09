using System;
using System.Windows.Forms;
using Unity;

namespace TVShowChecker
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<TVShowCheckerForm>());
        }

        internal static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.AddRegistrations();
            return container;
        }
    }
}
