using Microsoft.Maui.Controls.Shapes;

namespace TabBarSwitches.Maui
{
    public class PathGeometryExtension : IMarkupExtension<Geometry>
    {
        public string Path { get; set; }

        public Geometry ProvideValue(IServiceProvider serviceProvider)
        {
            var pathGeometryConverter = new PathGeometryConverter();
            var path = pathGeometryConverter.ConvertFromInvariantString(Path) as Geometry;

            return path;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<Geometry>).ProvideValue(serviceProvider);
        }
    }
}
