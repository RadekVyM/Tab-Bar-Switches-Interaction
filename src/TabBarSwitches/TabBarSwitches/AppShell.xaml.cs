using System;
using Xamarin.Forms;

namespace TabBarSwitches
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            AddTab(typeof(HomePage), PageEnum.HomePage);
            AddTab(typeof(LikesPage), PageEnum.LikesPage);
            AddTab(typeof(ChatsPage), PageEnum.ChatsPage);
            AddTab(typeof(SettingsPage), PageEnum.SettingsPage);
        }

        private void AddTab(Type page, PageEnum pageEnum)
        {
            Tab tab = new Tab { Route = pageEnum.ToString(), Title = pageEnum.ToString() };
            tab.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(page) });

            tabBar.Items.Add(tab);
        }
    }

    public enum PageEnum
    {
        HomePage = 0, LikesPage = 1, ChatsPage = 2, SettingsPage = 3
    }
}
