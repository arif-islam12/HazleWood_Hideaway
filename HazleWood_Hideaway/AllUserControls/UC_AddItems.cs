using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_AddItems : UserControl
    {
        function fn = new function();
        String query;
        public UC_AddItems()
        {
            InitializeComponent();
        }
        public void clearAll()
        {
            txtCatagory.SelectedIndex = -1;
            txtItemName.Clear();
            txtPrice.Clear();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            query = "insert into items (name,catagory,price) values('" + txtItemName.Text + "','" + txtCatagory.Text + "','" + txtPrice.Text + "')";
            fn.setDta(query);

            clearAll();
        }
        

        private void UC_AddItems_Leave_1(object sender, EventArgs e)
        {
            clearAll();
        }
    }
}
