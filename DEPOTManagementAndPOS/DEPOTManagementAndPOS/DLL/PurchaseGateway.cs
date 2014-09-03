using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEPOTManagementAndPOS.Model;

namespace DEPOTManagementAndPOS.DLL
{
    class PurchaseGateway
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        private Purchase _aPurchase;
        public PurchaseGateway()
        {
            _connection = new SqlConnection();
            _connection.ConnectionString = ConfigurationManager.ConnectionStrings["DepotDB"].ConnectionString;

        }
        public List<Purchase> GetProductNameWithCategoryAndBrandName()
        {
            
            List<Purchase> _aPurchaseList = new List<Purchase>();

            _connection.Open();
            string query = string.Format("SELECT CategoryEntryTable.Name, BrandEntryTable.Name, ProductNameExtentionEntryTable.ProductNameExtention " +
                                         "FROM ProductNameExtentionEntryTable " +
                                         "JOIN CategoryEntryTable " +
                                         "ON ProductNameExtentionEntryTable.CategoryId = CategoryEntryTable.CategoryId " +
                                         "JOIN BrandEntryTable " +
                                         "ON ProductNameExtentionEntryTable.BrandId = BrandEntryTable.BrandId");
            _command = new SqlCommand(query, _connection);
            SqlDataReader aReader = _command.ExecuteReader();

            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    _aPurchase = new Purchase();
                    _aPurchase.CategoryEntry.Name = (aReader[0]).ToString();
                    _aPurchase.BrandEntry.Name = (string)aReader[1];
                    _aPurchase.Product.ProductNameExtention = (string)aReader[2];

                    _aPurchaseList.Add(_aPurchase);
                }
            }
            _connection.Close();
            return _aPurchaseList;
        }

        public bool SaveAllPurchaseInfo(Purchase aPurchase)
        {
            _connection.Open();
            string query = string.Format("INSERT INTO PurchaseTable VALUES ('{0}', {1}, {2}, {3}, '{4}')", aPurchase.ProductName, 
                aPurchase.Price, aPurchase.Quantity, aPurchase.TotalPurchasePrice, aPurchase.DateTime);

            _command = new SqlCommand(query, _connection);
            int affectedRows = _command.ExecuteNonQuery();
            _connection.Close();
            if (affectedRows > 0)
            {
                return true;
            }

            return false;
        }
    }
}
