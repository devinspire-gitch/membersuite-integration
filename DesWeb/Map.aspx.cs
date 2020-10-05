using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

using GoogleMaps.LocationServices;
namespace DesWeb
{
    public partial class Map : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string ConvertDataTabletoString()
        {

            DataTable dataTable = null;
            dataTable=HttpContext.Current.Cache["partnerTable"] as DataTable;
            //if (dataTable == null)
            //{
            //    ConciergeSampleBase demoToRun = new SearchUsingMSQL();
            //    string  zip = null;
            //    string cityState = null;
            //    if (this.Request.QueryString["zip"] != null && this.Request.QueryString["zip"] != "")
            //    {
            //        zip = this.Request.QueryString["zip"];
            //    }

            //    if (this.Request.QueryString["search"] != null && this.Request.QueryString["zip"] != "")
            //    {
            //        cityState = this.Request.QueryString["search"];
            //    }
            //    var result = demoToRun.Run(zip, cityState); // and run it
            //    HttpContext.Current.Cache.Insert("partnerTable", result.SearchResult.Table, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
            //    dataTable = HttpContext.Current.Cache["partnerTable"] as DataTable;
            //}
            

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
            foreach (DataRow dr in dataTable.Rows)
            {
                var address = dr["_Preferred_Address_Line1"] + " ," + dr["_Preferred_Address_City"] + " ," + dr["_Preferred_Address_Country"];

                var locationService = new GoogleLocationService("AIzaSyBKZp9cuEthSeBVTg51R2VYdebIyPIQwv8");
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
      }
}