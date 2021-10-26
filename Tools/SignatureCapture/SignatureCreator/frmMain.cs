using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SignatureCreator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }       

        private void btnSelect_Click(object sender, EventArgs e)
        {
            rtxtImageData.Text = "";
            txtSignaturePath.Text = "";

            try
            {
                if (DialogResult.OK == ofdImageSelector.ShowDialog())
                {
                    txtSignaturePath.Text = ofdImageSelector.FileName;

                    Image signatureImage = Image.FromFile(ofdImageSelector.FileName);

                    if (signatureImage != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            signatureImage.Save(ms, ImageFormat.Png);

                            string base64String = Convert.ToBase64String(ms.ToArray());

                            if (!String.IsNullOrEmpty(base64String))
                            {
                                rtxtImageData.Text = base64String;
                            }
                            else
                            {
                                MessageBox.Show("Unable to convert into base64 format!!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid image type!!!!");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}