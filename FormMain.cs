using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Common;

namespace PassBook
{
    public partial class FormMain : Form
    {
        string DATABASE_NAME = @"data.db";

        SQLiteConnection connection;

        public FormMain()
        {
            InitializeComponent();
        }

        private void UpdateList()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT [rowid], [equipment], [cabinet], [nameUser] FROM [records];", connection);
            SQLiteDataReader reader = command.ExecuteReader();

            listView.Items.Clear();
            foreach (DbDataRecord record in reader)
            {
                var lvi = new ListViewItem();
                lvi.Text = reader["equipment"].ToString();
                lvi.Tag = reader["rowid"].ToString();
                lvi.SubItems.Add(reader["cabinet"].ToString());

                var lvsi = new ListViewItem.ListViewSubItem();
                lvsi.Text = reader["nameUser"].ToString();
                lvsi.Tag = reader["nameUser"].ToString();
                lvi.SubItems.Add(lvsi);

                listView.Items.Add(lvi);
            }
        }

        private void EditItem()
        {
            if (listView.SelectedItems.Count > 0)
            {
                FormRecord formRecord = new FormRecord();

                formRecord.addr = listView.SelectedItems[0].Text;
                formRecord.login = listView.SelectedItems[0].SubItems[1].Text;
                formRecord.password = (String)listView.SelectedItems[0].SubItems[2].Tag;

                var id = (String)listView.SelectedItems[0].Tag;

                if (formRecord.ShowDialog() == DialogResult.OK)
                {
                    var sql = String.Format(
                        "UPDATE [records]" + "" +
                        "  SET [equipment] = '{0}', [cabinet] = '{1}', [nameUser] = '{2}'" +
                        "  WHERE [rowid] = {3}",
                        formRecord.addr, formRecord.login, formRecord.password, id);

                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();

                    UpdateList();
                }
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            connection = new SQLiteConnection(string.Format("Data Source={0};", DATABASE_NAME));
            connection.Open();

            UpdateList();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormRecord formRecord = new FormRecord();

            if (formRecord.ShowDialog() == DialogResult.OK)
            {
                var sql = String.Format("INSERT INTO [records]([equipment], [cabinet], [nameUser]) VALUES ('{0}', '{1}', '{2}')",
                    formRecord.addr, formRecord.login, formRecord.password);

                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                UpdateList();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            //EditItem();
            MessageBox.Show(listView.SelectedItems[0].Text + " " + listView.SelectedItems[0].SubItems[1].Text + " " + (String)listView.SelectedItems[0].SubItems[2].Tag);

        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT [rowid], [equipment], [cabinet], [nameUser] FROM [records] WHERE [equipment] like '%" + textBoxSearch.Text + "%'", connection);
            SQLiteDataReader reader = command.ExecuteReader();

            listView.Items.Clear();
            foreach (DbDataRecord record in reader)
            {
                var lvi = new ListViewItem();
                lvi.Text = reader["equipment"].ToString();
                lvi.Tag = reader["rowid"].ToString();
                lvi.SubItems.Add(reader["cabinet"].ToString());

                var lvsi = new ListViewItem.ListViewSubItem();
                lvsi.Text = reader["nameUser"].ToString();
                lvsi.Tag = reader["nameUser"].ToString();
                lvi.SubItems.Add(lvsi);

                listView.Items.Add(lvi);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var id = (string)listView.SelectedItems[0].Tag;
            var sql = string.Format("DELETE FROM [records] WHERE [rowid] = {0}", id);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            UpdateList();
        }
    }
}