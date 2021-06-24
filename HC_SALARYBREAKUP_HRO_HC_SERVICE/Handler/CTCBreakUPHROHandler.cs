using HC_SALARYBREAKUP_HRO_HC_SERVICE.Models;
using HireCraft.HM_APIService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HC_SALARYBREAKUP_HRO_HC_SERVICE.Handler
{
    public class CTCBreakUPHROHandler
    {


        public void Process()
        {
            DataSet ds= GetData();//fitment approved
            if (ds.Tables[0].Rows.Count > 0)
            {


                Token Token = GetToken();
                PrepareJSON(ds, Token.access_token);
            }
            else
            {
                Log.LogData("No Data", Log.Status.Info);
            }

        }

        private Token GetToken()
        {
            Token opTokenClass = new Token();

            try
            {
                Log.LogData("Fetching Token ", Log.Status.Info);
                string link = Helper.AuthorizationURL;
                var parameters = "client_id=" + Helper.client_id + "&client_secret=" + Helper.client_secret + "&grant_type=" + Helper.grant_type;
                //Log.LogData("url calling", Log.Status.Fatal);
                //Log.LogData("Destappkey" + Helper.destAppKey.ToString() + " reskey " + Helper.OpexResKey.ToString(), Log.Status.Info);
                //string parameters = "destAppKey=aksnfkpvcjq30u88tlo4&resKey=lnt1099mc7dppuvt7eim";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (WebClient client = new WebClient())
                {
                    client.Headers["Authorization"] = Helper.Authorization;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.UploadString(link, parameters);
                    Log.LogData("resonse came:" + response, Log.Status.Fatal);
                    opTokenClass = JsonConvert.DeserializeObject<Token>(response);

                }
                Log.LogData("Fetching Token completed ", Log.Status.Info);

            }
            catch (Exception ex)
            {
                Log.LogData("Error at GetToken " + ex.ToString(), Log.Status.Error);
            }
            return opTokenClass;
        }

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                
                using (SqlConnection conn = new SqlConnection(Helper.ConString))
                {
                    conn.Open();
                    using (SqlCommand oCmd = conn.CreateCommand())
                    {
                        oCmd.CommandText = "GetsalaryCTCFromHCtoHRO";
                        oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataAdapter oSql = new SqlDataAdapter(oCmd);
                        oSql.Fill(ds);
                    }
                }



            }
            catch (Exception ex)
            {
                Log.LogData("Error at Process " + ex.ToString(), Log.Status.Error);
            }
            return ds;

        }

        public void PrepareJSON(DataSet ds,string Token)
        {
            try
            {
                string OfferCTC = "";
                Int64 RID = 0;
                string JsonString = "";

                try
                {
                    Log.LogData("Preparing Json ", Log.Status.Info);
                    
                    Dictionary<string, object> row;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            if (col.ColumnName == "rid")
                            {
                                RID = Convert.ToInt64(dr[col]);
                            }
                            else
                            {
                                if(col.ColumnName == "TotalCTC")
                                {
                                    OfferCTC = Convert.ToString(dr[col]);

                                }


                                row.Add(col.ColumnName, dr[col]);
                            }
                        }
                        rows.Add(row);
                        JsonString = JsonConvert.SerializeObject(rows);
                        JsonString = JsonString.Replace("[", "");
                        JsonString = JsonString.Replace("]", "");
                        Log.LogData("Json: " + JsonString, Log.Status.Info);
                        RequestURL(JsonString, RID, Token, OfferCTC);
                    }



                }
                catch (Exception ex)
                {
                    Log.LogData("Error at SendRequest " + ex.ToString(), Log.Status.Error);
                }

            }
            catch(Exception ex)
            {
                Log.LogData("Error at RequestForCTCDetails " + ex.ToString(), Log.Status.Error);
            }
        }

        public void RequestURL(string JsonString, Int64 rid, string Token,string OfferCTC)
        {
            try
            {

                Log.LogData("Pushing Data ", Log.Status.Info);
                responseDetails opresponseDetails = new responseDetails();
                string result = "";
                string link = Helper.CTCSalaryURL;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(link);
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers["client_key"] = Helper.client_key;
                httpWebRequest.Headers["client_code"] = Helper.client_code;
                httpWebRequest.Headers["access_identifier"] = Helper.access_identifier;
                httpWebRequest.Headers["Authorization"] = "Bearer " + Token;

                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonString);
                    streamWriter.Flush();
                    streamWriter.Close();

                }
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                Log.LogData("Pushing Data Completed ", Log.Status.Info);
                UpdateCTCDATA(result, rid,OfferCTC);
            }
            catch (Exception ex)
            {
                Log.LogData("Error at RequestURL " + ex.ToString(), Log.Status.Error);
            }

            //opresponseDetails = JsonConvert.DeserializeObject<responseDetails>(result);

        }

        public void UpdateCTCDATA(string Response, Int64 rid,string OfferCTC)
        {
            Log.LogData("ResponseJSON " + Response, Log.Status.Info);
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("Name");
                dt.Columns.Add("Value");
                dt.Columns.Add("Annual");
                dt.Columns.Add("HeadGroup");

                ResponseDetails opResponseDetails = JsonConvert.DeserializeObject<ResponseDetails>(Response);
                
                
                List<SalaryHeadGroup> opSalaryHeadParent = new List<SalaryHeadGroup>();
                opSalaryHeadParent = opResponseDetails.SalaryHeadGroups;
                foreach(var data in opSalaryHeadParent)
                {


                    List<SalaryHead> opSalaryHead = new List<SalaryHead>();
                    opSalaryHead = data.SalaryHeads;
                    for (int i = 0; i < opSalaryHead.Count; i++)
                    {


                        DataRow dr = dt.NewRow();
                        dr["Name"] = opSalaryHead[i].HeadName;
                        dr["Value"] = opSalaryHead[i].HeadAmountM;
                        dr["Annual"] = opSalaryHead[i].HeadAmountA;
                        dr["HeadGroup"] = data.HeadGroupName;
                        dt.Rows.Add(dr);
                    }
                }



                


                Log.LogData("updation of flag and Response of RID: " + rid.ToString(), Log.Status.Info);
                using (SqlConnection conn = new SqlConnection(Helper.ConString))
                {
                    conn.Open();
                    using (SqlCommand oCmd = conn.CreateCommand())
                    {
                        oCmd.CommandText = "usp_updateHROCTCAmounts";
                        oCmd.CommandType = CommandType.StoredProcedure;

                        oCmd.Parameters.AddWithValue("@result", Response);

                        oCmd.Parameters.AddWithValue("@dttable", dt);
                        oCmd.Parameters.AddWithValue("@rid", rid);
                        oCmd.Parameters.AddWithValue("@OfferCTC", OfferCTC);
                        oCmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogData("Error at UpdateFlag " + ex.ToString(), Log.Status.Error);
            }
        }
    }
}
