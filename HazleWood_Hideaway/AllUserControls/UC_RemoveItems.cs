﻿using Guna.UI2.WinForms;
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
    public partial class UC_RemoveItems : UserControl
    {

        function fn = new function();
        String query;
        public UC_RemoveItems()
        {
            InitializeComponent();
        }

        private void UC_RemoveItems_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            query = "select * from items";
            DataSet ds = fn.getdata(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            query = "select * from items where name like '" + txtSearch.Text + "%'";
            DataSet ds = fn.getdata(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("Delete Item?", "Important Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                int id = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                query = "delete from items where iid=" + id + "";
                fn.setDta(query);
                loadData();
            }
        }
    }
}
