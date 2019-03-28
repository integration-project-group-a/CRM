using System;
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
            l1.FirstName = "TestLead2";
            l1.LastName = "APAPAPa";
            l1.Company = "azea";
            l1.Email = "A.azezaeazeaz@telecom.be";


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
            c1.FirstName = "AbdelContact";
            c1.LastName = "Alo";
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
        private void SearchLead(string searchingLead)
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

        }
        private string SearchLeadID(string searchingLead)
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
        }
        private void DeleteLead(string [] ids)
        {
            try
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
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                        e.Message + "\n" + e.StackTrace);
            }
        }









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
            //SearchLeadID(searchLeadX);

            string searchLeadX = "Drilon Kryeziu";
            string idTESTA = SearchLeadID(searchLeadX);
            Response.Write("<br/>@@@@@@@@@@@@@@@@Id:" + idTESTA + "<br/>__________________<br/>");
            SearchLead(searchLeadX);
            string[] ids=new string[1];

            ids[0] = idTESTA;

           // DeleteLead(ids);






            //________________________________________//
            //LeadConvert[] leadToConvert = new LeadConvert[0];

            //leadToConvert[0] = new LeadConvert();
            //leadToConvert[0].convertedStatus = "Closed - Converted";
            //_________________________________________//






        }


    }
}