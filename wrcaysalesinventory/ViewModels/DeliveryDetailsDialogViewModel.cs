using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.ViewModels
{
    public class DeliveryDetailsDialogViewModel : ViewModelBase
    {
        private DeliveryModel _model;
        public DeliveryModel Model { get => _model; set => Set(ref _model, value); }
        private ObservableCollection<DeliveryCartModel> _dataList;
        public ObservableCollection<DeliveryCartModel> DataList { get => _dataList; set => Set(ref _dataList, value); }
        public RelayCommand<object> LoadedCmd => new(LoadedCommand);
        private void LoadedCommand(object obj)
        {
            try
            {
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand = new(@"SELECT dp.id,
                                                dp.product_id,
                                                p.product_name,
                                                dp.quantity,
                                                p.product_cost,
                                                dp.total
                                            FROM tbldeliveryproducts dp
                                            JOIN tblproducts p ON dp.product_id = p.id
                                            WHERE header_id = @hid", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@hid", Model.ID);
                DataTable dt = new();
                SqlDataAdapter adapter = new(sqlCommand);

                adapter.Fill(dt);

                ObservableCollection<DeliveryCartModel> list = new();
                foreach(DataRow row in dt.Rows)
                {
                    DeliveryCartModel dmodel = new()
                    {
                        ID = row["id"].ToString(),
                        ProductID = row["product_id"].ToString(),
                        ProductName = row["product_name"].ToString(),
                        Cost = row["product_cost"].ToString(),
                        Quantity = row["quantity"].ToString(),
                        TotalDue = row["total"].ToString()
                    };
                    list.Add(dmodel);
                }
                DataList = list;
            } catch
            {
                Debug.WriteLine("Delivery Product Error");
            }
        }
    }
}
