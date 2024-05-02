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
using GalaSoft.MvvmLight.Command;
using LiveChartsCore.Kernel.Sketches;
using System.Threading.Tasks;
using System.Data.SqlClient;
using wrcaysalesinventory.Data.Classes;
using System.Data;
using HandyControl.Tools.Extension;
using System.Collections.Generic;
using System.Windows.Documents;

namespace wrcaysalesinventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            GenerateSeries();
        }

        public void GenerateSeries()
        {
            ColumnSeries<DateTimePoint> weekly = new();
            //ColumnSeries<DateTimePoint> monthlyy = new();
            //ColumnSeries<DateTimePoint> yearly = new();

            ObservableCollection<DateTimePoint> sweekly = new();
            //ObservableCollection<DateTimePoint> smonthly = new();
            //ObservableCollection<DateTimePoint> syearly = new();


            SqlConnection conn = SqlBaseConnection.GetInstance();
            DateTime d = DateTime.Now;
            for (int i = 7; i > 0; i--)
            {
                SqlCommand cmd = new("SELECT COUNT(*) FROM tbltransactionheaders WHERE FORMAT(date_added,'dd/MM/yyyy') = FORMAT(@date_added, 'dd/MM/yyyy')", conn);

                cmd.Parameters.AddWithValue("@date_added", d).SqlDbType = SqlDbType.DateTime;

                double res = double.Parse(cmd.ExecuteScalar().ToString());
                sweekly.Add(new DateTimePoint(d, res));
                d = DateTime.Now.AddDays(-i);
            }

            SqlCommand ncmd = new("SELECT COUNT(*) FROM tblproducts", conn);
            TotalProducts = (int)ncmd.ExecuteScalar();

            ncmd = new("SELECT CASE WHEN SUM(total_amount) IS NULL THEN 0 ELSE SUM(total_amount) END AS revenue FROM tbltransactionheaders", conn);
            TotalRevenue = double.Parse(ncmd.ExecuteScalar().ToString());

            ncmd = new("SELECT  CASE WHEN SUM(due_total) IS NULL THEN 0 ELSE SUM(due_total)  END AS expenses FROM tbldeliveryheaders", conn);
            TotalExpenses = double.Parse(ncmd.ExecuteScalar().ToString());

            weekly.Values = sweekly;
            Series.Add(weekly);

        }
        private int _totalProduct = 0;
        private double _totalRevenue = 0;
        private double _totalExpenses = 0;

        private int _weekly = DateTime.Now.Day;
        private int _monthly = DateTime.Now.Month;
        private int _yearly = DateTime.Now.Year;

        public int TotalProducts { get => _totalProduct; set => Set(ref _totalProduct, value); }
        public double TotalRevenue { get => _totalRevenue; set => Set(ref _totalRevenue, value); }
        public double TotalExpenses { get => _totalExpenses; set => Set(ref _totalExpenses, value); }


        private static readonly SolidColorPaint[] paints =
        {
                new (SKColors.Red),
                new (SKColors.Green),
                new (SKColors.Blue),
                new (SKColors.Yellow)
        };

        public List<ISeries> Series { get; set; } = new();


        public Axis[] XAxes { get; set; } =
        {
             new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yyyy")),
        };

        //    {
        //    new ColumnSeries<DateTimePoint>
        //    {
        //        Values = null
        //    }        .OnPointMeasured(point =>
        //    {
        //        // this method is called for each point in the series
        //        // we can customize the visual here

        //        if (point.Visual is null) return;

        //        // get a paint from the array
        //        var paint = paints[point.Index % paints.Length];
        //        // set the paint to the visual
        //        point.Visual.Fill = paint;
        //    })
        //};

        // You can use the DateTimeAxis class to define a date time based axis 

        // The first parameter is the time between each point, in this case 1 day 
        // you can also use 1 year, 1 month, 1 hour, 1 minute, 1 second, 1 millisecond, etc 

        // The second parameter is a function that receives the value and returns the label 
        //public Axis[] XAxes { get; set; } =
        //{
        //    new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yyyy")),
        //};

        //public RelayCommand<object> Daily => new(DailySeries);
        //public void DailySeries(object obj)
        //{
        //    Series[0].IsVisible = !Series[0].IsVisible;
        //}

        //public RelayCommand<object> Weekly => new(WeeklySeries);
        //public void WeeklySeries(object obj)
        //{
        //    Series[0].IsVisible = !Series[1].IsVisible;
        //}

        //public RelayCommand<object> Monthly => new(MonthlySeries);
        //public void MonthlySeries(object obj)
        //{
        //    Series[0].IsVisible = !Series[2].IsVisible;
        //}

        //public RelayCommand<object> Yearly => new(YearlySeries);
        //public void YearlySeries(object obj)
        //{
        //    Series[0].IsVisible = !Series[3].IsVisible;
        //}
    }
}
