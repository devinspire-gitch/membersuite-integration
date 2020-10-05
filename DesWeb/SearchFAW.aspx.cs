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
    public partial class SearchFAW : System.Web.UI.Page
    {
        private void GetPartnersPageWise(int searchIndex,int pageIndex,int pageSize)
        {

            string zip = null;
            string cityState = null;
            string company = null;
            int miles = -1;
            List<string> searchParams = new List<string>();
            if (Session["paramshw"] != null)
            {
                searchParams = (List<string>)Session["paramshw"];
                
            }

            if (Session["mileshw"] != null)
            {
                miles = Convert.ToInt32(Session["mileshw"]);
            }

            List<string> searchCertifcs = new List<string>();
            if (Session["certshw"] != null)
            {
                searchCertifcs = (List<string>)Session["certshw"];
            }

            if (Session["ziphw"] != null)
            {
                zip = (string)Session["ziphw"];
                
            }
            if (Session["cityStatehw"] != null)
            {
                cityState = (string)Session["cityStatehw"];
            }

            if (Session["companyhw"] != null)
            {
                company = (string)Session["companyhw"];
            }


            ConciergeSampleBase demoToRun = new SearchUsingMSQL();
            var result = demoToRun.RunHD(zip, cityState, company, searchParams,searchCertifcs, miles, searchIndex, pageSize); // and run it
            if (result != null)
            {
                if (result.SearchResult.TotalRowCount > 0)
                {
                    DataTable resTable = result.SearchResult.Table;
                    resTable.Columns.Add("ImagePath");
                    foreach (DataRow dr in resTable.Rows)
                    {
                        string AssociationId = "82de7c93-0004-c334-b8e8-0b3b889fab82";
                        string PartitionId = "26429";
                        var imgAdd = dr[2];
                        if (dr[2].ToString() != "")
                            //dr["ImagePath"] = "https://images.membersuite.com/" + AssociationId + "/" + PartitionId + "/" + dr[2];
                            dr["ImagePath"] = "https://www.iwla.com/assets/1/23/" + dr["Image.Name"];
                        else
                            dr["ImagePath"] = "https://www.iwla.com/cms/images/notfound.png";


                    }
                   
                    // btnMap.Visible = true;
                    Session["partnerTablehw"] = resTable;
                    Repeater1.DataSource = resTable;
                    Repeater1.DataBind();
                    int recordCount = result.SearchResult.TotalRowCount;
                    this.PopulatePager(recordCount, pageIndex, pageSize);
                }
                else {
                    Session["partnerTablehw"] = null;
                }
               
            }
            else
            {
                Session["partnerTablehw"] = null;
            }


        }

        protected void cbReqInfo_Checked(object sender, EventArgs e)
        {
            RepeaterItem checkBox = (sender as CheckBox).Parent as RepeaterItem;
            HiddenField hfid = checkBox.FindControl("hfName") as HiddenField;
            HiddenField hfemail = checkBox.FindControl("hfEmail") as HiddenField;

            List<Company> reqInfoCompanies = new List<Company>();
            if (Session["reqinfocom"] != null)
            {
                reqInfoCompanies = (List<Company>)Session["reqinfocom"];
                bool exists = false;
                foreach (var comp in reqInfoCompanies)
                {
                    if (comp.Name == hfid.Value)
                        exists = true;
                }

                if (!exists)
                    reqInfoCompanies.Add(new Company(hfid.Value,hfemail.Value));

                Session["reqinfocom"] = reqInfoCompanies;
            }
            else {
                reqInfoCompanies.Add(new Company(hfid.Value, hfemail.Value));
                Session["reqinfocom"] = reqInfoCompanies;
            }
        }
        private void PopulatePager(int recordCount, int currentPage,int pageSize)
        {
            double dblPageCount = (double)((decimal)recordCount / (decimal)pageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                pages.Add(new ListItem("<<", "1", currentPage > 1));
                if (currentPage != 1)
                {
                    pages.Add(new ListItem("Previous", (currentPage - 1).ToString()));
                }
                if (pageCount < 4)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage < 4)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                    pages.Add(new ListItem("...", (currentPage).ToString(), false));
                }
                else if (currentPage > pageCount - 4)
                {
                    pages.Add(new ListItem("...", (currentPage).ToString(), false));
                    for (int i = currentPage - 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else
                {
                    pages.Add(new ListItem("...", (currentPage).ToString(), false));
                    for (int i = currentPage - 2; i <= currentPage + 2; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                    pages.Add(new ListItem("...", (currentPage).ToString(), false));
                }
                if (currentPage != pageCount)
                {
                    pages.Add(new ListItem("next", (currentPage + 1).ToString()));
                }
                pages.Add(new ListItem(">>", pageCount.ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }
        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            this.GetPartnersPageWise(((pageIndex-1)*10), pageIndex, 10);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GetPartnersPageWise(0,1,10);
                Session["reqinfocom"] = null;
            }

            if (!IsPostBack)
            {
              
            }
        }
        public string ConvertDataTabletoString()
        {
            if (Session["partnerTablehw"] != null)
            {
                DataTable dataTable = null;
                dataTable = Session["partnerTablehw"] as DataTable;
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Address");
                dt.Columns.Add("Lat");
                dt.Columns.Add("Lng");
                if (dataTable != null)
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var address = dr["_Preferred_Address_Line1"] + " ," + dr["_Preferred_Address_City"] + " ," + dr["_Preferred_Address_Country"];

                        var locationService = new GoogleLocationService("AIzaSyBxO7g5cR0QVhJMf8sMk5On0LMo5Aepyqo");
                        var point = locationService.GetLatLongFromAddress(address);

                        if (point != null)
                        {
                            var latitude = point.Latitude;
                            var longitude = point.Longitude;
                            DataRow _address = dt.NewRow();
                            _address["Name"] = dr["Name"];
                            _address["Phone"] = dr["_Preferred_PhoneNumber"];
                            _address["Address"] = address;
                            _address["Lat"] = latitude;
                            _address["Lng"] = longitude;
                            dt.Rows.Add(_address);
                        }
                    }

                foreach (DataRow dr in dt.Rows)
                {

                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);

                }


                return serializer.Serialize(rows);
            }
            return null;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("FAW.aspx");
        }

        protected void btnReqInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Mail.aspx");
        }
    }

    public class Company
    {
        private string name;
        private string email;
       

        public Company(string name, string email)
        {
            this.name = name;
            this.email = email;
           
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
 }