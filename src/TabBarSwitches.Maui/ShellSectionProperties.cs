using Microsoft.Maui.Controls.Shapes;

namespace TabBarSwitches.Maui
{
    public static class ShellProperties
    {
        public static readonly BindableProperty IconGeometryProperty =
            BindableProperty.CreateAttached("IconGeometry", typeof(Geometry), typeof(BaseShellItem), default(Geometry));

        public static readonly BindableProperty PrimarySelectionColorProperty =
            BindableProperty.CreateAttached("PrimarySelectionColor", typeof(Color), typeof(BaseShellItem), Colors.Black);

        public static readonly BindableProperty SecondarySelectionColorProperty =
            BindableProperty.CreateAttached("SecondarySelectionColor", typeof(Color), typeof(BaseShellItem), Colors.Gray);

        public static readonly BindableProperty PageTypeProperty =
            BindableProperty.CreateAttached("PageType", typeof(PageType), typeof(BaseShellItem), default(PageType));

        public static Geometry GetIconGeometry(BindableObject item)
        {
            return (Geometry)item.GetValue(IconGeometryProperty);
        }

        public static void SetIconGeometry(BindableObject item, Geometry value)
        {
            item.SetValue(IconGeometryProperty, value);
        }

        public static Color GetPrimarySelectionColor(BindableObject item)
        {
            return (Color)item.GetValue(PrimarySelectionColorProperty);
        }

        public static void SetPrimarySelectionColor(BindableObject item, Color value)
        {
            item.SetValue(PrimarySelectionColorProperty, value);
        }

        public static Color GetSecondarySelectionColor(BindableObject item)
        {
            return (Color)item.GetValue(SecondarySelectionColorProperty);
        }

        public static void SetSecondarySelectionColor(BindableObject item, Color value)
        {
            item.SetValue(SecondarySelectionColorProperty, value);
        }

        public static PageType GetPageType(BindableObject item)
        {
            return (PageType)item.GetValue(PageTypeProperty);
        }

        public static void SetPageType(BindableObject item, PageType value)
        {
            item.SetValue(PageTypeProperty, value);
        }
    }
}
