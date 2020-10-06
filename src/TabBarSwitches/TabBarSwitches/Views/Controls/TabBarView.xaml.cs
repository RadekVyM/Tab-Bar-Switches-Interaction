using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TabBarSwitches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabBarView : ContentView
    {
        #region Private members

        double svgSize => 18;
        double tabViewSelectedWidth => 180;
        double tabViewDefaultWidth;
        double margin;
        List<double> positionsOnLeft;
        List<double> positionsOnRight;

        #endregion

        #region Constructor

        public TabBarView()
        {
            InitializeComponent();

            positionsOnLeft = new List<double>();
            positionsOnRight = new List<double>();

            SizeChanged += TabBarViewSizeChanged;

            var pageEnums = Enum.GetValues(typeof(PageEnum)).Cast<PageEnum>();

            foreach (var page in pageEnums)
            {
                string path = "";
                string colour = "";
                string lightColour = "";
                string text = "";

                switch (page)
                {
                    case PageEnum.HomePage:
                        {
                            path = "HomePath";
                            colour = "Green";
                            lightColour = "LightGreen";
                            text = "Home";
                        }
                        break;
                    case PageEnum.LikesPage:
                        {
                            path = "LikesPath";
                            colour = "Pink";
                            lightColour = "LightPink";
                            text = "Likes";
                        }
                        break;
                    case PageEnum.ChatsPage:
                        {
                            path = "ChatsPath";
                            colour = "Blue";
                            lightColour = "LightBlue";
                            text = "Chats";
                        }
                        break;
                    case PageEnum.SettingsPage:
                        {
                            path = "SettingsPath";
                            colour = "Purple";
                            lightColour = "LightPurple";
                            text = "Settings";
                        }
                        break;
                }

                var svg = new TabSvgView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = svgSize * 2d,
                    Colour = App.Current.Resources.GetValue<Color>(colour),
                    LightColour = App.Current.Resources.GetValue<Color>(lightColour),
                    DefaultColour = App.Current.Resources.GetValue<Color>("DefaultGray"),
                    Path = App.Current.Resources.GetValue<string>(path),
                    SvgHeight = svgSize,
                    SvgWidth = svgSize,
                    Page = page,
                    Expanded = page == PageEnum.HomePage,
                    Text = text
                };

                svg.SetOpacity(page == PageEnum.HomePage ? 1d : 0.1d);

                TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += RecognizerTapped;

                svg.GestureRecognizers.Add(recognizer);

                absoluteLayout.Children.Add(svg);
            }
        }

        #endregion

        #region Private methods

        private void TabBarViewSizeChanged(object sender, EventArgs e)
        {
            double d = (absoluteLayout.Width - tabViewSelectedWidth) / 3;

            tabViewDefaultWidth = d < 100 ? d : 100;

            margin = (absoluteLayout.Width - ((tabViewDefaultWidth * 3) + tabViewSelectedWidth)) / 2;

            positionsOnLeft.Clear();
            positionsOnRight.Clear();

            positionsOnLeft.Add(margin);
            positionsOnRight.Add(margin);

            for (int i = 0; i < 3; i++)
            {
                positionsOnLeft.Add(margin + ((i + 1) * tabViewDefaultWidth));
                positionsOnRight.Add(margin + (tabViewSelectedWidth + (i * tabViewDefaultWidth)));
            }

            foreach (var view in absoluteLayout.Children)
            {
                TabSvgView svgView = view as TabSvgView;

                svgView.DefaultWidthChanged(tabViewDefaultWidth, tabViewSelectedWidth);

                var expandedView = absoluteLayout.Children.FirstOrDefault(v => (v as TabSvgView).Expanded == true) as TabSvgView;

                bool left = expandedView?.Page >= svgView.Page;

                double x = left ? positionsOnLeft[(int)svgView.Page] : positionsOnRight[(int)svgView.Page];
                double y = (absoluteLayout.Height - svgView.Height) / 2;
                double width = svgView.Expanded ? tabViewSelectedWidth : tabViewDefaultWidth;
                double height = svgView.Height;

                svgView.WidthRequest = width;

                AbsoluteLayout.SetLayoutBounds(svgView, new Rectangle(x, y, width, height));
                svgView.SetOpacity(svgView.Expanded ? 1d : 0);
            }
        }

        private async void RecognizerTapped(object sender, EventArgs e)
        {
            TabSvgView oldView = absoluteLayout.Children.FirstOrDefault(v => (bool)(v as TabSvgView)?.Expanded) as TabSvgView;
            TabSvgView newView = sender as TabSvgView;

            if (newView == oldView)
                return;

            _ = Shell.Current.GoToAsync("///" + newView.Page.ToString());

            absoluteLayout.Children.ForEach(v => (v as TabSvgView).Expanded = false);
            newView.Expanded = true;

            List<Task> tasks = new List<Task>();

            tasks.Add(oldView.UpdateValues(false));
            tasks.Add(newView.UpdateValues(true));

            foreach (var view in absoluteLayout.Children)
            {
                TabSvgView tabSvgView = view as TabSvgView;

                bool left = newView?.Page >= tabSvgView.Page;

                if (tabSvgView != newView)
                    tasks.Add(tabSvgView.LayoutTo(new Rectangle(left ? positionsOnLeft[(int)tabSvgView.Page] : positionsOnRight[(int)tabSvgView.Page], tabSvgView.Y, tabViewDefaultWidth, tabSvgView.Height)));
                else
                    tasks.Add(newView.LayoutTo(new Rectangle(positionsOnLeft[(int)newView.Page], newView.Y, tabViewSelectedWidth, newView.Height)));
            }

            await Task.WhenAll(tasks);
        }

        private void BackCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;
            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                paint.Color = App.Current.Resources.GetValue<Color>("TabBarGray").ToSKColor();
                paint.IsAntialias = true;

                float corner = (float)(svgSize * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density);

                canvas.DrawRoundRect(0, 0, info.Width, info.Height, corner, corner, paint);
                canvas.DrawRect(0, corner, info.Width, info.Height, paint);
            }
        }

        #endregion
    }
}