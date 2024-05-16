using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel()
        {
            GenerateSeries();
            GenerateNotifok();
        }

        private ObservableCollection<NotifItemModle> notifItemModles = new();
        public ObservableCollection<NotifItemModle> CardList { get => notifItemModles; set => Set(ref notifItemModles, value); }

        public void GenerateNotifok()
        {
            try
            {
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand = new("SELECT product_name FROM tblinventory i JOIN tblproducts p ON i.product_id = p.id WHERE stocks <= p.critical_level", sqlConnection);
                SqlDataAdapter adapter = new(sqlCommand);
                DataTable dataTable = new();
                adapter.Fill(dataTable);

                ObservableCollection<NotifItemModle> temp = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    NotifItemModle notifItems = new()
                    {
                        ProductName = row["product_name"].ToString()
                    };
                    temp.Add(notifItems);
                }
                CardList = temp;
            } catch
            {
                Debug.WriteLine("An error occured in notif");
            }
        }

        public void GenerateSeries()
        {
            Series.Clear();
            ColumnSeries<DateTimePoint> serie= new();
            ColumnSeries<DateTimePoint> cweekly = new();
            ColumnSeries<DateTimePoint> monthlyy = new();
            //ColumnSeries<DateTimePoint> yearly = new();


            ObservableCollection<DateTimePoint> values = new();
            ObservableCollection<DateTimePoint> vweekly = new();
            ObservableCollection<DateTimePoint> vmonthly = new();
            ObservableCollection<DateTimePoint> syearly = new();

            try
            {
                SqlConnection conn = SqlBaseConnection.GetInstance();
                DateTime today = DateTime.Now;
                
                for(int i = 1; i < 24; i++)
                {
                    SqlCommand cmd = new("SELECT COUNT(*) FROM tbltransactionheaders WHERE date_added BETWEEN @stime AND @etime", conn);
                    cmd.Parameters.AddWithValue("@stime", today.AddHours(-1)).SqlDbType = SqlDbType.DateTime;
                    cmd.Parameters.AddWithValue("@etime", today).SqlDbType = SqlDbType.DateTime;
                    double res = double.Parse(cmd.ExecuteScalar().ToString());
                    values.Add(new DateTimePoint(today, res));
                    today = DateTime.Now.AddHours(-i);
                }

                serie.Values = values;
                Series.Add(serie);

                today = DateTime.Now;
                for (int i = 1; i < 7; i++)
                {
                    SqlCommand cmd = new("SELECT COUNT(*) FROM tbltransactionheaders WHERE FORMAT(date_added, 'dd/MM/yyyy') = FORMAT(@date_added, 'dd/MM/yyyy')", conn);
                    cmd.Parameters.AddWithValue("@date_added", today).SqlDbType = SqlDbType.DateTime;

                    double res = double.Parse(cmd.ExecuteScalar().ToString());
                    vweekly.Add(new DateTimePoint(today, res));
                    today = DateTime.Now.AddDays(-i);
                }

                cweekly.Values = vweekly;
                cweekly.IsVisible = false;
                Series.Add(cweekly);

                //serie = new();
                //serie.Values = values;
                //Series.Add(serie);
                //values.Clear();


                today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                for (int i = 1; i <= 12; i++)
                {
                    SqlCommand cmd = new("SELECT COUNT(*) FROM tbltransactionheaders WHERE MONTH(date_added) = MONTH(@date_added)", conn);
                    cmd.Parameters.AddWithValue("@date_added", today).SqlDbType = SqlDbType.DateTime;

                    double res = double.Parse(cmd.ExecuteScalar().ToString());
                    vmonthly.Add(new DateTimePoint(today, res));
                    today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i);
                }

                monthlyy.Values = vmonthly;
                monthlyy.IsVisible = false;
                Series.Add(monthlyy);

                SqlCommand ncmd = new("SELECT COUNT(*) FROM tblproducts WHERE product_status = (SELECT TOP 1 id FROM tblstatus WHERE status_name = 'Active');", conn);
                TotalProducts = (int)ncmd.ExecuteScalar();

                ncmd = new("SELECT CASE WHEN SUM(total_amount) IS NULL THEN 0 ELSE SUM(total_amount) END AS revenue FROM tbltransactionheaders", conn);
                TotalRevenue = double.Parse(ncmd.ExecuteScalar().ToString());

                ncmd = new("SELECT CASE WHEN SUM(due_total) IS NULL THEN 0 ELSE SUM(due_total) END AS expenses FROM tbldeliveryheaders", conn);
                TotalExpenses = double.Parse(ncmd.ExecuteScalar().ToString());

                
            } catch
            {
                Growl.Info("Unable to fetch data.");
            }

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
            new Axis
            {
                Labeler = val => new DateTime((long)val).ToString("dd/MM/yyyy hh:mm:ss tt"),
                IsVisible = true,
            },
            new Axis
            {
                Labeler = val => new DateTime((long)val).ToString("dd/MM/yyyy"),
                IsVisible = false,
            },
            new Axis
            {
                Labeler = val => new DateTime((long)val).ToString("dd/MM/yyyy"),
                IsVisible = false
            },
                                                                     new Axis
            {
                Labeler = val => new DateTime((long)val).ToString("dd/MM/yyyy"),
                IsVisible = false
            }
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

        public RelayCommand<object> Daily => new(DailySeries);
        public void DailySeries(object obj)
        {
            foreach (ColumnSeries<DateTimePoint> x in Series) x.IsVisible = false;
            foreach (Axis x in XAxes) x.IsVisible = false;
            Series[0].IsVisible = !Series[0].IsVisible;
            XAxes[0].IsVisible = !XAxes[0].IsVisible;
        }

        public RelayCommand<object> Weekly => new(WeeklySeries);
        public void WeeklySeries(object obj)
        {
            foreach (ColumnSeries<DateTimePoint> x in Series) x.IsVisible = false;
            foreach (Axis x in XAxes) x.IsVisible = false;
            Series[1].IsVisible = !Series[1].IsVisible;
            XAxes[1].IsVisible = !XAxes[1].IsVisible;
        }

        public RelayCommand<object> Monthly => new(MonthlySeries);
        public void MonthlySeries(object obj)
        {
            foreach (ColumnSeries<DateTimePoint> x in Series) x.IsVisible = false;
            foreach (Axis x in XAxes) x.IsVisible = false;
            Series[2].IsVisible = !Series[2].IsVisible;
            XAxes[2].IsVisible = !XAxes[2].IsVisible;
        }

        public RelayCommand<object> Yearly => new(YearlySeries);
        public void YearlySeries(object obj)
        {
            foreach (ColumnSeries<DateTimePoint> x in Series) x.IsVisible = false;
            //foreach (DateTimeAxis x in XAxes) x.IsVisible = false;
            Series[3].IsVisible = !Series[3].IsVisible;
            //XAxes[3].IsVisible = !XAxes[3].IsVisible;
        }
    }
}
