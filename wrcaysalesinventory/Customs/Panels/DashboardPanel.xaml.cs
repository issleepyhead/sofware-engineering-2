using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for DashboardPanel.xaml
    /// </summary>
    public partial class DashboardPanel : Grid
    {
        public DashboardPanel()
        {
            InitializeComponent();
        }

        public ISeries[] Series { get; set; } =
    {
            new ColumnSeries<DateTimePoint>
            {
                Values = null
            }
        };

        // You can use the DateTimeAxis class to define a date time based axis 

        // The first parameter is the time between each point, in this case 1 day 
        // you can also use 1 year, 1 month, 1 hour, 1 minute, 1 second, 1 millisecond, etc 

        // The second parameter is a function that receives the value and returns the label 
        public Axis[] XAxes { get; set; } =
        {
            new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("MMMM dd"))
        };
    }
}

//[
//                    new DateTimePoint(new DateTime(2021, 1, 1), 3),
//                    new DateTimePoint(new DateTime(2021, 1, 2), 6),
//                    new DateTimePoint(new DateTime(2021, 1, 3), 5),
//                    new DateTimePoint(new DateTime(2021, 1, 4), 3),
//                    new DateTimePoint(new DateTime(2021, 1, 5), 5),
//                    new DateTimePoint(new DateTime(2021, 1, 6), 8),
//                    new DateTimePoint(new DateTime(2021, 1, 7), 6)
//                ]
