using SimpleToolkit.Core;

namespace TabBarSwitches.Maui
{
    public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
    {
        public AppShell()
        {
            InitializeComponent();

            Loaded += AppShellLoaded;
        }

        private static void AppShellLoaded(object sender, EventArgs e)
        {
            var shell = sender as AppShell;

            shell.Window.SubscribeToSafeAreaChanges(safeArea =>
            {
                shell.rootContainer.Padding = new Thickness(safeArea.Left, 0, safeArea.Right, 0);
                shell.tabBarView.InnerPadding = new Thickness(0, 0, 0, safeArea.Bottom);
            });
        }

        private void TabBarViewCurrentPageChanged(object sender, TabBarEventArgs e)
        {
            Shell.Current.GoToAsync("///" + e.CurrentPage.ToString());
        }
    }

    public enum PageType
    {
        HomePage = 0, LikesPage = 1, ChatsPage = 2, SettingsPage = 3
    }
}