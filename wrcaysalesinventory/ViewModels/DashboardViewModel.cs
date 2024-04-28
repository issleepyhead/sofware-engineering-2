using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using GalaSoft.MvvmLight;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using System;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Measure;
using LiveChartsCore.ConditionalDraw;

namespace wrcaysalesinventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private static readonly SolidColorPaint[] paints =
             [
                new (SKColors.Red),
                new (SKColors.Green),
                new (SKColors.Blue),
                new (SKColors.Yellow)
             ];

    public ISeries[] Series { get; set; } =
            {
            new ColumnSeries<DateTimePoint>
            {
                Values =
                [
                    new DateTimePoint(new DateTime(2021, 1, 1), 3),
                    new DateTimePoint(new DateTime(2021, 1, 2), 6),
                    new DateTimePoint(new DateTime(2021, 1, 3), 5),
                    new DateTimePoint(new DateTime(2021, 1, 4), 3),
                    new DateTimePoint(new DateTime(2021, 1, 5), 5),
                    new DateTimePoint(new DateTime(2021, 1, 6), 8),
                    new DateTimePoint(new DateTime(2021, 1, 8), 42),
                    new DateTimePoint(new DateTime(2021, 1, 9), 23),
                    new DateTimePoint(new DateTime(2021, 1, 10), 213),
                    new DateTimePoint(new DateTime(2021, 1, 11),326),
                    new DateTimePoint(new DateTime(2021, 1, 12), 632),
                    new DateTimePoint(new DateTime(2021, 1, 13), 6312),
                    new DateTimePoint(new DateTime(2021, 1, 14), 6321),
                    new DateTimePoint(new DateTime(2021, 1, 15), 6112),
                    new DateTimePoint(new DateTime(2021, 1, 16), 2136),
                    new DateTimePoint(new DateTime(2021, 1, 17), 3216),
                    new DateTimePoint(new DateTime(2021, 1, 18), 31256),
                    new DateTimePoint(new DateTime(2021, 1, 19), 74746)
                ]
            }        .OnPointMeasured(point =>
            {
                // this method is called for each point in the series
                // we can customize the visual here

                if (point.Visual is null) return;

                // get a paint from the array
                var paint = paints[point.Index % paints.Length];
                // set the paint to the visual
                point.Visual.Fill = paint;
            })
        };

        // You can use the DateTimeAxis class to define a date time based axis 

        // The first parameter is the time between each point, in this case 1 day 
        // you can also use 1 year, 1 month, 1 hour, 1 minute, 1 second, 1 millisecond, etc 

        // The second parameter is a function that receives the value and returns the label 
        public Axis[] XAxes { get; set; } =
        {
            new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yyyy")),
        };
    }
}
