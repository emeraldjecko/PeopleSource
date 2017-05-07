using PeoplesSource.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PeoplesSource.Ebay.Models;
using PeoplesSource.Helpers;
using PeoplesSource.EWReturn;
using PeoplesSource.Models;
using System.Globalization;

namespace CallDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            GetData();
        }

        private static void GetData()
        {


            var Msg = new EbayMessage();
            var Master_Msg = new MasterMessage();
            string error = "";
            PeopleSourceEntities db = new PeopleSourceEntities();
            var sellerList = db.Sellers.ToList();
            try
            {
                foreach (var seller in sellerList)
                {

                    GetMyMessagesResponse view = null;
                    Console.Write("\nCheck Existed Data of Seller " + seller.SellarName + "");
                    if (!string.IsNullOrEmpty(seller.UserToken))
                    {
                        string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                                "<GetMyMessagesRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">" +
                                  "<RequesterCredentials>" +
                                        "<eBayAuthToken>" + seller.UserToken + "</eBayAuthToken>" +
                                  "</RequesterCredentials>" +
                                  "<WarningLevel>High</WarningLevel>" +
                                  "<DetailLevel>ReturnHeaders</DetailLevel>" +
                                "</GetMyMessagesRequest>";

                        XmlDocument xmlDoc = APICall.MakeAPIRequest(strReq, "GetMyMessages", "POST", error, seller.IsProxyRequired,
                                            seller.ProxyIP, seller.ProxyPort, seller.ProxyUsername, seller.ProxyPassword);

                        if (error == "")
                        {
                            //get the root node, for ease of use
                            XmlNode root = xmlDoc["GetMyMessagesResponse"];
                            if (root != null)
                            {
                                if (root["Errors"] != null)
                                {
                                    error += root["Errors"]["ErrorCode"].InnerText + " ";
                                    error += root["Errors"]["ShortMessage"].InnerText + " ";
                                    error += root["Errors"]["LongMessage"].InnerText;
                                }
                                else
                                {
                                    if (root["Ack"].InnerText == "Success")
                                    {
                                        string jsonText = JsonConvert.SerializeXmlNode(root);
                                        string xmls = xmlDoc.InnerXml.Replace(" xmlns=\"urn:ebay:apis:eBLBaseComponents\"", "");
                                        using (TextReader sr = new StringReader(xmls))
                                        {
                                            XmlSerializer serializer = new XmlSerializer(typeof(GetMyMessagesResponse));
                                            view = serializer.Deserialize(sr) as GetMyMessagesResponse;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Saving All Message Related Data In DataBase


                    var message_ids = new List<decimal>();

                    foreach (var values in view.Messages)
                    {
                        if (!string.IsNullOrEmpty(values.MessageID))
                        {
                            message_ids.Add(Convert.ToDecimal(values.MessageID));
                        }
                    }
                    var existing_messages = db.EbayMessages.Where(m => message_ids.Contains(m.EbayMessageid)).ToList();
                    var existing_Master_Messages = db.MasterMessages.ToList();
                    bool is_unique = true;
                    var messages_list = new List<EbayMessage>();
                    var Master_messages_list = new List<MasterMessage>();
                    foreach (var new_messages in view.Messages)
                    {
                        foreach (var existing_message in existing_messages)
                        {
                            if (existing_message.EbayMessageid == Convert.ToDecimal(new_messages.MessageID))
                            {
                                is_unique = false;
                            }
                        }

                        if (is_unique)
                        {
                            if (new_messages.Sender.ToLower() != "ebay")
                            {
                                var subject = new_messages.Subject;
                                if (subject.StartsWith("Re:"))
                                {
                                    subject = subject.Substring(4);
                                }
                                var Master_Msges = existing_Master_Messages.FirstOrDefault(m => m.MessageSubject.Trim() == subject.Trim() && m.Sellerid == seller.Sellerid);
                                if (Master_Msges == null)
                                {
                                    Master_Msg = new MasterMessage();
                                    Master_Msg.MessageSubject = subject.Trim();
                                    Master_Msg.Sellerid = seller.Sellerid;
                                    Master_Msg.MessageRecieveDate = new_messages.ReceiveDate;
                                    Master_Msg.MessageSenderid = Convert.ToInt32(new_messages.SendingUserID);
                                    Master_Msg.MessageExpiryDate = new_messages.ExpirationDate;
                                    Master_Msg.MessageItemid = new_messages.ItemID;
                                    Master_Msg.Sender = new_messages.Sender;
                                    Master_Msg.IsRead = new_messages.Read;
                                    Master_Msg.MessageType = new_messages.MessageType;
                                    Master_Msg.Type = true;
                                    existing_Master_Messages.Add(Master_Msg);
                                    Msg = new EbayMessage();

                                    Msg.MasterMessage = Master_Msg;
                                    Msg.EbayMessageid = Convert.ToDecimal(new_messages.MessageID);
                                    Msg.MessageSubject = subject.Trim();
                                    Msg.Sellerid = seller.Sellerid;
                                    Msg.MessageRecieveDate = new_messages.ReceiveDate;
                                    Msg.MessageSenderid = Convert.ToInt32(new_messages.SendingUserID);
                                    Msg.MessageExpiryDate = new_messages.ExpirationDate;
                                    Msg.MessageItemid = new_messages.ItemID;
                                    Msg.Sender = new_messages.Sender;
                                    Msg.IsRead = new_messages.Read;
                                    Msg.MessageType = new_messages.MessageType;
                                    Msg.Type = true;
                                    messages_list.Add(Msg);



                                }
                                else
                                {

                                    Master_Msges.MessageRecieveDate = (new_messages.ReceiveDate > Master_Msges.MessageRecieveDate) ? new_messages.ReceiveDate : Master_Msges.MessageRecieveDate;

                                    Msg = new EbayMessage();
                                    Msg.MasterMessage = Master_Msges;
                                    Msg.EbayMessageid = Convert.ToDecimal(new_messages.MessageID);
                                    Msg.MessageSubject = subject.Trim();
                                    Msg.Sellerid = seller.Sellerid;
                                    Msg.MessageRecieveDate = new_messages.ReceiveDate;
                                    Msg.MessageSenderid = Convert.ToInt32(new_messages.SendingUserID);
                                    Msg.MessageExpiryDate = new_messages.ExpirationDate;
                                    Msg.MessageItemid = new_messages.ItemID;
                                    Msg.Sender = new_messages.Sender;
                                    Msg.IsRead = new_messages.Read;
                                    Msg.MessageType = new_messages.MessageType;
                                    Msg.Type = true;
                                    messages_list.Add(Msg);
                                }
                            }
                            else
                            {
                                decimal MsgId = Convert.ToDecimal(new_messages.MessageID);
                                var ebay_message = existing_messages.FirstOrDefault(m => m.EbayMessageid == MsgId);
                                if (ebay_message == null)
                                {
                                    var subject = new_messages.Subject;
                                    if (subject.StartsWith("Re:"))
                                    {
                                        subject = subject.Substring(4);
                                    }
                                    Master_Msg = new MasterMessage();
                                    Master_Msg.MessageSubject = subject.Trim();
                                    Master_Msg.Sellerid = seller.Sellerid;
                                    Master_Msg.MessageRecieveDate = new_messages.ReceiveDate;
                                    Master_Msg.MessageSenderid = Convert.ToInt32(new_messages.SendingUserID);
                                    Master_Msg.MessageExpiryDate = new_messages.ExpirationDate;
                                    Master_Msg.MessageItemid = new_messages.ItemID;
                                    Master_Msg.Sender = new_messages.Sender;
                                    Master_Msg.IsRead = new_messages.Read;
                                    Master_Msg.MessageType = new_messages.MessageType;
                                    Master_Msg.Type = true;
                                    existing_Master_Messages.Add(Master_Msg);
                                    Msg = new EbayMessage();

                                    Msg.MasterMessage = Master_Msg;
                                    Msg.EbayMessageid = Convert.ToDecimal(new_messages.MessageID);
                                    Msg.MessageSubject = subject.Trim();
                                    Msg.Sellerid = seller.Sellerid;
                                    Msg.MessageRecieveDate = new_messages.ReceiveDate;
                                    Msg.MessageSenderid = Convert.ToInt32(new_messages.SendingUserID);
                                    Msg.MessageExpiryDate = new_messages.ExpirationDate;
                                    Msg.MessageItemid = new_messages.ItemID;
                                    Msg.Sender = new_messages.Sender;
                                    Msg.IsRead = new_messages.Read;
                                    Msg.MessageType = new_messages.MessageType;
                                    Msg.Type = true;
                                    messages_list.Add(Msg);


                                }
                            }
                        }
                    }
                    //if (Master_messages_list.Count > 0)
                    //{
                    //    db.MasterMessages.AddRange(Master_messages_list);
                    //}
                    if (messages_list.Count > 0)
                    {
                        db.EbayMessages.AddRange(messages_list);
                    }
                 
                    //db.SaveChanges();
                    Console.Write("\nCheck New Added Data For Seller " + seller.SellarName + "");
                    var c = new ReturnManagementService();
                    c.IpAddress = seller.ProxyIP;
                    c.Port = seller.ProxyPort;
                    c.UserName = seller.ProxyUsername;
                    c.Password = seller.ProxyPassword;
                    c.IsCredentialRequired = (bool)seller.IsCredentialsRequired;
                    c.IsProxyRequired = seller.IsProxyRequired;
                    c.OperationName = "getUserReturns";
                    //c.Token = "AgAAAA**AQAAAA**aAAAAA**aL52VQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AGkYChC5iHog+dj6x9nY+seQ**ZuICAA**AAMAAA**tDLOPafSIYVbRh13kVRnH8Xj5NVQ9zQa4Jlo4q5DtzuXAZot6ZU3mcwMGffAHl5Gei83rdFImVegPKTZzyrdG7cKgpkRBbZnwrnrcHyI7CEFqrLVPcqGx2LG2kbzEMQi4kNk78Y22vKNT1edvPFREcNYrG1nsybQ/UQbvGD7wbOa2y87Xy+2dCoDhH8ybsTZ5PdjYb6WOyTNIomwBYHql9+CiaRbSx3s8h9Uv4dh4I+A0O1xKt0upgutStb2xoeiDfcWB/olxuXGNKxeuJZ5ZiTmLoI6NVr4FN3W5ddOBhkJqmaGMzdPP7rWQF52okykVKbhsrsKIEPQWYI+JHpqh+6AyrbIrTaNyqyPWW2Lh7gZbOJ9mA/vOn3eJl/2o/wae8e19AxQ6u6OH3hZh0XCQfdApCCDnk0SshqFABsnLYX2Bs/9TQpNUZM71WjA5ExKd/RWrWLTDTqCsrcNKSnyqX6BYkJp15heWODVVyNXmKzDStwfxTY1y49euRfxIZTeP4ufO735+z2U6MV+g9rwDcHg+WtpOF1d2EIW2IehfcOwrZyH7fug29eUV3n1vCVQp4maqkCBUYz71XqPycfbBrnlqt7Xk1qwwsjMUMfBvBFVb3fbbG2CNTn6sznyhzRS7T5tjiaMuWIMjl/lyVoxdVWB6i/CBEgql8XtvxWsUxIW25QsWVhu03KYkSBUsG+n2bIPeGipad7NvWZjqOpnK78nEVKcFynHVTkWNspRHII9XtQ1qdpO7i5gm4imUxw5";
                    c.Token = seller.UserToken;
                    var returnItem = new ReturnModel();
                    var request = new getUserReturnsRequest();
                    var return_ids = new List<decimal>();


                    var returns = c.getUserReturns(request);

                    if (returns.returns != null)
                    {
                        Console.Write("\nCheck Return Messages Data For Seller " + seller.SellarName + "");
                        foreach (var values in returns.returns)
                        {
                            if (!string.IsNullOrEmpty(values.ReturnId.id))
                            {
                                return_ids.Add(Convert.ToDecimal(values.ReturnId.id));
                            }
                        }

                        var existing_return = db.EbayMessages.Where(m => return_ids.Contains(m.EbayMessageid)).ToList();
                        bool is_unique_return = true;
                        var return_list = new List<EbayMessage>();
                         var return_Master_list = new List<MasterMessage>();
                         foreach (var item in returns.returns)
                         {
                             foreach (var existing_returns in existing_return)
                             {
                                 if (existing_returns.EbayMessageid == Convert.ToDecimal(item.ReturnId.id))
                                 {
                                     is_unique_return = false;
                                 }
                             }

                             if (is_unique_return)
                             {
                                 var Messagesubject = "";
                                 if (!string.IsNullOrEmpty(item.returnRequest.comments))
                                 {
                                     if (!string.IsNullOrEmpty(item.returnRequest.returnItem[0].itemId))
                                         Messagesubject = item.returnRequest.comments.ToString() + " (Item ID : " + item.returnRequest.returnItem[0].itemId + ")";
                                     else
                                         Messagesubject = item.returnRequest.comments.ToString();
                                 }
                                 else
                                 {
                                     Messagesubject = "Return From " + item.otherParty.userId + " (Item ID : " + item.returnRequest.returnItem[0].itemId + ")";
                                 }

                                 Master_Msg = new MasterMessage();
                                 Master_Msg.MessageSubject = Messagesubject.Trim();
                                 Master_Msg.Sellerid = seller.Sellerid;
                                 Master_Msg.MessageRecieveDate = Convert.ToDateTime(item.creationDate);
                                 Master_Msg.MessageSenderid = Convert.ToDecimal(item.ReturnId.id);
                                 Master_Msg.MessageItemid = Convert.ToDecimal(item.returnRequest.returnItem[0].itemId);
                                 Master_Msg.Sender = item.otherParty.userId;
                                 Master_Msg.MessageType = item.ReturnType.ToString();
                                 Master_Msg.Type = false;
                                 Master_Msg.IsRead = true;
                                 Msg = new EbayMessage();
                                 Msg.MasterMessage = Master_Msg;
                                 Msg.EbayMessageid = Convert.ToDecimal(item.ReturnId.id);
                                 Msg.MessageSubject = Messagesubject.Trim();
                                 Msg.Sellerid = seller.Sellerid;
                                 Msg.MessageRecieveDate = Convert.ToDateTime(item.creationDate);
                                 Msg.MessageSenderid = Convert.ToDecimal(item.ReturnId.id);
                                 Msg.MessageItemid = Convert.ToDecimal(item.returnRequest.returnItem[0].itemId);
                                 Msg.Sender = item.otherParty.userId;
                                 Msg.MessageType = item.ReturnType.ToString();
                                 Msg.Type = false;
                                 Msg.IsRead = true;
                                 return_list.Add(Msg);
                             }
                         }
                        if (return_list.Count > 0)
                        {
                            db.EbayMessages.AddRange(return_list);
                        }
                        // if (return_Master_list.Count > 0)
                        //{
                        //    db.MasterMessages.AddRange(return_Master_list);
                        //}
                    }
                }
                db.SaveChanges();
            }catch (Exception Ex)
            {
                Console.Write("\nError in saving data (Programe.cs)");
                Console.Write("\nError Message : " + Ex.Message + "");
            }
            ///////////////////////////////////////////////////////////////////////////
            try
            {
                var MessagesList = db.MasterMessages.Where(m => m.DeleteDate == null);
                var Clone = MessagesList;
                var Filteration = db.Filterations.Where(m => m.DeleteDate == null).ToList();
                if (Filteration != null)
                {
                    Console.Write("\nCheck For Applicable Filter");
                    foreach (var Data in Filteration)
                    {
                        MessagesList = Clone;
                        if (!string.IsNullOrEmpty(Data.Filter_HasWord))
                        {
                            MessagesList = MessagesList.Where(m => m.Note.Contains(Data.Filter_HasWord) || m.MessageSubject.Contains(Data.Filter_HasWord) || m.Sender.Contains(Data.Filter_HasWord) || m.MessageType.Contains(Data.Filter_HasWord));
                        }
                        if (!string.IsNullOrEmpty(Data.Filter_From))
                        {
                            MessagesList = MessagesList.Where(m => m.Sender.Contains(Data.Filter_From));
                        }
                        if (!string.IsNullOrEmpty(Data.Filter_To))
                        {
                            MessagesList = MessagesList.Where(m => m.Seller.SellarName.Contains(Data.Filter_To));
                        }
                        if (!string.IsNullOrEmpty(Data.Filter_TagName))
                        {
                            MessagesList = MessagesList.Where(m => (m.MessageTags.Where(k => k.Tag.TagName.Trim() == Data.Filter_TagName.ToLower().Trim())).Count() > 0);
                        }
                        if (!string.IsNullOrEmpty(Data.Filter_Subject))
                        {
                            MessagesList = MessagesList.Where(m => m.MessageSubject.Contains(Data.Filter_Subject));
                        }
                        if (!string.IsNullOrEmpty(Data.Filter_FromDate) && !string.IsNullOrEmpty(Data.Filter_ToDate))
                        {
                            DateTime FromDatetime = DateTime.ParseExact(Data.Filter_FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                            DateTime ToDatetime = DateTime.ParseExact(Data.Filter_ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                            MessagesList = MessagesList.Where(m => m.MessageRecieveDate >= FromDatetime && m.MessageRecieveDate <= ToDatetime);
                        }
                        if (Data.Filter_HasNote == true)
                        {
                            MessagesList = MessagesList.Where(m => m.Note != null);
                        }
                        List<MessageTag> AssignTag = new List<MessageTag>();
                        foreach (var NewData in MessagesList)
                        {
                            if (Data.Action_Type == "Read")
                            {
                                NewData.IsRead = true;
                            }
                            else if (Data.Action_Type == "UnRead")
                            {
                                NewData.IsRead = false;
                            }
                            if (!string.IsNullOrEmpty(Data.Action_Note))
                            {
                                NewData.Note = Data.Action_Note;
                            }
                            if (Data.Action_Delete == true)
                            {
                                NewData.DeleteDate = DateTime.Now;

                                foreach (var TagData in NewData.MessageTags)
                                {
                                    TagData.DeleteDate = DateTime.Now;
                                }
                            }
                            if (Data.Tagid != 0)
                            {
                                if (Data.Action_Delete == true)
                                {
                                    AssignTag.Add(new MessageTag
                                    {
                                        MasterMessageid = Convert.ToDecimal(NewData.MasterMessageid),
                                        Tagid = (decimal)Data.Tagid,
                                        DeleteDate = DateTime.Now
                                    });
                                }
                                else
                                {
                                    decimal TagID = (decimal)Data.Tagid;
                                    decimal MsgID = Convert.ToDecimal(NewData.MasterMessageid);
                                    int Check = db.MessageTags.Where(m => m.Tagid == TagID && m.MasterMessageid == MsgID && m.DeleteDate == null).Count();
                                    if (Check == 0)
                                    {
                                        AssignTag.Add(new MessageTag
                                        {
                                            MasterMessageid = Convert.ToDecimal(NewData.MasterMessageid),
                                            Tagid = (decimal)Data.Tagid
                                        });
                                    }
                                }
                            }
                        }
                        if (Data.Tagid != 0)
                        {
                            db.MessageTags.AddRange(AssignTag);
                            db.SaveChanges();
                        }
                    }
                    Console.Write("\nFilter Applied");
                }
            }
            catch (Exception Ex)
            {
                Console.Write("\nError Occurs in Programe.cs File..");
                Console.Write("\nError Message : " + Ex.Message + "");
            }
            Console.Write("\nExit");
        }
    }
}
