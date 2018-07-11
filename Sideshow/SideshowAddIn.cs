using Start9.Api.AppBar;
using Sideshow.Views;
using System;
using System.AddIn;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Sideshow
{
    [AddIn("Sideshow", Description = "Longhorn-flavoured Sidecar", Version = "1.0.0.0", Publisher = "Start9")]
    public class SideshowAddIn : IModule
    {
        public static SideshowAddIn Instance { get; private set; }

        public IConfiguration Configuration { get; set; } = new SideshowConfiguration();

        public IMessageContract MessageContract => null;

        public IReceiverContract ReceiverContract { get; } = new SideshowReceiverContract();

        public IHost Host { get; private set; }

        public void Initialize(IHost host)
        {
            void Start()
            {
                Instance = this;
                Host = host;
                Application.ResourceAssembly = Assembly.GetExecutingAssembly();
                App.Main();
            }

            var t = new Thread(Start);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {

                MessageBox.Show(e.ExceptionObject.ToString(), "Uh oh E R R O R E");
            };
        }
    }

    public class SideshowReceiverContract : IReceiverContract
    {
        public SideshowReceiverContract()
        {
            SidebarToggledEntry.MessageReceived += (sender, e) =>
            {
                if (((MainWindow)Application.Current.MainWindow).IsVisible)
                    ((MainWindow)Application.Current.MainWindow).Hide();
                else
                    ((MainWindow)Application.Current.MainWindow).Show();
            };
        }
        public IList<IReceiverEntry> Entries => new[] { SidebarToggledEntry };
        public IReceiverEntry SidebarToggledEntry { get; } = new ReceiverEntry("Toggle Visibility");
    }

    public class SideshowConfiguration : IConfiguration
    {
        public IList<IConfigurationEntry> Entries => new[] { new ConfigurationEntry(GroupItems, "Group Items") };

        public TaskbarGroupMode GroupItems { get; set; } = TaskbarGroupMode.GroupAlways;

        public Boolean ShowSearchBar { get; set; } = false;

        public Boolean Lock { get; set; } = false;

        public Boolean AutoHide { get; set; } = false;

        public AppBarDockMode DockPosition { get; set; } = AppBarDockMode.Bottom;

        public Boolean ShowStoreApps { get; set; } = true;

        public Boolean ShowOnAllDisplays { get; set; } = true;

        public Boolean ReplaceCmdWithPowershell { get; set; } = true;

        public List<Toolbar> Toolbars { get; set; } = new List<Toolbar>();

    }

    [Serializable]
    public class Toolbar
    {
        public String Name { get; set; }

    }

    public enum TaskbarGroupMode
    {
        GroupAlways,
        DontGroup,
        GroupWhenFull
    }

}
