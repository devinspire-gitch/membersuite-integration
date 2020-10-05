using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using MemberSuite.SDK.Concierge;
using MemberSuite.SDK.Results;
using MemberSuite.SDK.Searching;
using MemberSuite.SDK.Searching.Operations;
using MemberSuite.SDK.Types;

namespace DesWeb
{
    public class SearchUsingMSQL : ConciergeSampleBase
    {
        public override MSQLResult Run(string zip,string cityState, string company, List<string> keywords,int miles,int startRecord,int numberofRecords)
        {
            /* This sample is designed to demonstrate running a search in MemberSuite
             * using MSQL. We'll do a search for all people who have first OR last names that
             * begin with the letter A */

            // First, we need to prepare the proxy with the proper security settings.
            // This allows the proxy to generate the appropriate security header. For more information
            // on how to get these settings, see http://api.docs.membersuite.com in the Getting Started section

            //if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            //{
            //    ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
            //    ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            //    ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationID"];
            //}

            if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            {
                ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
                ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            }

            if (string.IsNullOrWhiteSpace(ConciergeAPIProxyGenerator.AssociationId) && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AssociationId"]))
            {
                ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationId"];
            }

            // ok, let's generate our API proxy
            using (var api = ConciergeAPIProxyGenerator.GenerateProxy())
            {
                 var resultZips = api.GetZipCodesWithinSpecifiedRadiusByZip(zip,miles);
                //string msql = "select Name, LocalID, PrimaryContactName__RightSide__rt.Name,WebSite,EmailAddress,Zip,_Preferred_PhoneNumber, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City,_Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, FAW_alchohol__c, FAW_apparel__c, FAW_Automotive__c, FAW_Building__c, FAW_Chemical__c, FAW_CPG__c, Description__c, FAW_Electronics__c, FAW_DryFood__c, FAW_TempControl__c, FAW_FulfilB2B__c, FAW_FulfilB2C__c, FAW_GenWare__c, FAW_ImExport__c, FAW_Industrial__c, FAW_Intermodal__c, FAW_International__c, FAW_intime__c, FAW_Kitting__c, FAW_Labeling__c, FAW_light__c, FAW_polymers__c, FAW_PortServices__c, FAW_ReverseLogistics__c, FAW_Rail__c, FAW_Shipping__c, FAW_TempControlNonFood__c, FAW_Tobacco__c, TransportationServices__c, FAW_Trucking__c, FAW_Value__c, FAW_Whiteglove__c from Organization where (Membership.ReceivesMemberBenefits=1) ";

                string msql = "select Name,Image.Name, LocalID, PrimaryContactName__RightSide__rt, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City, _Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, PrimaryContactName__RightSide__rt.Name, EmailAddress,Website, _Preferred_PhoneNumber, FAP_Barcode__c, FAP_Building__c, FAP_Business__c, FAP_Communication__c, FAP_Construction__c, FAP_Conveyor__c, FAP_Electronic__c, FAP_Energy__c, FAP_Forklifts__c, FAP_Human__c, FAP_Information__c, FAP_Insurance__c, FAP_IT__c, FAP_Labor__c, FAP_Legal__c, FAP_Lighting__c, FAP_Marketing__c, FAP_Material__c, FAP_Office__c, FAP_Packaging__c, FAP_Pallets__c, FAP_Payroll__c, FAP_Plant__c, FAP_Propane__c, FAP_Racking__c, FAP_Radio__c, FAP_Real__c, FAP_Renewables__c, FAP_Robotics__c, FAP_Safety__c, FAP_Sanitation__c, FAP_Security__c, FAP_Solar__c, FAP_Staffing__c, FAP_Transporta__c, FAP_Transportation__c, FAP_Utilities__c, FAP_Warehouse__c from Organization where (Membership.ReceivesMemberBenefits=1 AND Membership.Type IN ('82de7c93-006a-c48f-241b-0b3b96a280ab','82de7c93-006a-c0fe-045c-0b3b96a2dd59')) ";
                string whereConcatenator = " AND ";
                string endBracket = " )";

                if (company != null)
                {
                    msql += whereConcatenator;
                    msql += "Name like '" + company + "%' ";
                }

                if (cityState != null)
                {
                    whereConcatenator = " AND ( ";
                    msql += whereConcatenator;
                    msql += "_Preferred_Address_City like '" + cityState + "%' ";
                    whereConcatenator = " OR ";
                    msql += whereConcatenator;
                    msql += "_Preferred_Address_State like '" + cityState + "%' ";
                    msql += endBracket;
                    whereConcatenator = " AND ";
                }

                bool needsBrancket = false;
                
                whereConcatenator = " AND ( ";
                if (zip != null)
                {
                    if (resultZips.ResultValue!=null)
                    foreach (ZipCodeRadius keyw in resultZips.ResultValue)
                    {
                            msql += whereConcatenator;
                            msql += "_Preferred_Address_PostalCode like '" + keyw.ZipCode + "%' ";
                            whereConcatenator = " OR "; 
                            needsBrancket = true;
                      
                    }
                   
                }

                if (needsBrancket)
                    msql += endBracket;


                #region "params"
                needsBrancket = false;
                endBracket = " )";
                whereConcatenator = " AND ( ";
              
                foreach (string keyw in keywords)
                {
                    if (keyw.Trim() == "Barcode Labeling & Thermal Printing")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Barcode__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }

                    if (keyw.Trim() == "Labor Management Software")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Labor__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }

                    if (keyw.Trim() == "Renewables & Recyclables")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Renewables__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Renewables & Recyclables")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Building__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Renewables & Recyclables")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Legal__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "RFID & Wireless Infrastructure")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Legal__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }

                    if (keyw.Trim() == "Business Services & Consulting")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Business__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Lighting")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Lighting__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Robotics")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Robotics__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Communication Services")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Communication__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Marketing & Website")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Marketing__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Safety Products")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Safety__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Construction Services")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Construction__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Material Handling Equipment")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Material__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Sanitation & Pest Management")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Sanitation__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Conveyor Systems")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Conveyor__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Office Materials")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Office__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Security")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Security__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Electronic Data Interchange & Systems Integration")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Electronic__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Packaging")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Packaging__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Software")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Packaging__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Energy Performance Management")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Energy__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Pallets")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Pallets__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Solar")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Solar__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Facility Maintenance")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Solar__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Payroll & Accounting")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Payroll__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Staffing Provider")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Staffing__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Forklifts & Accessories")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Forklifts__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Plant & Facility Equipment")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Plant__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Transportation")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Transportation__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Human Resources")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Transportation__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Propane")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Propane__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Transportation Management Software")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Transportation__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Information Technology & Hardware")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_IT__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Racking")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Racking__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Utilities")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Utilities__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Insurance")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Insurance__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Real Estate")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Real__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "Warehouse Management Software")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_Warehouse__c =True";
                         whereConcatenator = " OR ";needsBrancket = true;
                    }
                    if (keyw.Trim() == "IT Service & Repair")
                    {
                        msql += whereConcatenator;
                        msql += "FAP_IT__c =True";
                        needsBrancket = true;
                    }
                }

                if(needsBrancket)
                msql += endBracket;
                #endregion
                msql += " order by SortName, Name";

                
                var result = api.ExecuteMSQL(msql, startRecord, numberofRecords);

               

                if (!result.Success)
                {
                    Console.WriteLine("Search failed: {0}", result.FirstErrorMessage);
                    return null;
                }

                return result.ResultValue;
                Console.WriteLine("Search successful: {0} results returned.",
                    result.ResultValue.SearchResult.TotalRowCount);


                foreach (DataRow row in result.ResultValue.SearchResult.Table.Rows)
                    Console.WriteLine("#{0} - {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}",
                        row["Name"], row["LocalID"], row["PrimaryContactName__RightSide__rt.Name"], row["_Preferred_Address_Line1"], row["_Preferred_Address_Line2"], row["_Preferred_Address_City"], row["_Preferred_Address_Country"], row["_Preferred_Address_PostalCode"], row["FAW_alchohol__c"], row["FAW_apparel__c"], row["FAW_Automotive__c"], row["FAW_Building__c"], row["FAW_Chemical__c"], row["FAW_CPG__c"], row["Description__c"], row["FAW_Electronics__c"], row["FAW_DryFood__c"], row["FAW_TempControl__c"], row["FAW_FulfilB2B__c"], row["FAW_FulfilB2C__c"], row["FAW_GenWare__c"], row["FAW_ImExport__c"], row["FAW_Industrial__c"], row["FAW_Intermodal__c"], row["FAW_International__c"], row["FAW_intime__c"], row["FAW_Kitting__c"], row["FAW_Labeling__c"], row["FAW_light__c"], row["FAW_polymers__c"], row["FAW_PortServices__c"], row["FAW_ReverseLogistics__c"], row["FAW_Rail__c"], row["FAW_Shipping__c"], row["FAW_TempControlNonFood__c"], row["FAW_Tobacco__c"], row["TransportationServices__c"], row["FAW_Trucking__c"], row["FAW_Value__c"], row["FAW_Whiteglove__c"]);
            }
        }

        public override MSQLResult RunHD(string zip, string cityState, string company, List<string> keywords, List<string> certifics, int miles, int startRecord, int numberofRecords)
        {
            /* This sample is designed to demonstrate running a search in MemberSuite
             * using MSQL. We'll do a search for all people who have first OR last names that
             * begin with the letter A */

            // First, we need to prepare the proxy with the proper security settings.
            // This allows the proxy to generate the appropriate security header. For more information
            // on how to get these settings, see http://api.docs.membersuite.com in the Getting Started section

            //if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            //{
            //    ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
            //    ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            //    ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationID"];
            //}

            if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            {
                ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
                ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            }

            if (string.IsNullOrWhiteSpace(ConciergeAPIProxyGenerator.AssociationId) && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AssociationId"]))
            {
                ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationId"];
            }

            // ok, let's generate our API proxy
            using (var api = ConciergeAPIProxyGenerator.GenerateProxy())
            {
                var resultZips = api.GetZipCodesWithinSpecifiedRadiusByZip(zip, miles);
                //string msql = "select Name, LocalID, PrimaryContactName__RightSide__rt.Name,WebSite,EmailAddress,Zip,_Preferred_PhoneNumber, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City,_Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, FAW_alchohol__c, FAW_apparel__c, FAW_Automotive__c, FAW_Building__c, FAW_Chemical__c, FAW_CPG__c, Description__c, FAW_Electronics__c, FAW_DryFood__c, FAW_TempControl__c, FAW_FulfilB2B__c, FAW_FulfilB2C__c, FAW_GenWare__c, FAW_ImExport__c, FAW_Industrial__c, FAW_Intermodal__c, FAW_International__c, FAW_intime__c, FAW_Kitting__c, FAW_Labeling__c, FAW_light__c, FAW_polymers__c, FAW_PortServices__c, FAW_ReverseLogistics__c, FAW_Rail__c, FAW_Shipping__c, FAW_TempControlNonFood__c, FAW_Tobacco__c, TransportationServices__c, FAW_Trucking__c, FAW_Value__c, FAW_Whiteglove__c from Organization where (Membership.ReceivesMemberBenefits=1) ";

                string msql = "select Name,Image.Name, LocalID,  WebSite, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City, _Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, _Preferred_PhoneNumber, PrimaryContactName__RightSide__rt,EmailAddress, FAW_alchohol__c, FAW_apparel__c, FAW_Automotive__c, FAW_Building__c, FAW_Chemical__c, FAW_CPG__c, FAW_Cross__c, FAW_distribution__c, FAW_Electronics__c, FAW_DryFood__c, FAW_TempControl__c, FAW_FulfilB2B__c, FAW_FulfilB2C__c, FAW_GenWare__c, FAW_ImExport__c, FAW_Industrial__c, FAW_Intermodal__c, FAW_International__c, FAW_intime__c, FAW_Kitting__c, FAW_Labeling__c, FAW_light__c, FAW_polymers__c, FAW_PortServices__c, FAW_ReverseLogistics__c, FAW_Rail__c, FAW_Shipping__c, FAW_TempControlNonFood__c, FAW_Tobacco__c, TransportationServices__c, FAW_Trucking__c, FAW_Value__c, FAW_Whiteglove__c, CERTS_AIB__c, CERTS_AlcoholLis__c, CERTS_Bonded__c, CERTS_BRC__c, CERTS_CPAT__c, CERTS_FDA__c, CERTS_FTZ__c, CERTS_HACCP__c, CERTS_Hazmat__c, CERTS_ISO14001__c, CERTS_ISO9001__c, CERTS_LEED__c, CERTS_NACD__c, CERTS_Organic__c from Organization where (Membership.ReceivesMemberBenefits=1 AND Membership.Type IN ('82de7c93-006a-c89c-7677-0b3b96a2cf3a','82de7c93-006a-c30c-ade3-0b3b96a322b4','82de7c93-006a-cdca-b344-0b3c1d9c83fa')) ";
                string whereConcatenator = " AND ";
                string endBracket = " )";

                if (company != null)
                {
                    msql += whereConcatenator;
                    msql += "Name like '" + company + "%' ";
                }

                if (cityState != null)
                {
                    whereConcatenator = " AND ( ";
                    msql += whereConcatenator;
                    msql += "_Preferred_Address_City like '" + cityState + "%' ";
                    whereConcatenator = " OR ";
                    msql += whereConcatenator;
                    msql += "_Preferred_Address_State like '" + cityState + "%' ";
                    msql += endBracket;
                    whereConcatenator = " AND ";
                }

                bool needsBrancket = false;
                
                whereConcatenator = " AND ( ";
                if (zip != null)
                {
                    if (resultZips.ResultValue != null)
                        foreach (ZipCodeRadius keyw in resultZips.ResultValue)
                        {
                            msql += whereConcatenator;
                            msql += "_Preferred_Address_PostalCode like '" + keyw.ZipCode + "%' ";
                            whereConcatenator = " OR ";
                            needsBrancket = true;

                        }

                }

                if (needsBrancket)
                    msql += endBracket;


                #region "params"
                needsBrancket = false;
                endBracket = " )";
                whereConcatenator = " AND ( ";

                foreach (string keyw in keywords)
                {
                    if (keyw.Trim() == "Alcohol Only")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_alchohol__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }

                    //if (keyw.Trim() == "Foreign Trade Zone")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Labor__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}

                    if (keyw.Trim() == "Polymers")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_polymers__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //if (keyw.Trim() == "Alcohol/Tobacco")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAW_Tobacco__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    if (keyw.Trim() == "Port services")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_PortServices__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Apparel")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_apparel__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }

                    if (keyw.Trim() == "Rail")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Rail__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Automotive")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Automotive__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Import/export")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_ImExport__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "General Warehousing (public warehousing)")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_GenWare__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Reverse logistics (returns/repairs)")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_ReverseLogistics__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Industrial goods")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Industrial__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Building materials")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Building__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Intermodal/rail/box car")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Intermodal__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Shipping/port accessible")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Shipping__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Chemical/hazmat")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Chemical__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "International")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_International__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Temperature-controlled warehouse - nonfood")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_TempControlNonFood__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //if (keyw.Trim() == "Cold storage/frozen produce")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Electronic__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    if (keyw.Trim() == "Just-in-time")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_intime__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Tobacco Only")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Tobacco__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "CPG/Personal care products")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_CPG__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Kitting/pick and packing")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Kitting__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Transportation")
                    {
                        msql += whereConcatenator;
                        msql += "TransportationServices__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Cross-dock/transload")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Cross__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Trucking/drayage")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Trucking__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Distribution")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_distribution__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Light manufacturing")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_light__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Value-add Services")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Value__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Dry food/temperature-controlled food")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_DryFood__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //
                    //if (keyw.Trim() == "Minority owned")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Transportation__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    //if (keyw.Trim() == "Veteran owned")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Propane__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    //if (keyw.Trim() == "E-commerce fulfillment")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Transportation__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    //
                    //if (keyw.Trim() == "Pharmaceuticals")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_IT__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    if (keyw.Trim() == "White-glove delivery")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Whiteglove__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Electronics/appliances")
                    {
                        msql += whereConcatenator;
                        msql += "FAW_Electronics__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //if (keyw.Trim() == "Insurance")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Insurance__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    //if (keyw.Trim() == "Real Estate")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Real__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    //if (keyw.Trim() == "Warehouse Management Software")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_Warehouse__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    //if (keyw.Trim() == "IT Service & Repair")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAP_IT__c =True";

                    //}
                }

                if (needsBrancket)
                    msql += endBracket;
                #endregion

                #region "certifications"
                needsBrancket = false;
                endBracket = " )";
                whereConcatenator = " AND ( ";

                foreach (string keyw in certifics)
                {
                    if (keyw.Trim() == "AIB Certified Facilities")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_AIB__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }


                    if (keyw.Trim() == "FDA Registered Facilities")
                    {
                        msql += whereConcatenator;
                        msql += " CERTS_FDA__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
          
                    if (keyw.Trim() == "ISO9001")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_ISO9001__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //
                    if (keyw.Trim() == "Alcohol (licensed)")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_AlcoholLis__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }

                    if (keyw.Trim() == "Foreign Trade Zone")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_FTZ__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    //if (keyw.Trim() == "IWLA Responsible Warehouse Protocol Certified Facilities")
                    //{
                    //    msql += whereConcatenator;
                    //    msql += "FAW_Automotive__c =True";
                    //    whereConcatenator = " OR "; needsBrancket = true;
                    //}
                    if (keyw.Trim() == "Bonded warehouse")
                    {
                        msql += whereConcatenator;
                        msql += " CERTS_Bonded__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "HACCP standards adherence")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_HACCP__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "LEED Certified Facilities")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_LEED__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "BRC")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_BRC__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Hazmat certified facilities")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_Hazmat__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "NACD Responsible Distribution")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_NACD__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "C-TPAT")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_CPAT__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "ISO14001")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_ISO14001__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    if (keyw.Trim() == "Organic Certified Facilities")
                    {
                        msql += whereConcatenator;
                        msql += "CERTS_Organic__c =True";
                        whereConcatenator = " OR "; needsBrancket = true;
                    }
                    
                }

                if (needsBrancket)
                    msql += endBracket;
                #endregion
                msql += " order by SortName, Name";


                var result = api.ExecuteMSQL(msql, startRecord, numberofRecords);



                if (!result.Success)
                {
                    Console.WriteLine("Search failed: {0}", result.FirstErrorMessage);
                    return null;
                }

                return result.ResultValue;
                Console.WriteLine("Search successful: {0} results returned.",
                    result.ResultValue.SearchResult.TotalRowCount);


                foreach (DataRow row in result.ResultValue.SearchResult.Table.Rows)
                    Console.WriteLine("#{0} - {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}",
                        row["Name"], row["LocalID"], row["PrimaryContactName__RightSide__rt.Name"], row["_Preferred_Address_Line1"], row["_Preferred_Address_Line2"], row["_Preferred_Address_City"], row["_Preferred_Address_Country"], row["_Preferred_Address_PostalCode"], row["FAW_alchohol__c"], row["FAW_apparel__c"], row["FAW_Automotive__c"], row["FAW_Building__c"], row["FAW_Chemical__c"], row["FAW_CPG__c"], row["Description__c"], row["FAW_Electronics__c"], row["FAW_DryFood__c"], row["FAW_TempControl__c"], row["FAW_FulfilB2B__c"], row["FAW_FulfilB2C__c"], row["FAW_GenWare__c"], row["FAW_ImExport__c"], row["FAW_Industrial__c"], row["FAW_Intermodal__c"], row["FAW_International__c"], row["FAW_intime__c"], row["FAW_Kitting__c"], row["FAW_Labeling__c"], row["FAW_light__c"], row["FAW_polymers__c"], row["FAW_PortServices__c"], row["FAW_ReverseLogistics__c"], row["FAW_Rail__c"], row["FAW_Shipping__c"], row["FAW_TempControlNonFood__c"], row["FAW_Tobacco__c"], row["TransportationServices__c"], row["FAW_Trucking__c"], row["FAW_Value__c"], row["FAW_Whiteglove__c"]);
            }
        }


        public override MSQLResult GetComDetails(string company)
        {
            /* This sample is designed to demonstrate running a search in MemberSuite
             * using MSQL. We'll do a search for all people who have first OR last names that
             * begin with the letter A */

            // First, we need to prepare the proxy with the proper security settings.
            // This allows the proxy to generate the appropriate security header. For more information
            // on how to get these settings, see http://api.docs.membersuite.com in the Getting Started section

            //if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            //{
            //    ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
            //    ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            //    ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationID"];
            //}

            if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            {
                ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
                ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            }

            if (string.IsNullOrWhiteSpace(ConciergeAPIProxyGenerator.AssociationId) && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AssociationId"]))
            {
                ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationId"];
            }

            // ok, let's generate our API proxy
            using (var api = ConciergeAPIProxyGenerator.GenerateProxy())
            {
                
                //string msql = "select Name, LocalID, PrimaryContactName__RightSide__rt.Name,WebSite,EmailAddress,Zip,_Preferred_PhoneNumber, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City,_Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, FAW_alchohol__c, FAW_apparel__c, FAW_Automotive__c, FAW_Building__c, FAW_Chemical__c, FAW_CPG__c, Description__c, FAW_Electronics__c, FAW_DryFood__c, FAW_TempControl__c, FAW_FulfilB2B__c, FAW_FulfilB2C__c, FAW_GenWare__c, FAW_ImExport__c, FAW_Industrial__c, FAW_Intermodal__c, FAW_International__c, FAW_intime__c, FAW_Kitting__c, FAW_Labeling__c, FAW_light__c, FAW_polymers__c, FAW_PortServices__c, FAW_ReverseLogistics__c, FAW_Rail__c, FAW_Shipping__c, FAW_TempControlNonFood__c, FAW_Tobacco__c, TransportationServices__c, FAW_Trucking__c, FAW_Value__c, FAW_Whiteglove__c from Organization where (Membership.ReceivesMemberBenefits=1) ";

                string msql = "select Name,Image.Name,CompanyInfo__c,Youtube__c, LocalID, PrimaryContactName__RightSide__rt, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City, _Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, PrimaryContactName__RightSide__rt.Name, EmailAddress,Website, _Preferred_PhoneNumber, FAP_Barcode__c, FAP_Building__c, FAP_Business__c, FAP_Communication__c, FAP_Construction__c, FAP_Conveyor__c, FAP_Electronic__c, FAP_Energy__c, FAP_Forklifts__c, FAP_Human__c, FAP_Information__c, FAP_Insurance__c, FAP_IT__c, FAP_Labor__c, FAP_Legal__c, FAP_Lighting__c, FAP_Marketing__c, FAP_Material__c, FAP_Office__c, FAP_Packaging__c, FAP_Pallets__c, FAP_Payroll__c, FAP_Plant__c, FAP_Propane__c, FAP_Racking__c, FAP_Radio__c, FAP_Real__c, FAP_Renewables__c, FAP_Robotics__c, FAP_Safety__c, FAP_Sanitation__c, FAP_Security__c, FAP_Solar__c, FAP_Staffing__c, FAP_Transporta__c, FAP_Transportation__c, FAP_Utilities__c, FAP_Warehouse__c from Organization where (Membership.ReceivesMemberBenefits=1 AND Membership.Type IN ('82de7c93-006a-c48f-241b-0b3b96a280ab','82de7c93-006a-c0fe-045c-0b3b96a2dd59')) ";
                string whereConcatenator = " AND ";

                if (company != null)
                {
                    msql += whereConcatenator;
                    msql += "Name like '" + company + "%' ";
                }

               


                var result = api.ExecuteMSQL(msql, 0, 1);



                if (!result.Success)
                {
                    Console.WriteLine("Search failed: {0}", result.FirstErrorMessage);
                    return null;
                }

                return result.ResultValue;
                Console.WriteLine("Search successful: {0} results returned.",
                    result.ResultValue.SearchResult.TotalRowCount);


                foreach (DataRow row in result.ResultValue.SearchResult.Table.Rows)
                    Console.WriteLine("#{0} - {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}",
                        row["Name"], row["LocalID"], row["PrimaryContactName__RightSide__rt.Name"], row["_Preferred_Address_Line1"], row["_Preferred_Address_Line2"], row["_Preferred_Address_City"], row["_Preferred_Address_Country"], row["_Preferred_Address_PostalCode"], row["FAW_alchohol__c"], row["FAW_apparel__c"], row["FAW_Automotive__c"], row["FAW_Building__c"], row["FAW_Chemical__c"], row["FAW_CPG__c"], row["Description__c"], row["FAW_Electronics__c"], row["FAW_DryFood__c"], row["FAW_TempControl__c"], row["FAW_FulfilB2B__c"], row["FAW_FulfilB2C__c"], row["FAW_GenWare__c"], row["FAW_ImExport__c"], row["FAW_Industrial__c"], row["FAW_Intermodal__c"], row["FAW_International__c"], row["FAW_intime__c"], row["FAW_Kitting__c"], row["FAW_Labeling__c"], row["FAW_light__c"], row["FAW_polymers__c"], row["FAW_PortServices__c"], row["FAW_ReverseLogistics__c"], row["FAW_Rail__c"], row["FAW_Shipping__c"], row["FAW_TempControlNonFood__c"], row["FAW_Tobacco__c"], row["TransportationServices__c"], row["FAW_Trucking__c"], row["FAW_Value__c"], row["FAW_Whiteglove__c"]);
            }
        }

        public override MSQLResult GetComDetailsHD(string company)
        {
            /* This sample is designed to demonstrate running a search in MemberSuite
             * using MSQL. We'll do a search for all people who have first OR last names that
             * begin with the letter A */

            // First, we need to prepare the proxy with the proper security settings.
            // This allows the proxy to generate the appropriate security header. For more information
            // on how to get these settings, see http://api.docs.membersuite.com in the Getting Started section

            //if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            //{
            //    ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
            //    ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            //    ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationID"];
            //}

            if (!ConciergeAPIProxyGenerator.IsSecretAccessKeySet)
            {
                ConciergeAPIProxyGenerator.SetAccessKeyId(ConfigurationManager.AppSettings["AccessKeyID"]);
                ConciergeAPIProxyGenerator.SetSecretAccessKey(ConfigurationManager.AppSettings["SecretAccessKey"]);
            }

            if (string.IsNullOrWhiteSpace(ConciergeAPIProxyGenerator.AssociationId) && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AssociationId"]))
            {
                ConciergeAPIProxyGenerator.AssociationId = ConfigurationManager.AppSettings["AssociationId"];
            }

            // ok, let's generate our API proxy
            using (var api = ConciergeAPIProxyGenerator.GenerateProxy())
            {

                //string msql = "select Name, LocalID, PrimaryContactName__RightSide__rt.Name,WebSite,EmailAddress,Zip,_Preferred_PhoneNumber, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City,_Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, FAW_alchohol__c, FAW_apparel__c, FAW_Automotive__c, FAW_Building__c, FAW_Chemical__c, FAW_CPG__c, Description__c, FAW_Electronics__c, FAW_DryFood__c, FAW_TempControl__c, FAW_FulfilB2B__c, FAW_FulfilB2C__c, FAW_GenWare__c, FAW_ImExport__c, FAW_Industrial__c, FAW_Intermodal__c, FAW_International__c, FAW_intime__c, FAW_Kitting__c, FAW_Labeling__c, FAW_light__c, FAW_polymers__c, FAW_PortServices__c, FAW_ReverseLogistics__c, FAW_Rail__c, FAW_Shipping__c, FAW_TempControlNonFood__c, FAW_Tobacco__c, TransportationServices__c, FAW_Trucking__c, FAW_Value__c, FAW_Whiteglove__c from Organization where (Membership.ReceivesMemberBenefits=1) ";

                string msql = "select Name,LocalID,Image.Name,CompanyInfo__c,Youtube__c, LocalID,WebSite, _Preferred_Address_Line1, _Preferred_Address_Line2, _Preferred_Address_City, _Preferred_Address_State, _Preferred_Address_Country, _Preferred_Address_PostalCode, _Preferred_PhoneNumber, PrimaryContactName__RightSide__rt, PrimaryContactName__RightSide__rt.EmailAddress, FAW_alchohol__c, FAW_apparel__c, FAW_Automotive__c, FAW_Building__c, FAW_Chemical__c, FAW_CPG__c, FAW_Cross__c, FAW_distribution__c, FAW_Electronics__c, FAW_DryFood__c, FAW_TempControl__c, FAW_FulfilB2B__c, FAW_FulfilB2C__c, FAW_GenWare__c, FAW_ImExport__c, FAW_Industrial__c, FAW_Intermodal__c, FAW_International__c, FAW_intime__c, FAW_Kitting__c, FAW_Labeling__c, FAW_light__c, FAW_polymers__c, FAW_PortServices__c, FAW_ReverseLogistics__c, FAW_Rail__c, FAW_Shipping__c, FAW_TempControlNonFood__c, FAW_Tobacco__c, TransportationServices__c, FAW_Trucking__c, FAW_Value__c, FAW_Whiteglove__c from Organization where (Membership.ReceivesMemberBenefits=1 AND Membership.Type IN ('82de7c93-006a-c89c-7677-0b3b96a2cf3a','82de7c93-006a-c30c-ade3-0b3b96a322b4','82de7c93-006a-cdca-b344-0b3c1d9c83fa')) ";
                string whereConcatenator = " AND ";

                if (company != null)
                {
                    msql += whereConcatenator;
                    msql += "Name like '" + company + "%' ";
                }




                var result = api.ExecuteMSQL(msql, 0, 1);



                if (!result.Success)
                {
                    Console.WriteLine("Search failed: {0}", result.FirstErrorMessage);
                    return null;
                }

                return result.ResultValue;
                Console.WriteLine("Search successful: {0} results returned.",
                    result.ResultValue.SearchResult.TotalRowCount);


                foreach (DataRow row in result.ResultValue.SearchResult.Table.Rows)
                    Console.WriteLine("#{0} - {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}",
                        row["Name"], row["LocalID"], row["PrimaryContactName__RightSide__rt.Name"], row["_Preferred_Address_Line1"], row["_Preferred_Address_Line2"], row["_Preferred_Address_City"], row["_Preferred_Address_Country"], row["_Preferred_Address_PostalCode"], row["FAW_alchohol__c"], row["FAW_apparel__c"], row["FAW_Automotive__c"], row["FAW_Building__c"], row["FAW_Chemical__c"], row["FAW_CPG__c"], row["Description__c"], row["FAW_Electronics__c"], row["FAW_DryFood__c"], row["FAW_TempControl__c"], row["FAW_FulfilB2B__c"], row["FAW_FulfilB2C__c"], row["FAW_GenWare__c"], row["FAW_ImExport__c"], row["FAW_Industrial__c"], row["FAW_Intermodal__c"], row["FAW_International__c"], row["FAW_intime__c"], row["FAW_Kitting__c"], row["FAW_Labeling__c"], row["FAW_light__c"], row["FAW_polymers__c"], row["FAW_PortServices__c"], row["FAW_ReverseLogistics__c"], row["FAW_Rail__c"], row["FAW_Shipping__c"], row["FAW_TempControlNonFood__c"], row["FAW_Tobacco__c"], row["TransportationServices__c"], row["FAW_Trucking__c"], row["FAW_Value__c"], row["FAW_Whiteglove__c"]);
            }
        }

    }
}
