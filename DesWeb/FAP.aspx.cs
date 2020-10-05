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
    public partial class FAP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["params"] = null;
            Session["cityState"] = null;
            Session["zip"] = null;
            Session["company"] = null;
            Session["reqinfocom"] = null;

            if (txtCity.Text != "") {
                Session["cityState"] = this.txtCity.Text;
            }

            if (txtCompanyName.Text != "")
            {
                Session["company"] = txtCompanyName.Text;
            }


            Session["miles"] = this.ddlMiles.SelectedValue;
            if (txtZipCode.Text != "")
            {
                Session["zip"] = this.txtZipCode.Text;
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
                Session["params"] = searchParams;
            }

            Response.Redirect("SearchFAP.aspx");

        }

      
    }
}