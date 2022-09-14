namespace TabBarSwitches.Maui
{
    public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
    {
        public AppShell()
        {
            InitializeComponent();
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