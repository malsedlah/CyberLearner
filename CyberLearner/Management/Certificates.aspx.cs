using System;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class Certificates : System.Web.UI.Page
    {
        CertificateService certificateService = new CertificateService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCertificates();
            }
        }

        private void LoadCertificates()
        {
            gvCertificates.DataSource = certificateService.GetAllCertificates(txtSearch.Text.Trim());
            gvCertificates.DataBind();

            lblTotal.Text = certificateService.CountCertificates().ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCertificates();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadCertificates();
        }
    }
}
