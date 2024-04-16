using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using GalaSoft.MvvmLight;
using LiveChartsCore.SkiaSharpView.VisualElements;

namespace wrcaysalesinventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {

        }

        public ISeries[] Series { get; set; } =
            {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };

        public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "My chart title",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),

        };
    }
}
