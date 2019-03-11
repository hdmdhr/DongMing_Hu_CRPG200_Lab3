using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DongMing_Hu_CRPG200_Lab3
{
    public partial class frmNorthwind : Form
    {
        public frmNorthwind()
        {
            InitializeComponent();
        }
        

        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.productsBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.northwindDataSet);
            }
            catch (DBConcurrencyException)
            {

                MessageBox.Show("Some rows were not updated due concurrency error, please try again.", "Concurrency Exception");
                this.order_DetailsTableAdapter.Fill(this.northwindDataSet.Order_Details);
                this.productsTableAdapter.Fill(this.northwindDataSet.Products);
            }
            catch (DataException ex)
            {
                // include ConstraintEx, NoNullAllowedEx
                MessageBox.Show(ex.Message, ex.GetType().ToString());
                productsBindingSource.CancelEdit();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error # " + ex.Number + ": " + ex.Message, ex.GetType().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Other error while saving data: " + ex.Message, "Others Error");
            }

        }

        private void frmNorthwind_Load(object sender, EventArgs e)
        {
            try
            {
                this.order_DetailsTableAdapter.Fill(this.northwindDataSet.Order_Details);
                this.categoriesTableAdapter.Fill(this.northwindDataSet.Categories);
                this.suppliersTableAdapter.Fill(this.northwindDataSet.Suppliers);
                this.productsTableAdapter.Fill(this.northwindDataSet.Products);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database load error # "+ex.Number+": "+ex.Message, ex.GetType().ToString());
            }

        }
    }
}
