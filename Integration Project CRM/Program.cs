using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Web.Services.Protocols;
using System.Xml;
using Integration_Project_CRM.Sfdc;

namespace Integration_Project_CRM
{
    class Program
    {

        private static void CreateLead(SforceService _sForceRef, string messageType, string uuid, string firstName, string lastName, string email, Int32 timestampLead, Int32 versionLead, bool isActive, bool isBanned, string gsm, DateTime gebDatum, string btwNr, bool gdpr)
        {



            Lead l1 = new Lead();

            l1.UUID__c = uuid;
            l1.FirstName = firstName;
            l1.LastName = lastName;
            l1.Email = email;



            l1.timestamp__cSpecified = true;
            l1.timestamp__c = timestampLead;

            l1.Version__cSpecified = true;
            l1.Version__c = versionLead;

            l1.IsActive__cSpecified = true;
            l1.IsActive__c = isActive;

            l1.IsBanned__cSpecified = true;
            l1.IsBanned__c = isBanned;

            l1.Company = messageType;

            l1.birthdate__cSpecified = true;
            l1.birthdate__c = gebDatum;

            l1.CompanyDunsNumber = btwNr;
            l1.Phone = gsm;

            l1.gdpr__cSpecified = true;
            l1.gdpr__c = gdpr;

            SaveResult[] createResult = _sForceRef.create(new sObject[] { l1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;

                Console.WriteLine("Id:" + id);
                Console.WriteLine("Lead + " + id + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("Error, Lead not added" + resultaat);
            }


        }
        private static void CreateContact(SforceService _sForceRef, string firstName, string lastName, string email/*,string btwnr*/, string gsm)
        {

            Contact c1 = new Contact();
            c1.FirstName = firstName;
            c1.LastName = lastName;
            c1.Email = email;
            c1.Phone = gsm;


            SaveResult[] createResult;

            createResult = _sForceRef.create(new sObject[] { c1 });

            if (createResult[0].success == true)
            {
                string id = createResult[0].id;

                Console.WriteLine("<br/>Id:" + id);
                Console.WriteLine("Contact + " + id + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("<br/> Error, Contact could not be added <br/>" + resultaat);
            }

            Console.WriteLine("Contact succesfully added ");

        }
        private static void CreateAccount(SforceService _sForceRef)
        {


            Account a1 = new Account();
            a1.Name = "AbdelAcnt";
            a1.Website = "A.Alo.be";
            a1.Phone = "0486555555";


            SaveResult[] createResult = _sForceRef.create(new sObject[] { a1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;
                Console.WriteLine("<br/>Id:" + id);
                Console.WriteLine("Account + " + id + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("<br/> Error, Contact could not be added <br/>" + resultaat);
            }




        }



        private static string GetLeadID(SforceService _sForceRef, string searchingLeadName, string searchingLeadEmail)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM LEAD WHERE NAME='" + searchingLeadName + "' AND EMAIL = '" + searchingLeadEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " lead records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Lead l1 = (Lead)records[i];
                            String id = l1.Id;
                            if (id != null)
                            {
                                Console.WriteLine("Lead " + searchingLeadName + " " + (i + 1) + ": " + id);
                                return id;
                            }
                            else
                            {
                                Console.WriteLine("Lead " + searchingLeadName + " " + (i + 1) + ": " + "ERROR");
                                return null;
                            }

                        }
                        if (qResult.done)
                        {
                            done = true;
                        }
                        else
                        {
                            qResult = _sForceRef.queryMore(qResult.queryLocator);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    return null;

                }
                Console.WriteLine("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }

        }
        private static string GetAccountID(SforceService _sForceRef, string searchingAccountName, string searchingAccountPhone)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM ACCOUNT WHERE NAME='" + searchingAccountName + "' AND PHONE = '" + searchingAccountPhone + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " account records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Account a1 = (Account)records[i];
                            string id = a1.Id;
                            if (id != null)
                            {
                                Console.WriteLine("Account " + searchingAccountName + " " + (i + 1) + ": " + id);
                                return id;
                            }
                            else
                            {
                                Console.WriteLine("Account " + searchingAccountName + " " + (i + 1) + ": " + "ERROR");
                                return null;
                            }

                        }
                        if (qResult.done)
                        {
                            done = true;
                        }
                        else
                        {
                            qResult = _sForceRef.queryMore(qResult.queryLocator);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    return null;

                }
                Console.WriteLine("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }

        }
        //accounts deleten waarvan contacten of opportunities nog aan gekoppeld zijn gaat niet!
        private static string GetContactID(SforceService _sForceRef, string searchingContactName, string searchingContactEmail)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM CONTACT WHERE NAME='" + searchingContactName + "' AND EMAIL = '" + searchingContactEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " contact records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; i++)
                        {
                            Contact c1 = (Contact)records[i];
                            String id = c1.Id;
                            if (id != null)
                            {
                                Console.WriteLine("Contact " + searchingContactName + " " + (i + 1) + ": " + id);
                                return id;
                            }
                            else
                            {
                                Console.WriteLine("Contact " + searchingContactName + " " + (i + 1) + ": " + "ERROR");
                                return null;
                            }

                        }
                        if (qResult.done)
                        {
                            done = true;
                        }
                        else
                        {
                            qResult = _sForceRef.queryMore(qResult.queryLocator);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    return null;

                }
                Console.WriteLine("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }

        }



        private static void Delete(SforceService _sForceRef, string id)
        {
            string[] ids = new string[1];
            ids[0] = id;

            try
            {
                if (ids[0] != null)
                {
                    DeleteResult[] deleteResults = _sForceRef.delete(ids);
                    for (int i = 0; i < deleteResults.Length; i++)
                    {
                        DeleteResult deleteResult = deleteResults[i];
                        if (deleteResult.success)
                        {
                            Console.WriteLine("Deleted Record ID: " + deleteResult.id);
                        }
                        else
                        {
                            // Handle the errors.
                            // We just print the first error out for sample purposes.
                            Error[] errors = deleteResult.errors;
                            if (errors.Length > 0)
                            {
                                Console.WriteLine("Error: could not delete " + "Record ID "
                                      + deleteResult.id + ".");
                                Console.WriteLine("   The error reported was: ("
                                      + errors[0].statusCode + ") "
                                      + errors[0].message + "\n");
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Error...");
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                        e.Message + "\n" + e.StackTrace);
            }
        }


        private static Lead GetLead(SforceService _sForceRef, string searchingLeadName, string searchingLeadEmail)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT FirstName, LastName, Id, email, status, phone FROM LEAD WHERE NAME='" + searchingLeadName + "' AND EMAIL = '" + searchingLeadEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " lead records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Lead l1 = (Lead)records[i];

                            if (l1 != null)
                            {
                                Console.WriteLine("Lead " + searchingLeadName + " found");
                                return l1;
                            }
                            else
                            {
                                Console.WriteLine("Lead " + searchingLeadName + " " + (i + 1) + ": " + "ERROR");
                                return null;
                            }

                        }
                        if (qResult.done)
                        {
                            done = true;
                        }
                        else
                        {
                            qResult = _sForceRef.queryMore(qResult.queryLocator);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    return null;

                }
                Console.WriteLine("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }



        }
        private static Contact GetContact(SforceService _sForceRef, string searchingContactName, string searchingContactEmail)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT FirstName, LastName, Id, email, status, phone FROM CONTACT WHERE NAME='" + searchingContactName + "' AND EMAIL = '" + searchingContactEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " contact records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Contact c1 = (Contact)records[i];

                            if (c1 != null)
                            {
                                Console.WriteLine("Contact " + searchingContactName + " found");
                                return c1;
                            }
                            else
                            {
                                Console.WriteLine("Contact " + searchingContactName + " " + (i + 1) + ": " + "ERROR");
                                return null;
                            }

                        }
                        if (qResult.done)
                        {
                            done = true;
                        }
                        else
                        {
                            qResult = _sForceRef.queryMore(qResult.queryLocator);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    return null;

                }
                Console.WriteLine("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }



        }
        private static Account GetAccount(SforceService _sForceRef, string searchingAccountName, string searchingAccountPhone)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT FirstName, LastName, Id, email, status, phone FROM ACCOUNT WHERE NAME='" + searchingAccountName + "' AND PHONE = '" + searchingAccountPhone + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " account records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Account a1 = (Account)records[i];

                            if (a1 != null)
                            {
                                Console.WriteLine("Account " + searchingAccountName + " found");
                                return a1;
                            }
                            else
                            {
                                Console.WriteLine("Account " + searchingAccountName + " " + (i + 1) + ": " + "ERROR");
                                return null;
                            }

                        }
                        if (qResult.done)
                        {
                            done = true;
                        }
                        else
                        {
                            qResult = _sForceRef.queryMore(qResult.queryLocator);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                    return null;

                }
                Console.WriteLine("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }



        }



        //____ CONVERTLEAD __ WERKEND ___?? Lead->Contact (gaat eveneens een account en een opportunity aanmaken( dmv company))_//
        private static string[] convertLeadToContact(SforceService _sForceRef, Lead leadToConvert)
        {



            String[] result = new String[4];
            try
            {
                // Create two leads to convert
                Lead[] leads = new Lead[1];

                leads[0] = leadToConvert;

                // Create a LeadConvert array to be used
                //   in the convertLead() call
                LeadConvert[] leadsToConvert =
                      new LeadConvert[leads.Length]; ;
                for (int i = 0; i < leads.Length; ++i)
                {

                    leadsToConvert[i] = new LeadConvert();
                    leadsToConvert[i].convertedStatus = "Closed - Converted";
                    leadsToConvert[i].leadId = leads[i].Id;
                    result[0] = leads[i].Id;

                }
                // Convert the leads and iterate through the results
                LeadConvertResult[] lcResults =
                      _sForceRef.convertLead(leadsToConvert);
                for (int j = 0; j < lcResults.Length; ++j)
                {
                    if (lcResults[j].success)
                    {
                        Console.WriteLine("Lead converted successfully!");
                        Console.WriteLine("Account ID: " +
                                     lcResults[j].accountId);
                        Console.WriteLine("Contact ID: " +
                                     lcResults[j].contactId);
                        Console.WriteLine("Opportunity ID: " +
                                     lcResults[j].opportunityId);
                    }
                    else
                    {
                        Console.WriteLine("\nError converting new Lead: " +
                                  lcResults[j].errors[0].message);
                    }
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                      e.Message + "\n" + e.StackTrace);
            }
            return result;



        }


        private static void updateRecordLead(SforceService _sForceRef, String idLead, string uuid, string firstName, string lastName, string email, Int32 timestampLead, double versionLead, bool isActive, bool isBanned, string gsm, DateTime gebDatum, string btwNr, bool gdpr)
        {
            Lead[] updates = new Lead[1];




            Lead l1 = new Lead();
            l1.Id = idLead;

            l1.UUID__c = uuid;
            l1.FirstName = firstName;
            l1.LastName = lastName;
            l1.Email = email;



            l1.timestamp__cSpecified = true;
            l1.timestamp__c = timestampLead;

            l1.Version__cSpecified = true;
            l1.Version__c = versionLead;

            l1.IsActive__cSpecified = true;
            l1.IsActive__c = isActive;

            l1.IsBanned__cSpecified = true;
            l1.IsBanned__c = isBanned;

            l1.Company = "none";

            l1.birthdate__cSpecified = true;
            l1.birthdate__c = gebDatum;

            l1.CompanyDunsNumber = btwNr;
            l1.Phone = gsm;

            l1.gdpr__cSpecified = true;
            l1.gdpr__c = gdpr;
            updates[0] = l1;


            try
            {
                SaveResult[] saveResults = _sForceRef.update(updates);
                foreach (SaveResult saveResult in saveResults)
                {
                    if (saveResult.success)
                    {
                        Console.WriteLine("Successfully updated Lead ID: " +
                                  saveResult.id);
                    }
                    else
                    {
                        Error[] errors = saveResult.errors;
                        if (errors.Length > 0)
                        {
                            Console.WriteLine("Error: could not update " +
                                      "Lead ID " + saveResult.id + "."
                                );
                            Console.WriteLine("\tThe error reported was: (" +
                                      errors[0].statusCode + ") " +
                                      errors[0].message + "."
                                );
                        }
                    }
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                               e.Message + "\n" + e.StackTrace);
            }

        }
        private static void updateRecordAccount(SforceService _sForceRef, String idAccount)
        {
            Account[] updates = new Account[1];




            Account account1 = new Account();
            account1.Id = idAccount;

            account1.Name = "Geupdate Naam";

            updates[0] = account1;


            // Invoke the update call and save the results
            try
            {
                SaveResult[] saveResults = _sForceRef.update(updates);
                foreach (SaveResult saveResult in saveResults)
                {
                    if (saveResult.success)
                    {
                        Console.WriteLine("Successfully updated Account ID: " +
                                  saveResult.id);
                    }
                    else
                    {
                        // Handle the errors.
                        // We just print the first error out for sample purposes.
                        Error[] errors = saveResult.errors;
                        if (errors.Length > 0)
                        {
                            Console.WriteLine("Error: could not update " +
                                      "Account ID " + saveResult.id + "."
                                );
                            Console.WriteLine("\tThe error reported was: (" +
                                      errors[0].statusCode + ") " +
                                      errors[0].message + "."
                                );
                        }
                    }
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                               e.Message + "\n" + e.StackTrace);
            }

        }
        private static void updateRecordContact(SforceService _sForceRef, String idContact)
        {
            Contact[] updates = new Contact[1];




            Contact contact1 = new Contact();
            contact1.Id = idContact;

            contact1.FirstName = "Thomas";
            contact1.LastName = "Shelby";

            updates[0] = contact1;


            try
            {
                SaveResult[] saveResults = _sForceRef.update(updates);
                foreach (SaveResult saveResult in saveResults)
                {
                    if (saveResult.success)
                    {
                        Console.WriteLine("Successfully updated Contact ID: " +
                                  saveResult.id);
                    }
                    else
                    {
                        Error[] errors = saveResult.errors;
                        if (errors.Length > 0)
                        {
                            Console.WriteLine("Error: could not update " +
                                      "Contact ID " + saveResult.id + "."
                                );
                            Console.WriteLine("\tThe error reported was: (" +
                                      errors[0].statusCode + ") " +
                                      errors[0].message + "."
                                );
                        }
                    }
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                               e.Message + "\n" + e.StackTrace);
            }

        }

        /*private void SearchLead(string searchingLead)
         {
             SearchResult sResult = _sForceRef.search("FIND {" + searchingLead + "} IN Name FIELDS RETURNING" + " Lead(Id, Name)");

             SearchRecord[] recs = sResult.searchRecords;

             List<Lead> leads = new List<Lead>();

             if (recs.Length > 0)
             {
                 for (int i = 0; i < recs.Length; i++)
                 {
                     sObject rec = recs[i].record;
                     if (rec is Lead)
                     {
                         leads.Add((Lead)rec);
                     }
                 }
             }


             foreach (var l in leads)
             {
                 Response.Write("<br/>Id:" + l.Id + "<br/>" + "Name:" + l.Name + "<br/>__________________<br/>");

             }

         }*/
        /* private string SearchLeadID(string searchingLead)
         {
             SearchResult sResult = _sForceRef.search("FIND {" + searchingLead + "} IN Name FIELDS RETURNING" + " Lead(Id)");

             SearchRecord[] recs = sResult.searchRecords;

             List<Lead> leads = new List<Lead>();

             if (recs.Length > 0)
             {
                 for (int i = 0; i < recs.Length; i++)
                 {
                     sObject rec = recs[i].record;
                     if (rec is Lead)
                     {
                         leads.Add((Lead)rec);
                     }
                 }
             }

             string testIDLead = "";

             foreach (var l in leads)
             {
                 Response.Write("<br/>Id:" + l.Id + "<br/>" + "_________<br/>__________________<br/>");
                 testIDLead = l.Id;
             }


             return testIDLead;
         }*/



        static void Main(string[] args)
        {



            SforceService _sForceRef = null;
            LoginResult CurrentLoginResult = null;
            _sForceRef = new SforceService();



            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                CurrentLoginResult = _sForceRef.login(Constants.USERNAME, Constants.PASSWORD + Constants.TOKEN);
                _sForceRef.Url = CurrentLoginResult.serverUrl;
                _sForceRef.SessionHeaderValue = new SessionHeader();
                _sForceRef.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;




                //___________________________________________________________________________________________________________________________//


                var factory = new ConnectionFactory() { HostName = "10.3.56.27", UserName = "manager", Password = "ehb", Port = 5672 };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    //channel.QueueDeclare(queue: "CRM",
                    //durable: false,
                    //exclusive: false,
                    //autoDelete: false,
                    //arguments: null);

                    channel.QueueBind(queue: "task_queue", exchange: "logs", routingKey: "task");
                    Console.WriteLine("[*] Waiting for Logs");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(message);
                        XmlNodeList xmlList = doc.GetElementsByTagName("messageType");
                        //string messageType = xmlList[0].InnerText.ToLower();
                        string messageType = xmlList[0].InnerText;

                        XmlNodeList uuid = doc.GetElementsByTagName("UUID");
                        XmlNodeList fname = doc.GetElementsByTagName("firstname");
                        XmlNodeList lname = doc.GetElementsByTagName("lastname");
                        XmlNodeList email = doc.GetElementsByTagName("email");
                        XmlNodeList sender = doc.GetElementsByTagName("sender");
                        XmlNodeList gdpr = doc.GetElementsByTagName("GDPR");
                        XmlNodeList timestamp = doc.GetElementsByTagName("timestamp");
                        XmlNodeList version = doc.GetElementsByTagName("version");
                        XmlNodeList isactive = doc.GetElementsByTagName("isActive");
                        XmlNodeList banned = doc.GetElementsByTagName("banned");
                        ///Not Required:
                        XmlNodeList geboortedatum = doc.GetElementsByTagName("geboortedatum");
                        XmlNodeList gsm = doc.GetElementsByTagName("gsm-nummer");
                        XmlNodeList btw = doc.GetElementsByTagName("btw-nummer");
                        XmlNodeList extra = doc.GetElementsByTagName("extraField");


                        switch (messageType)
                        {
                            //CONTACT  

                            case "conatact":


                                CreateContact(_sForceRef, "Sami", "Pete", email[0].InnerText, gsm[0].InnerText);
                                //CreateContact(_sForceRef, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, gsm[0].InnerText);

                                break;

                            ////LEAD
                            ///
                            case "leaddd":

                                // CreateLead(_sForceRef, "AazCCC", "Bonion", "Azeei", "zaee@shelby.com", DateTime.Now, 1, false, true, "23474352", new DateTime(1997, 08, 14), "235775", true);
                                CreateLead(_sForceRef,messageType, uuid[0].InnerText, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), Convert.ToBoolean(isactive[0].InnerText), Convert.ToBoolean(banned[0].InnerText), gsm[0].InnerText, Convert.ToDateTime(geboortedatum[0].InnerText), btw[0].InnerText, Convert.ToBoolean(gdpr[0].InnerText));


                                //OK--> CreateLead(_sForceRef, messageType, uuid[0].InnerText, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, 234234, 2, true, true, gsm[0].InnerText, new DateTime(2000, 12, 12), btw[0].InnerText, true);
                                //CreateLead(_sForceRef, messageType, uuid[0].InnerText, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, 234234, 2, Convert.ToBoolean(isactive[0].InnerText), Convert.ToBoolean(banned[0].InnerText), gsm[0].InnerText, new DateTime(2000, 12, 12), btw[0].InnerText, Convert.ToBoolean(gdpr[0].InnerText));


                                break;

                            case "delete":


                                string id = GetLeadID(_sForceRef, "Bonion Azeei", "zaee@shelby.com");
                                Delete(_sForceRef, id);
                                break;

                            case "ConvertLead_Contact":

                                Lead lead = GetLead(_sForceRef, "Onion Azoi", "zaaa@shelby.com");
                                convertLeadToContact(_sForceRef, lead);


                                break;

                            case "update":

                                string idTeUpdatenRec = GetLeadID(_sForceRef, "John Wick", "wiki@gmail.com");
                                updateRecordLead(_sForceRef, idTeUpdatenRec, "DE3ACAAA", "John", "Weak", "jonny@wiki.com", 566465465, 3, false, false, "0000003", new DateTime(1960, 01, 04), "000005934", false);

                                break;

                            default:
                                Console.WriteLine("Invalid incoming message");
                                break;

                        }


                    };

                    channel.BasicConsume(queue: "task_queue",
                                    autoAck: true,
                                    consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }

                //___________________________________________________________________________________________________________________________//



            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                // This is likley to be caused by bad username or password
                _sForceRef = null;
                Console.WriteLine("SOAP Exception occured " + e.Message.ToString());
            }
            catch (Exception e)
            {
                // This is something else, probably comminication
                _sForceRef = null;
                Console.WriteLine("Exception occured " + e.Message.ToString());
            }









        }
    }
}
