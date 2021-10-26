using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SignatureDashboard
{
    public partial class frmImagesStore : Form
    {
        public frmImagesStore()
        {
            InitializeComponent();
        }

        //Get table rows from sql server to be displayed in Datagrid.
        void GetImagesFromDatabase()
        {
            try
            {
                //Initialize SQL Server connection.
                SqlConnection CN = new SqlConnection(txtConnectionString.Text);

                //Initialize SQL adapter.
                SqlDataAdapter ADAP = new SqlDataAdapter("Select CustomerSignature from CCTransactionAdditionalInfo", CN);

                //Initialize Dataset.
                DataSet DS = new DataSet();

                //Fill dataset with ImagesStore table.
                ADAP.Fill(DS, "ImagesStore");

                //Fill Grid with dataset.
                dataGridView1.DataSource = DS.Tables["ImagesStore"];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            //Fill datagrid with table rows.
            GetImagesFromDatabase();
        }       

        //When user changes row selection, display image of selected row in picture box.
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!DBNull.Value.Equals(dataGridView1.Rows[e.RowIndex].Cells[0].Value))
                {
                    //Get image data from gridview column.
                    byte[] imageData = (byte[])dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                    //Initialize image variable
                    Image newImage;
                    //Read image data into a memory stream
                    using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                    {
                        ms.Write(imageData, 0, imageData.Length);

                        //Set image variable value using memory stream.
                        newImage = Image.FromStream(ms, true);
                    }

                    //set picture
                    pictureBox1.Image = newImage;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}