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
using System.Data.SqlClient;
using System.Data;
using GoogleMaps.LocationServices;
namespace DesWeb
{
    public partial class DetailsHD : System.Web.UI.Page
    {

        string _website = "";
        public string WebSite
        {
            get { return _website; }
            set {_website = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                if (Request.QueryString["company"] != null)
                {
                    string company = Request.QueryString["company"];
                    ConciergeSampleBase demoToRun = new SearchUsingMSQL();
                    var result = demoToRun.GetComDetailsHD(company); // and run it
                    if (result != null)
                    {
                        this.lblName.Text = result.SearchResult.Table.Rows[0]["Name"].ToString();
                        var localId= result.SearchResult.Table.Rows[0]["LocalID"].ToString();
                        this.imgCompany.ImageUrl = "https://www.iwla.com/assets/1/23/" + result.SearchResult.Table.Rows[0]["Image.Name"].ToString();
                        this.lblContactName.Text = result.SearchResult.Table.Rows[0]["PrimaryContactName__RightSide__rt"].ToString();
                        this.lblAddress1.Text = result.SearchResult.Table.Rows[0]["_Preferred_Address_Line1"].ToString();
                        this.lblAddress2.Text = result.SearchResult.Table.Rows[0]["_Preferred_Address_Line2"].ToString();
                        this.lblPhone.Text = result.SearchResult.Table.Rows[0]["_Preferred_PhoneNumber"].ToString();
                        this.lblCityStateZip.Text = result.SearchResult.Table.Rows[0]["_Preferred_Address_City"].ToString() + ", " + result.SearchResult.Table.Rows[0]["_Preferred_Address_State"].ToString() + " " + result.SearchResult.Table.Rows[0]["_Preferred_Address_PostalCode"].ToString();
                        this.lnkWebSite.NavigateUrl = result.SearchResult.Table.Rows[0]["Website"].ToString();
                        this.lnkWebSite.Text = result.SearchResult.Table.Rows[0]["Website"].ToString();
                      
                        ContentIframe.Attributes["src"] = result.SearchResult.Table.Rows[0]["Youtube__c"].ToString().Replace("https://www.youtube.com/watch?v=", "https://www.youtube.com/embed/");
                        this.ltNote.Text = result.SearchResult.Table.Rows[0]["CompanyInfo__c"].ToString();
                    }
                }
            }
        }
        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Server.Transfer("FAP.aspx");
        }
    }
}