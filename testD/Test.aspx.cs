﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using testD.Sfdc;

namespace testD
{
    public partial class Test : System.Web.UI.Page
    {

        private string username = "anas.ahraoui@student.ehb.be";
        private string password = "SVdbERAM1032";
        private const string token = "JAopgk4StlivOMjARAz2koSCw";


        private string _sessionId;
        private DateTime _nextLoginTime;
        private LoginResult _loginResult;
        private SforceService _sForceRef = new SforceService();


        protected void Page_Load(object sender, EventArgs e)
        {

        }



        private bool IsConnected()
        {
            bool connected = false;

            if (!string.IsNullOrEmpty(_sessionId) && _sessionId != null)
            {
                if (DateTime.Now > _nextLoginTime)
                    connected = false;
                else
                    connected = true;
            }
            else
            {
                connected = false;
            }

            return connected;
        }
        private void getSessionInformation()
        {
            _loginResult = new LoginResult();

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            _loginResult = _sForceRef.login(username, password + token);
            _sessionId = _loginResult.sessionId;
            Session["_sessionId"] = _loginResult.sessionId;
            Session["_serverUrl"] = _loginResult.serverUrl;
            Session["_nextLoginTime"] = DateTime.Now;

        }


        private void CreateLead()
        {

            Lead l1 = new Lead();
            l1.FirstName = "Tommy";
            l1.LastName = "Shelby";
            l1.Company = "compCorp";
            l1.Email = "S@compcorp.be";


            SaveResult[] createResult = _sForceRef.create(new sObject[] { l1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;
                Response.Write("<br/>Id:" + id);
                Response.Write("Lead + " + id + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Response.Write("<br/> Error, Lead not added <br/>" + resultaat);
            }


        }
        private void CreateContact()
        {

            Contact c1 = new Contact();
            c1.FirstName = "Samir";
            c1.LastName = "Arwachi";
            c1.Email = "A.Alo@telecom.be";


            SaveResult[] createResult = _sForceRef.create(new sObject[] { c1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;
                Response.Write("<br/>Id:" + id);
                Response.Write("Contact + " + id + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Response.Write("<br/> Error, Contact could not be added <br/>" + resultaat);
            }


        }
        private void CreateAccount()
        {


            Account a1 = new Account();
            a1.Name = "AbdelAcnt";
            a1.Website = "A.Alo.be";
            a1.Phone = "0486555555";


            SaveResult[] createResult = _sForceRef.create(new sObject[] { a1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;
                Response.Write("<br/>Id:" + id);
                Response.Write("Account + " + id + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Response.Write("<br/> Error, Contact could not be added <br/>" + resultaat);
            }




        }



        private string GetLeadID(string searchingLeadName, string searchingLeadEmail)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM LEAD WHERE NAME='" + searchingLeadName + "' AND EMAIL = '" + searchingLeadEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Response.Write("Logged-in user can see a total of "
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
                                Response.Write("Lead " + searchingLeadName + " " + (i + 1) + ": " + id);
                                return id;
                            }
                            else
                            {
                                Response.Write("Lead " + searchingLeadName + " " + (i + 1) + ": " + "ERROR");
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
                    Response.Write("No records found.");
                    return null;

                }
                Response.Write("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }

        }
        private string GetAccountID(string searchingAccountName, string searchingAccountPhone)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM ACCOUNT WHERE NAME='" + searchingAccountName + "' AND PHONE = '" + searchingAccountPhone + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Response.Write("Logged-in user can see a total of "
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
                                Response.Write("Account " + searchingAccountName + " " + (i + 1) + ": " + id);
                                return id;
                            }
                            else
                            {
                                Response.Write("Account " + searchingAccountName + " " + (i + 1) + ": " + "ERROR");
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
                    Response.Write("No records found.");
                    return null;

                }
                Response.Write("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }

        }
        //accounts deleten waarvan contacten of opportunities nog aan gekoppeld zijn gaat niet!
        private string GetContactID(string searchingContactName, string searchingContactEmail)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM CONTACT WHERE NAME='" + searchingContactName + "' AND EMAIL = '" + searchingContactEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Response.Write("Logged-in user can see a total of "
                       + qResult.size + " contact records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Contact c1 = (Contact)records[i];
                            String id = c1.Id;
                            if (id != null)
                            {
                                Response.Write("Contact " + searchingContactName + " " + (i + 1) + ": " + id);
                                return id;
                            }
                            else
                            {
                                Response.Write("Contact " + searchingContactName + " " + (i + 1) + ": " + "ERROR");
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
                    Response.Write("No records found.");
                    return null;

                }
                Response.Write("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }

        }



        private void Delete(string id)
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
                            Response.Write("Deleted Record ID: " + deleteResult.id);
                        }
                        else
                        {
                            // Handle the errors.
                            // We just print the first error out for sample purposes.
                            Error[] errors = deleteResult.errors;
                            if (errors.Length > 0)
                            {
                                Response.Write("Error: could not delete " + "Record ID "
                                      + deleteResult.id + ".");
                                Response.Write("   The error reported was: ("
                                      + errors[0].statusCode + ") "
                                      + errors[0].message + "\n");
                            }
                        }
                    }

                }
                else
                {
                    Response.Write("Error...");
                }
            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                        e.Message + "\n" + e.StackTrace);
            }
        }


        private Lead GetLead(string searchingLeadName, string searchingLeadEmail)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT FirstName, LastName, Id, email, status, phone FROM LEAD WHERE NAME='" + searchingLeadName + "' AND EMAIL = '" + searchingLeadEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Response.Write("Logged-in user can see a total of "
                       + qResult.size + " lead records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Lead l1 = (Lead)records[i];
                            
                            if (l1 != null)
                            {
                                Response.Write("Lead " + searchingLeadName + " found");
                                return l1;
                            }
                            else
                            {
                                Response.Write("Lead " + searchingLeadName + " " + (i + 1) + ": " + "ERROR");
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
                    Response.Write("No records found.");
                    return null;

                }
                Response.Write("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }



        }
        private Contact GetContact(string searchingContactName, string searchingContactEmail)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT FirstName, LastName, Id, email, status, phone FROM CONTACT WHERE NAME='" + searchingContactName + "' AND EMAIL = '" + searchingContactEmail + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Response.Write("Logged-in user can see a total of "
                       + qResult.size + " contact records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Contact c1 = (Contact)records[i];

                            if (c1 != null)
                            {
                                Response.Write("Contact " + searchingContactName + " found");
                                return c1;
                            }
                            else
                            {
                                Response.Write("Contact " + searchingContactName + " " + (i + 1) + ": " + "ERROR");
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
                    Response.Write("No records found.");
                    return null;

                }
                Response.Write("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }



        }
        private Account GetAccount(string searchingAccountName, string searchingAccountPhone)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT FirstName, LastName, Id, email, status, phone FROM ACCOUNT WHERE NAME='" + searchingAccountName + "' AND PHONE = '" + searchingAccountPhone + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Response.Write("Logged-in user can see a total of "
                       + qResult.size + " account records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; ++i)
                        {
                            Account a1 = (Account)records[i];

                            if (a1 != null)
                            {
                                Response.Write("Account " + searchingAccountName + " found");
                                return a1;
                            }
                            else
                            {
                                Response.Write("Account " + searchingAccountName + " " + (i + 1) + ": " + "ERROR");
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
                    Response.Write("No records found.");
                    return null;

                }
                Response.Write("\nQuery succesfully executed.");
                return null;

            }
            catch (SoapException e)
            {
                Response.Write("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return null;

            }



        }

        //____ CONVERTLEAD __ WERKEND ___?? Lead->Contact (gaat eveneens een account en een opportunity aanmaken( dmv company))_//
        private string [] convertLeadToContact()
        {

            
            
                String[] result = new String[4];
                try
                {
                    // Create two leads to convert
                    Lead[] leads = new Lead[1];
                  
                    leads[0] = GetLead("Tommy Shelby", "S@compcorp.be");

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
                        Response.Write("Lead converted successfully!");
                        Response.Write("Account ID: " +
                                     lcResults[j].accountId);
                        Response.Write("Contact ID: " +
                                     lcResults[j].contactId);
                        Response.Write("Opportunity ID: " +
                                     lcResults[j].opportunityId);
                        }
                        else
                        {
                        Response.Write("\nError converting new Lead: " +
                                  lcResults[j].errors[0].message);
                        }
                    }
                }
                catch (SoapException e)
                {
                Response.Write("An unexpected error has occurred: " +
                                      e.Message + "\n" + e.StackTrace);
                }
                return result;
            


        }

        
        private void updateRecord(String[] ids)
        {
                Account[] updates = new Account[2];

                Account account1 = new Account();
                account1.Id = ids[0];
                account1.ShippingPostalCode = "89044";
                updates[0] = account1;

                Account account2 = new Account();
                account2.Id = ids[1];
                account2.NumberOfEmployees = 1000;
                updates[1] = account2;

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









        protected void Button1_Click(object sender, EventArgs e)
        {

            if (!IsConnected())
                getSessionInformation();

            _sForceRef.Url = _loginResult.serverUrl;
            _sForceRef.SessionHeaderValue = new SessionHeader();
            _sForceRef.SessionHeaderValue.sessionId = _loginResult.sessionId;



            //CreateLead();
            //CreateContact();
            //CreateAccount();




            //string id=GetLeadID("Bertha Boxer", "blabla@gmail.com");
            //Delete(id);

            //string id = GetAccountID("Edge Communications", "(512) 757-6000");
            //Delete(id);

            //string id=GetContactID("firstnameContactTwee lastnameContactTwee", "");
            //Delete(id);



            //CreateLead();
            Response.Write(convertLeadToContact());



            //________________________________________//
            //LeadConvert[] leadToConvert = new LeadConvert[0];

            //leadToConvert[0] = new LeadConvert();
            //leadToConvert[0].convertedStatus = "Closed - Converted";
            //_________________________________________//






        }


    }
}