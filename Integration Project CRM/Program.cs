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

            
            l1.btw__c = btwNr;
            l1.Phone = gsm;

            l1.gdpr__cSpecified = true;
            l1.gdpr__c = gdpr;

            SaveResult[] createResult = _sForceRef.create(new sObject[] { l1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;

                Console.WriteLine("Lead " + firstName+" "+ lastName + " succesfully added "+ Environment.NewLine+"UUID of the created Lead: "+ uuid);

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("Error, Lead not added."+ Environment.NewLine +"ERROR> "+ resultaat);
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
        private static void CreateEvent(SforceService _sForceRef, string nameEvent, string uuid, Int32 timestamp, Int32 version, bool isActive)
        {



            EEvent__c event1 = new EEvent__c();

            event1.UUID__c = uuid;
            event1.Name = nameEvent;

            event1.IsActive__cSpecified = true;
            event1.timestamp__cSpecified = true;
            event1.Version__cSpecified = true;
            event1.timestamp__c = timestamp;
            event1.Version__c = version;
            event1.IsActive__c = isActive;



            SaveResult[] createResult = _sForceRef.create(new sObject[] { event1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;

                Console.WriteLine("Event:" + nameEvent + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("Error, event "+ nameEvent + " not added. "+ Environment.NewLine +"ERROR> "+ resultaat);
            }


        }
        private static void CreateSession(SforceService _sForceRef, string nameSession, string uuidEvent, string uuidSession ,string description,string lokaal, DateTime start,DateTime end)
        {



            SSession__c session1 = new SSession__c();

            session1.UUID_Event__c = uuidEvent;

            session1.UUID_Session__c = uuidSession;

            session1.Name = nameSession;
            session1.Description__c = description;
            session1.Lokaal__c = lokaal;

            session1.StartDTime__cSpecified = true;
            session1.EndDTime__cSpecified = true;

            session1.StartDTime__c = start;
            session1.EndDTime__c = end;


            SaveResult[] createResult = _sForceRef.create(new sObject[] { session1 });

            if (createResult[0].success)
            {
                string id = createResult[0].id;

                Console.WriteLine("Session:" + nameSession + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("Error, event " + nameSession + " not added. " + Environment.NewLine + "ERROR> " + resultaat);
            }


        }
        private static void CreateGamingGroup(SforceService _sForceRef, string name, string uuidGroup,string uuidLeader,string uuidGame, Int32 timestamp, Int32 version, bool isActive,bool banned)
        {



            GameGroup__c gameGroup = new GameGroup__c();

            gameGroup.Name = name;
            gameGroup.UUID_Group__c = uuidGroup;

            gameGroup.timestamp__cSpecified = true;
            gameGroup.timestamp__c = timestamp;

            gameGroup.Version__cSpecified = true;
            gameGroup.Version__c = version;

            gameGroup.UUID_Game__c = uuidGame;
            gameGroup.GroupLeader_UUID__c = uuidLeader;

            gameGroup.IsActive__cSpecified = true;
            gameGroup.Banned__cSpecified = true;

            gameGroup.IsActive__c = isActive;
            gameGroup.Banned__c = banned;


        
            



            SaveResult[] createResult = _sForceRef.create(new sObject[] { gameGroup });

            if (createResult[0].success)
            {
                string id = createResult[0].id;

                Console.WriteLine("Gaming-group:" + name + " succesfully added ");

            }
            else
            {
                string resultaat = createResult[0].errors[0].message;
                Console.WriteLine("Error, Gaming-group " + name + " not added. " + Environment.NewLine + "ERROR> " + resultaat);
            }


        }




        private static string GetLeadID(SforceService _sForceRef,string uuid)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM LEAD WHERE UUID__c='" + uuid  +"'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Lead with UUID: "+uuid+" has been found!");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; i++)
                        {
                            Lead l1 = (Lead)records[i];
                            String id = l1.Id;
                            if (id != null)
                            {
                                Console.WriteLine("Lead with following UUID" + uuid + " has the following SalesForce-ID: " + id);
                                return id;
                            }
                            else
                            {
                                Console.WriteLine("Lead with following UUID" + uuid + " has no SalesForce-ID ..."+ Environment.NewLine+ "ERROR");
                                return "empty";
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
                    Console.WriteLine("No Lead found.");
                    return "empty";

                }
                Console.WriteLine("Query succesfully executed."+Environment.NewLine+"No Lead with UUID: "+uuid+" founded in SalesForce...");
                return "empty";

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return "empty";

            }

        }
        private static string GetEventID(SforceService _sForceRef, string uuid)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT Id FROM EEvent__c WHERE UUID__c='" + uuid + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Event with UUID: " + uuid + " has been found!");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; i++)
                        {
                            EEvent__c e1 = (EEvent__c)records[i];
                            String id = e1.Id;
                            if (id != null)
                            {
                                Console.WriteLine("Event with following UUID" + uuid + " has the following SalesForce-ID: " + id);
                                return id;
                            }
                            else
                            {
                                Console.WriteLine("Event with following UUID" + uuid + " has no SalesForce-ID ..." + Environment.NewLine + "ERROR");
                                return "empty";
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
                    Console.WriteLine("No Event found.");
                    return "empty";

                }
                Console.WriteLine("Query succesfully executed." + Environment.NewLine + "No Event with UUID: " + uuid + " founded in SalesForce...");
                return "empty";

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return "empty";

            }

        }
        

        private static string GetGamingGroupID(SforceService _sForceRef, string uuid)
        {
            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT id FROM GameGroup__c WHERE UUID_Group__c='" + uuid + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Gaming Group with UUID: " + uuid + " has been found!");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; i++)
                        {
                            GameGroup__c g1 = (GameGroup__c)records[i];
                            String id = g1.Id;
                            if (id != null)
                            {
                                Console.WriteLine("Gaming Group with following UUID" + uuid + " has the following SalesForce-ID: " + id);
                                return id;
                            }
                            else
                            {
                                Console.WriteLine("Gaming Group with following UUID" + uuid + " has no SalesForce-ID ..." + Environment.NewLine + "ERROR");
                                return "empty";
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
                    Console.WriteLine("No Gaming Group found.");
                    return "empty";

                }
                Console.WriteLine("Query succesfully executed." + Environment.NewLine + "No Gaming Group with UUID: " + uuid + " founded in SalesForce...");
                return "empty";

            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " +
                                           e.Message + "\n" + e.StackTrace);
                return "empty";

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


        private static Lead GetLead(SforceService _sForceRef, string uuid)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT  UUID__c, FirstName, LastName, Id, Email, Status, Timestamp__c, Version__c, IsActive__c, IsBanned__c, Company, Birthdate__c, btw__c, Phone, Gdpr__c FROM LEAD WHERE UUID__c='" + uuid + "'";
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
                                Console.WriteLine("Lead " + uuid + " found");
                                return l1;
                            }
                            else
                            {
                                Console.WriteLine("Lead " + uuid + " " + (i + 1) + ": " + "ERROR");
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
        private static EEvent__c GetEvent(SforceService _sForceRef, string uuid)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT  UUID__c, Name, Timestamp__c, Version__c, IsActive__c FROM EEvent__c WHERE UUID__c ='" + uuid + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " event records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; i++)
                        {
                            EEvent__c e1 = (EEvent__c)records[i];

                            if (e1 != null)
                            {
                                Console.WriteLine("Event " + uuid + " found");
                                return e1;
                            }
                            else
                            {
                                Console.WriteLine("Event " + uuid + " " + (i + 1) + ": " + "ERROR");
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
        private static GameGroup__c GetGamingGroup(SforceService _sForceRef, string uuid)
        {

            QueryResult qResult = null;
            try
            {
                String soqlQuery = "SELECT id, UUID_Group__c, Name, Timestamp__c, Version__c, IsActive__c,UUID_Game__C,GroupLeader_UUID__c,Banned__c FROM GameGroup__c WHERE UUID_Group__c ='" + uuid + "'";
                qResult = _sForceRef.query(soqlQuery);
                Boolean done = false;
                if (qResult.size > 0)
                {
                    Console.WriteLine("Logged-in user can see a total of "
                       + qResult.size + " Gaming Group(s) records.");
                    while (!done)
                    {
                        sObject[] records = qResult.records;
                        for (int i = 0; i < records.Length; i++)
                        {
                            GameGroup__c e1 = (GameGroup__c)records[i];

                            if (e1 != null)
                            {
                                Console.WriteLine("Gaming Group " + uuid + " found");
                                return e1;
                            }
                            else
                            {
                                Console.WriteLine("Gaming Group " + uuid + " " + (i + 1) + ": " + "ERROR");
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

        
        private static void updateRecordLead(SforceService _sForceRef,string messageType ,string idLead, string uuid, string firstName, string lastName, string email, Int32 timestampLead, double versionLead, bool isActive, bool isBanned, string gsm, DateTime gebDatum, string btwNr, bool gdpr)
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

            l1.Company = messageType;

            l1.birthdate__cSpecified = true;
            l1.birthdate__c = gebDatum;

            l1.btw__c = btwNr;
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
        private static void updateRecordEvent(SforceService _sForceRef, string messageType, string idEvent ,string nameEvent, string uuid, Int32 timestamp, Int32 version, bool isActive)
        {
            EEvent__c[] updates = new EEvent__c[1];




            EEvent__c event1 = new EEvent__c();
            event1.Id = idEvent;


            event1.UUID__c = uuid;
            event1.Name = nameEvent;

            event1.IsActive__cSpecified = true;
            event1.timestamp__cSpecified = true;
            event1.timestamp__c = timestamp;
            event1.Version__c = version;
            event1.IsActive__c = isActive;



            updates[0] = event1;


            try
            {
                SaveResult[] saveResults = _sForceRef.update(updates);
                foreach (SaveResult saveResult in saveResults)
                {
                    if (saveResult.success)
                    {
                        Console.WriteLine("Successfully updated Event ID: " +
                                  saveResult.id);
                    }
                    else
                    {
                        Error[] errors = saveResult.errors;
                        if (errors.Length > 0)
                        {
                            Console.WriteLine("Error: could not update " +
                                      "Event ID " + saveResult.id + "."
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
        private static void updateRecordGamingGroup(SforceService _sForceRef, string messageType, string idGameGroup ,string name, string uuidGroup, string uuidLeader, string uuidGame, Int32 timestamp, Int32 version, bool isActive, bool banned)
        {
            GameGroup__c[] updates = new GameGroup__c[1];





            GameGroup__c gameGroup = new GameGroup__c();
            gameGroup.Id = idGameGroup;

            gameGroup.Name = name;
            gameGroup.UUID_Group__c = uuidGroup;

            gameGroup.timestamp__cSpecified = true;
            gameGroup.timestamp__c = timestamp;

            gameGroup.Version__cSpecified = true;
            gameGroup.Version__c = version;

            gameGroup.UUID_Game__c = uuidGame;
            gameGroup.GroupLeader_UUID__c = uuidLeader;

            gameGroup.IsActive__cSpecified = true;
            gameGroup.Banned__cSpecified = true;

            gameGroup.IsActive__c = isActive;
            gameGroup.Banned__c = banned;




            updates[0] = gameGroup;


            try
            {
                SaveResult[] saveResults = _sForceRef.update(updates);
                foreach (SaveResult saveResult in saveResults)
                {
                    if (saveResult.success)
                    {
                        Console.WriteLine("Successfully updated Gaming Group ID: " +
                                  saveResult.id);
                    }
                    else
                    {
                        Error[] errors = saveResult.errors;
                        if (errors.Length > 0)
                        {
                            Console.WriteLine("Error: could not update " +
                                      "Gaming Group ID " + saveResult.id + "."
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
                        XmlNodeList sender = doc.GetElementsByTagName("sender");

                        //________________________EVENT_____________________________
                        XmlNodeList eventUUID = doc.GetElementsByTagName("UUID");
                        XmlNodeList eventName = doc.GetElementsByTagName("name");

                        //________________________LEAD______________________________
                        XmlNodeList uuidLead = doc.GetElementsByTagName("UUID");
                        XmlNodeList fname = doc.GetElementsByTagName("firstname");
                        XmlNodeList lname = doc.GetElementsByTagName("lastname");
                        XmlNodeList email = doc.GetElementsByTagName("email");
                        XmlNodeList gdpr = doc.GetElementsByTagName("GDPR");
                        XmlNodeList timestamp = doc.GetElementsByTagName("timestamp");
                        XmlNodeList version = doc.GetElementsByTagName("version");
                        XmlNodeList isactive = doc.GetElementsByTagName("isActive");
                        XmlNodeList banned = doc.GetElementsByTagName("banned");
                        ///Not Required:
                        XmlNodeList geboortedatum = doc.GetElementsByTagName("geboortedatum");
                        XmlNodeList gsm = doc.GetElementsByTagName("gsm");
                        XmlNodeList btw = doc.GetElementsByTagName("btw");
                        XmlNodeList extra = doc.GetElementsByTagName("extraField");

                        //________________________Session___________________________
                        XmlNodeList sessionName = doc.GetElementsByTagName("titel");
                        XmlNodeList sessionUUID = doc.GetElementsByTagName("UUID");
                        XmlNodeList UUIDofParentEvent = doc.GetElementsByTagName("event_UUID");
                        XmlNodeList lokaal = doc.GetElementsByTagName("lokaal");
                        XmlNodeList descr = doc.GetElementsByTagName("desc");
                        XmlNodeList start = doc.GetElementsByTagName("start");
                        XmlNodeList end = doc.GetElementsByTagName("end");




                        XmlNodeList uuidGroup = doc.GetElementsByTagName("groupUUID");
                        XmlNodeList uuidGame = doc.GetElementsByTagName("gameUUID");
                        XmlNodeList uuidLeader = doc.GetElementsByTagName("GroupLeaderUUID");
                        XmlNodeList gamegroupname = doc.GetElementsByTagName("groupName");


                        bool gdprbool = Convert.ToInt32(gdpr[0].InnerText) != 0;
                        bool isActivebool = Convert.ToInt32(isactive[0].InnerText) != 0;
                        bool bannedbool = Convert.ToInt32(banned[0].InnerText) != 0;



                        //string messageType = xmlList[0].InnerText.ToLower();
                        string messageType = xmlList[0].InnerText;





                        switch (messageType)
                        {
                            //CONTACT  
                            /* Gaan we die uiteindelijk nog gebruiken???
                            case "contact":




                                CreateContact(_sForceRef, "Sami", "Pete", email[0].InnerText, gsm[0].InnerText);
                                //CreateContact(_sForceRef, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, gsm[0].InnerText);

                                break;*/

                            case "GameGroup":

                                if (uuidGroup[0].InnerText == "" || uuidLeader[0].InnerText == "" || uuidGame[0].InnerText == "")
                                {
                                    Console.WriteLine("Not every required field is set..." + Environment.NewLine);
                                    break;
                                }
                                else
                                {

                                    string idRecGamingGroup = GetGamingGroupID(_sForceRef, uuidGroup[0].InnerText);


                                    //check of event al bestaat
                                    if (idRecGamingGroup == "empty")
                                    {
                                        Console.WriteLine("New data received from " + sender[0].InnerText + Environment.NewLine);
                                        CreateGamingGroup(_sForceRef, gamegroupname[0].InnerText, uuidGroup[0].InnerText, uuidLeader[0].InnerText, uuidGame[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), isActivebool, bannedbool);
                                        break;
                                    }
                                    else
                                    {
                                        //update als version hoger is en zelfde uuid  
                                        if (Convert.ToInt32(version[0].InnerText) > GetGamingGroup(_sForceRef, uuidGroup[0].InnerText).Version__c)
                                        {
                                            updateRecordGamingGroup(_sForceRef, messageType, idRecGamingGroup, gamegroupname[0].InnerText, uuidGroup[0].InnerText, uuidLeader[0].InnerText, uuidGame[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), isActivebool, bannedbool);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Version lower or same as the already saved event..." + Environment.NewLine);
                                            break;
                                        }


                                    }

                                }



                            case "Visitor":
                                //check of verplichte velden niet leeg zijn
                                if (uuidLead[0].InnerText =="" || fname[0].InnerText == "" || lname[0].InnerText == "" || email[0].InnerText =="" || timestamp[0].InnerText =="" || version[0].InnerText =="")
                                {
                                    Console.WriteLine("Not every required field is set..."+Environment.NewLine);
                                    break;
                                }
                                else
                                {
                                    string idRec = GetLeadID(_sForceRef, uuidLead[0].InnerText);

                                    //check of gekregen data niet al bestaat
                                    if (idRec == "empty")
                                    {
                                        Console.WriteLine("new data received from " + sender[0].InnerText + Environment.NewLine);
                                        CreateLead(_sForceRef, messageType, uuidLead[0].InnerText, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), isActivebool, bannedbool, gsm[0].InnerText, Convert.ToDateTime(geboortedatum[0].InnerText), btw[0].InnerText, gdprbool);
                                        break;
                                    }
                                    else
                                    {
                                        // als data al bestaat gaat de versions vergelijken, indien nieuwe versie update
                                        if (Convert.ToInt32(version[0].InnerText) > GetLead(_sForceRef, uuidLead[0].InnerText).Version__c)
                                        {
                                            updateRecordLead(_sForceRef, messageType, idRec, uuidLead[0].InnerText, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), isActivebool, bannedbool, gsm[0].InnerText, Convert.ToDateTime(geboortedatum[0].InnerText), btw[0].InnerText, gdprbool);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("version lower or same as the already saved lead..." + Environment.NewLine);
                                            break;
                                        }

                                    }



                                }


                            case "CreateEvent":

                                //check of verplichte velden niet leeg zijn

                                if (eventUUID[0].InnerText==""|| eventName[0].InnerText == "")
                                {
                                    Console.WriteLine("Not every required field is set..." + Environment.NewLine);
                                    break;
                                }
                                else {

                                    string idRecEvent = GetEventID(_sForceRef, eventUUID[0].InnerText);


                                    //check of event al bestaat
                                    if (idRecEvent == "empty")
                                    {
                                        Console.WriteLine("New data received from " + sender[0].InnerText + Environment.NewLine);
                                        CreateEvent(_sForceRef, eventName[0].InnerText, eventUUID[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), isActivebool);
                                        break;
                                    }
                                    else
                                    {
                                        //update als version hoger is en zelfde uuid  
                                        if (Convert.ToInt32(version[0].InnerText) > GetEvent(_sForceRef, eventUUID[0].InnerText).Version__c)
                                        {
                                            updateRecordEvent(_sForceRef, messageType, idRecEvent, eventName[0].InnerText, eventUUID[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), isActivebool);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Version lower or same as the already saved event..." + Environment.NewLine);
                                            break;
                                        }


                                    }

                                }

                                

                            case "Session":

                                //Si fields titel,uuid,lokaal,start,end -> vide -> error 
                                //check si session existe deja?(UUID)
                                //Si oui, update
                                //Si non, create


                                CreateSession(_sForceRef, sessionName[0].InnerText, UUIDofParentEvent[0].InnerText, sessionUUID[0].InnerText, descr[0].InnerText, lokaal[0].InnerText, Convert.ToDateTime(start[0].InnerText), Convert.ToDateTime(end[0].InnerText));

                                break;

                            case "delete":
                                XmlNodeList uuidDelete = doc.GetElementsByTagName("UUID");


                                string id = GetLeadID(_sForceRef, uuidDelete[0].InnerText);


                                if(id != "empty")
                                {
                                    Lead l = GetLead(_sForceRef, uuidDelete[0].InnerText);
                                    if ((l.IsActive__c != true)&& (l.gdpr__c != true))
                                    {
                                        Delete(_sForceRef, id);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Unable to delete (Still Active lead / gdpr = false)");
                                        break;
                                    }

                                }
                                Console.WriteLine("Lead not found");
                                break;





                            case "ConvertLead_Contact":
                                
                                Lead lead = GetLead(_sForceRef,"XXX");
                                convertLeadToContact(_sForceRef, lead);


                                break;

                            case "update":

                                //string idTeUpdatenRec = GetLeadID(_sForceRef, "John Wick", "wiki@gmail.com");
                                //updateRecordLead(_sForceRef, idTeUpdatenRec, "DE3ACAAA", "John", "Weak", "jonny@wiki.com", 566465465, 3, false, false, "0000003", new DateTime(1960, 01, 04), "000005934", false);



                                //Inside logic of VISITOR
                                //string idTeUpdatenRec = GetLeadID(_sForceRef, uuid[0].InnerText );
                                //updateRecordLead(_sForceRef, messageType ,idTeUpdatenRec, uuid[0].InnerText, fname[0].InnerText, lname[0].InnerText, email[0].InnerText, Convert.ToInt32(timestamp[0].InnerText), Convert.ToInt32(version[0].InnerText), Convert.ToBoolean(isactive[0].InnerText), Convert.ToBoolean(banned[0].InnerText), gsm[0].InnerText, Convert.ToDateTime(geboortedatum[0].InnerText), btw[0].InnerText, Convert.ToBoolean(gdpr[0].InnerText));

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
