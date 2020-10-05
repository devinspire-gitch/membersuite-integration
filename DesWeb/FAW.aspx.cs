using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MemberSuite.SDK.Concierge;
using MemberSuite.SDK.Searching;
using MemberSuite.SDK.Searching.Operations;
using MemberSuite.SDK.Types;
using GoogleMaps.LocationServices;
namespace DesWeb
{
    public partial class FAW : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["paramshw"] = null;
            Session["cityStatehw"] = null;
            Session["ziphw"] = null;
            Session["companyhw"] = null;
            Session["reqinfocom"] = null;


            if (txtCity.Text != "") {
                Session["cityStatehw"] = this.txtCity.Text;
            }

            if (txtCompanyName.Text != "")
            {
                Session["companyhw"] = txtCompanyName.Text;
            }


            Session["mileshw"] = this.ddlMiles.SelectedValue;
            if (txtZipCode.Text != "")
            {
                Session["ziphw"] = this.txtZipCode.Text;
            }

            List<string> searchParams = new List<string>();
            for (int i = 0; i < cbKeyword.Items.Count; i++)
            {

                if (cbKeyword.Items[i].Selected)
                {

                   searchParams.Add(cbKeyword.Items[i].Text);

                }

            }

            if (cbKeyword.Items.Count > 0)
            {
                Session["paramshw"] = searchParams;
            }

            List<string> searchCertifs = new List<string>();
            for (int i = 0; i < cbCertifications.Items.Count; i++)
            {

                if (cbCertifications.Items[i].Selected)
                {

                    searchCertifs.Add(cbCertifications.Items[i].Text);

                }

            }

            if (cbCertifications.Items.Count > 0)
            {
                Session["certshw"] = searchCertifs;
            }

            Response.Redirect("SearchFAW.aspx");

        }

    
    }
}