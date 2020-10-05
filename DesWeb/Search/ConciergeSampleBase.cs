using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemberSuite.SDK.Results;

namespace DesWeb
{
    /// <summary>
    /// This is the base helper class for all API samples
    /// </summary>
    public abstract class ConciergeSampleBase
    {
        public abstract MSQLResult Run(string zip, string cityState, string company, List<string> keywords,int miles, int startRecord, int numberofRecords);
        public abstract MSQLResult RunHD(string zip, string cityState, string company, List<string> keywords, List<string> certifics, int miles, int startRecord, int numberofRecords);

        public abstract MSQLResult GetComDetails(string company);
        public abstract MSQLResult GetComDetailsHD(string company);
    }
}
