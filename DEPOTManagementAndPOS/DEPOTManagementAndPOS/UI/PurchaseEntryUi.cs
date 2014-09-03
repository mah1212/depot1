using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DEPOTManagementAndPOS.BLL;
using DEPOTManagementAndPOS.Model;

namespace DEPOTManagementAndPOS.UI
{
    public partial class PurchaseEntryUi : Form
    {
        public PurchaseEntryUi()
        {
            InitializeComponent();
        }

        private void viewProductInfoButton_Click(object sender, EventArgs e)
        {
            ViewProductInfoUI aViewProductInfoUi=new ViewProductInfoUI();
            aViewProductInfoUi.ShowDialog();


        }

        private void PurchaseEntryUi_Load(object sender, EventArgs e)
        {
            PurchaseManager _aPurchaseManager = new PurchaseManager();
            List<Purchase> _aPurchaseList = new List<Purchase>();
            string[] productNameFromJoinedDatabase = _aPurchaseManager.GetProductNameWithCategoryAndBrandName().Select(x => string.Format("{0} {1} {2}", x.CategoryEntry.Name, x.BrandEntry.Name, x.Product.ProductNameExtention)).ToArray();

            
            productNameTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            productNameTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(productNameFromJoinedDatabase);
            productNameTextBox.AutoCompleteCustomSource = autoComplete;

        }

        private void savePurchaseButton_Click(object sender, EventArgs e)
        {
            Purchase _aPurchase = new Purchase();
            PurchaseManager _aPurchaseManager = new PurchaseManager();
            bool savePurchaseSuccess = false;

            _aPurchase.ProductName = productNameTextBox.Text;
            _aPurchase.Price = Convert.ToDouble(unitPriceTextBox.Text);
            _aPurchase.Quantity = Convert.ToInt32(quantityTextBox.Text);
            
            totalPriceTextBox.Text = _aPurchase.GetTotalPurchasePrice().ToString();
            _aPurchase.TotalPurchasePrice = _aPurchase.GetTotalPurchasePrice();
            _aPurchase.DateTime = dateTimePicker.Value.Date;

            savePurchaseSuccess = _aPurchaseManager.SaveAllPurchaseInfo(_aPurchase);
            if (savePurchaseSuccess)
            {
                MessageBox.Show("Product Purchase info saved Successfully");
            }
            else
            {
                MessageBox.Show("Error saving Purchase info");
            }
        }

        private void quantityTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Purchase _aPurchase = new Purchase();
            _aPurchase.Price = Convert.ToDouble(unitPriceTextBox.Text);
            _aPurchase.Quantity = Convert.ToInt32(quantityTextBox.Text);
            totalPriceTextBox.Text = _aPurchase.GetTotalPurchasePrice().ToString();
        }

       
    }
}
