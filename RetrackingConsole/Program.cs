using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net.Mail;


namespace RetrackingJob
{
    class Program
    {
        static void Main(string[] args)
        {
            string txtFileNameNow = DateTime.Now.ToString("MMddyyyy") + ".txt";
            string txtFileNamePrv = DateTime.Now.AddDays(-1).ToString("MMddyyyy") + ".txt";

            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\" + txtFileNamePrv))
            {
                File.Delete(System.IO.Directory.GetCurrentDirectory() + @"\" + txtFileNamePrv);
            }

            string mntNow = DateTime.Now.ToString("MMMM");
            string dayNow = DateTime.Now.Day.ToString();
            string yrNow = DateTime.Now.Year.ToString();


            string fullDateNow = mntNow + " " + dayNow + ", " + yrNow;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=DESKTOP-O9O7Q8M\MSSQLSERVERDEV;" +
                    "Initial Catalog=PeopleSource;Integrated Security=SSPI;";
                conn.Open();

                SqlCommand command = new SqlCommand(@"SELECT * FROM [Order] WHERE txn_id LIKE '%RET%' AND tracking IS NOT NULL AND item_sku LIKE '%AAA%' AND TrackingClosedStatus = 0", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!string.IsNullOrEmpty(reader["tracking"].ToString()))
                        {
                            Console.WriteLine("processing trackingID:" + reader["tracking"].ToString());
                            string trackingDetailsAPI = getDetails(reader["tracking"].ToString());
                            if (trackingDetailsAPI.Contains(fullDateNow))
                            {

                                XmlDocument xd = new XmlDocument();

                                xd.LoadXml(trackingDetailsAPI);

                                string trackingID = "";
                                string trackingSummary = "";

                                foreach (XmlNode node in xd.LastChild)
                                {
                                    trackingID = node.Attributes["ID"].Value.ToString();
                                    trackingSummary = node.ChildNodes[0].InnerText.ToString();
                                    for (int i = 1; i < 2; i++)
                                    {
                                        string trackingInfo = node.ChildNodes[i].InnerText.ToString();

                                        List<TrackerDetailsResponseModel> TrackDetail = new List<TrackerDetailsResponseModel>() { };
                                        TrackDetail.Add(SplitStringArray(trackingInfo));
                                        Console.WriteLine("Details:" + trackingInfo);
                                        string strBody = $"Date:{DateTime.Now} <br /> " +
                                                         $"Tracking Number: {reader["tracking"]} <br /> " +
                                                         $"Buyer Email: {reader["payer_email"]} <br /> " +
                                                         $"Seller Id: {reader["account"]} <br /> " +
                                                         $"Buyer Name: {reader["name"]} <br /> " +
                                                         $"Buyer Address: {reader["address_street"]} {reader["address_street2"]} {reader["address_city"]} {reader["address_state"]} {reader["address_country"]} {reader["address_zip"]} <br /> " +
                                                         $"Item SKU: {reader["item_sku"]} <br /> " +
                                                         $"Item Description: {reader["item_description"]} <br /> " +
                                                         $"Status: {TrackDetail[0].OrderStatus} <br /> " +
                                                         $"Tracking Summary: {trackingSummary} <br /> ";

                                        if (File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\" + txtFileNameNow))
                                        {
                                            if (!searchString(trackingID + " " + trackingInfo, txtFileNameNow))
                                            {
                                                sendEmail(strBody, TrackDetail[0].OrderStatus);
                                                write2file(txtFileNameNow, trackingID + " " + trackingInfo);
                                            }
                                        }
                                        else
                                        {
                                            sendEmail(strBody, TrackDetail[0].OrderStatus);
                                            write2file(txtFileNameNow, trackingID + " " + trackingInfo);
                                        }

                                    }

                                    Console.WriteLine("trackingID:" + trackingID);
                                    Console.WriteLine("trackingSummary:" + trackingSummary);
                                }
                            }

                        }
                    }
                }

                //Console.Clear();

            }
        }

        static public void sendEmail(string body, string stat)
        {
            try
            {
                string UserEmail = "synctunoreply@gmail.com";
                string Password = "7OJ#zUA%%596";

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(UserEmail);
                mail.To.Add("forwaren@gmail.com");
                mail.Subject = "SyncTu Order Status Update: " + stat;
                mail.IsBodyHtml = true;
                mail.Body = body;


                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(UserEmail, Password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception)
            {

            }
        }

        static public string getDetails(string TrackingId)
        {
            string UserId = "297TOPGO2294";
            string Password = "352NT04XF253";
            string ApiUrl = "http://production.shippingapis.com/ShippingAPI.dll?API=TrackV2 ";
            string CompleteUrl = string.Concat(ApiUrl, string.Format("&XML=<TrackRequest USERID=\"{0}\"><TrackID ID=\"{1}\"></TrackID></TrackRequest>", UserId, TrackingId));
            WebRequest req = WebRequest.Create(CompleteUrl);
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(string.Concat(UserId, ":", Password)));
            //req.Credentials = new NetworkCredential("username", "password");

            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new
              StreamReader(resp.GetResponseStream(), enc);

            string Response = loResponseStream.ReadToEnd();

            loResponseStream.Close();
            resp.Close();

            return Response;
        }

        static public TrackerDetailsResponseModel SplitStringArray(string str)
        {
            string pattern = @"(?<=\D)[\,]";
            string substitution = @"-";
            List<string> FinalTrackingDetails = new List<string>();

            Regex regex = new Regex(pattern);
            string result = regex.Replace(str, substitution);
            List<string> Splitstring = result.Split('-').ToList();
            int i = 0;
            string currentValue = string.Empty;
            foreach (string value in Splitstring)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(value);
                    FinalTrackingDetails.Add(currentValue);
                    FinalTrackingDetails.Add(value);
                    currentValue = string.Empty;
                }
                catch (Exception e)
                {
                    currentValue += value;
                }
            }
            if (!string.IsNullOrEmpty(currentValue))
                FinalTrackingDetails.Add(currentValue);

            TrackerDetailsResponseModel trackerDetailsResponseModel = new TrackerDetailsResponseModel() { };
            for (int s = 0; s < FinalTrackingDetails.Count; s++)
            {
                switch (s)
                {
                    case 0: trackerDetailsResponseModel.OrderStatus = FinalTrackingDetails[s]; break;
                    case 1: trackerDetailsResponseModel.OrderDate = FinalTrackingDetails[s]; break;
                    case 2: trackerDetailsResponseModel.OrderLocation = FinalTrackingDetails[2]; break;
                }

            }
            return trackerDetailsResponseModel;
        }

        static public void write2file(string txtFile, string details)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\" + txtFile, true))
            {
                file.WriteLine(details);
            }
        }

        static public bool searchString(string details, string txtFile)
        {
            string[] lines = System.IO.File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + @"\" + txtFile);
            foreach (string line in lines)
            {
                if (line == details) return true;
            }
            return false;
        }
    }

    
}
