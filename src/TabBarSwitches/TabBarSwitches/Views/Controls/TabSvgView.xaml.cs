using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TabBarSwitches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabSvgView : ContentView
    {
        #region Public members

        public PageEnum Page { get; set; }

        public bool Expanded { get; set; }

        public object Colour
        {
            get => GetValue(ColourProperty);
            set => SetValue(ColourProperty, value);
        }

        public object LightColour
        {
            get => GetValue(LightColourProperty);
            set => SetValue(LightColourProperty, value);
        }

        public object DefaultColour
        {
            get => GetValue(DefaultColourProperty);
            set => SetValue(DefaultColourProperty, value);
        }

        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public double SvgWidth
        {
            get => (double)GetValue(SvgWidthProperty);
            set => SetValue(SvgWidthProperty, value);
        }

        public double SvgHeight
        {
            get => (double)GetValue(SvgHeightProperty);
            set => SetValue(SvgHeightProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty ColourProperty =
            BindableProperty.Create(nameof(Colour), typeof(object), typeof(TabSvgView), Color.Black, BindingMode.OneWay, propertyChanged: MyPropertyChanged, coerceValue: (bindable, value) =>
            {
                TabSvgView view = bindable as TabSvgView;

                view.label.TextColor = value.GetColour();

                return value;
            });

        public static readonly BindableProperty LightColourProperty =
            BindableProperty.Create(nameof(LightColour), typeof(object), typeof(TabSvgView), Color.Black, BindingMode.OneWay);

        public static readonly BindableProperty DefaultColourProperty =
            BindableProperty.Create(nameof(DefaultColour), typeof(object), typeof(TabSvgView), Color.Black, BindingMode.OneWay, propertyChanged: MyPropertyChanged);

        public static readonly BindableProperty PathProperty =
            BindableProperty.Create(nameof(Path), typeof(string), typeof(TabSvgView), "", BindingMode.OneWay, propertyChanged: MyPropertyChanged);

        public static readonly BindableProperty SvgWidthProperty =
            BindableProperty.Create(nameof(SvgWidth), typeof(double), typeof(TabSvgView), 20d, BindingMode.OneWay, propertyChanged: MyPropertyChanged);

        public static readonly BindableProperty SvgHeightProperty =
            BindableProperty.Create(nameof(SvgHeight), typeof(double), typeof(TabSvgView), 20d, BindingMode.OneWay, propertyChanged: MyPropertyChanged);

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TabSvgView), "", BindingMode.OneWay);

        private static void MyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TabSvgView svgView = bindable as TabSvgView;

            try
            {
                svgView.canvasView.InvalidateSurface();
            }
            catch { };
        }

        #endregion

        #region Constructor

        public TabSvgView()
        {
            InitializeComponent();

            SizeChanged += TabSvgViewSizeChanged;
        }

        #endregion

        #region Public members

        public void DefaultWidthChanged(double defaultWidth, double selectedWidth)
        {
            canvasView.Margin = new Thickness((defaultWidth - canvasView.Height) / 2d, 0, 0, 0);
            backBoxView.Margin = new Thickness((defaultWidth - Height) / 3d, 0);
            label.Margin = new Thickness(((selectedWidth - canvasView.Margin.Left - backBoxView.Margin.Right - canvasView.Width) / 2d) - (80 / 2) + canvasView.Width + canvasView.Margin.Left - 10, 0, 0, 0);
        }

        public void ChangeBoxViewWidth(double width)
        {
            backBoxView.WidthRequest = width;
        }

        public async Task UpdateValues(bool expanded)
        {
            Expanded = expanded;

            canvasView.InvalidateSurface();

            uint animLength = 250;

            double opacity = expanded ? 1 : 0;

            Animation animation = new Animation();

            animation.Add(0, 1, new Animation(v =>
            {
                backBoxView.Opacity = v;
            }, backBoxView.Opacity, opacity));
            animation.Add(expanded ? 0.7d : 0d, expanded ? 1d : 0.3d, new Animation(v =>
            {
                label.Opacity = v;
            }, backBoxView.Opacity, opacity));

            animation.Commit(this, "OpacityAnimation", length: animLength, finished: (d, b) =>
            {
                backBoxView.Opacity = opacity;
                label.Opacity = opacity;
            });
        }

        public void SetOpacity(double opacity)
        {
            backBoxView.Opacity = opacity;
            label.Opacity = opacity;
        }

        #endregion

        #region Private members

        private void TabSvgViewSizeChanged(object sender, EventArgs e)
        {
            backBoxView.CornerRadius = (float)Height / 2;
            backBoxView.HeightRequest = Height;
        }

        private void CanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;
            canvas.Clear();

            if (string.IsNullOrWhiteSpace(Path))
                return;

            SKPath path = SKPath.ParseSvgPathData(Path);

            using (SKPaint paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = Expanded ? Colour.GetColour().ToSKColor() : DefaultColour.GetColour().ToSKColor();
                paint.StrokeCap = SKStrokeCap.Round;
                paint.StrokeJoin = SKStrokeJoin.Round;
                paint.IsAntialias = true;

                path.GetBounds(out SKRect bounds);

                canvas.Translate(info.Width / 2, info.Height / 2);
                canvas.Scale(Math.Min((float)(info.Width / bounds.Width), (float)(info.Height / bounds.Height)));
                canvas.Translate(-bounds.MidX, -bounds.MidY);

                canvas.DrawPath(path, paint);
            };
        }

        #endregion
    }
}