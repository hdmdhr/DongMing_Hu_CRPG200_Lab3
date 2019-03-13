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
    /*
     * Author: DongMing Hu
     * Date: Mar. 9, 2019
     * Purpose: Load and save data from / to database, handle all possible errors.
     * 
     */

    public partial class frmNorthwind : Form
    {
        public frmNorthwind()
        {
            InitializeComponent();
        }
        
        // Save Button Clicked: try to save, catch all possible errors
        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.productsBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.northwindDataSet);
                MessageBox.Show("Your changes are saved successfully.", "Save Succeed");
            }
            catch (DBConcurrencyException)
            {
                MessageBox.Show("Some rows were not updated due to concurrency error, please try again.", "Concurrency Exception");
                this.order_DetailsTableAdapter.Fill(this.northwindDataSet.Order_Details);
                this.productsTableAdapter.Fill(this.northwindDataSet.Products);
            }
            catch (DataException ex)
            {
                // include ConstraintException, NoNullAllowedException
                MessageBox.Show(ex.Message, ex.GetType().ToString());
                productsBindingSource.CancelEdit();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database load error # " + ex.Number + ": " + ex.Message, ex.GetType().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Other error while saving data: " + ex.Message, "Others Error");
            }
        }

        // When Form Loaded: fill the dataset tables
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

        // When Data Binding Has Error: don't know how to trigger this though
        private void productsBindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }

        // When Validating Units in Stock: if cannot convert to int, show error message
        private void unitsInStockTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!Int32.TryParse(unitsInStockTextBox.Text,out int i))
            {
                MessageBox.Show("Units need to be a whole number!", "Invalid Input");
                e.Cancel = true;
            }
        }

        // When Text Change in Name TB: resize font size to fit textbox
        private void productNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // get the text width of Product Name textbox
            var textWidth = System.Windows.Forms.TextRenderer.MeasureText(productNameTextBox.Text, new Font(productNameTextBox.Font.FontFamily, productNameTextBox.Font.Size, productNameTextBox.Font.Style)).Width;
            // if text doesn't fit in textbox(too big), reduce font size
            if (productNameTextBox.Width < textWidth)
            {
                while (productNameTextBox.Width < textWidth &
                    productNameTextBox.Font.SizeInPoints > 10 )
                    productNameTextBox.Font = new Font(productNameTextBox.Font.FontFamily, productNameTextBox.Font.Size - 0.5f, productNameTextBox.Font.Style);
            }
            else  // if text is too small, increase font size
            {
                while (productNameTextBox.Width > textWidth &
                    productNameTextBox.Font.SizeInPoints < 12)
                    productNameTextBox.Font = new Font(productNameTextBox.Font.FontFamily, productNameTextBox.Font.Size + 0.5f, productNameTextBox.Font.Style);
            }
        }
    }
}
