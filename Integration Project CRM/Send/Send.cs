﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using System.Diagnostics;


namespace Send
{
    class Send
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "10.3.56.27" };
            factory.UserName = "manager";
            factory.Password = "ehb";
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");
                String xmlStr;
                byte[] body;
                xmlStr = "<message><header><messageType>Visitorrr</messageType><description>Creation of a visitor</description><sender>front-end</sender><!-- crm --></header><datastructure><UUID>Flavi34</UUID><name><firstname>Mosatooo</firstname><lastname>Posatoooo</lastname></name><email>blAPosat@saAt.be</email><GDPR>false</GDPR><timestamp>1522113440</timestamp><version>3</version><isActive>false</isActive><banned>false</banned><!-- Not required fields --><geboortedatum>1997-08-12</geboortedatum><btw>BE15656464654</btw><gsm>01164165468</gsm><extraField></extraField></datastructure></message>";

                body = Encoding.UTF8.GetBytes(xmlStr);
                channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);


                Console.WriteLine(" [x] Sent {0}", xmlStr);
                // Console.ReadKey();



                //channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                //xmlStr = "<Message> <header> <!-- type of message --> <MessageType>Visitor</MessageType> <!--What your Message does --> <description>Update of a visitor</description> <!--Who sent it--> <!--(fronted, crm, facturatie, kassa, monitor, planning, uuid) --> <sender>front-end</sender> <!-- kassa, crm, front-end --> </header> <datastructure> <!-- required fields = UUID name + email & hashing. --> <UUID>200</UUID> <!-- id of the user --> <name> <firstname>Anthe234</firstname> <lastname>Boets</lastname> </name> <!-- kassa , front-end --> <email>anthe.boets@student.ehb.be</email> <GDPR>true</GDPR> <timestamp>1999-04-30T00:00:00+00:00</timestamp> <version>1</version> <isActive>true</isActive> <banned>false</banned> <!-- Not required fields --> <geboortedatum>1999-04-30T00:00:00+00:00</geboortedatum> <btw-nummer/> <gsm-nummer/> <extraField/> </datastructure> </Message>";

                //body = Encoding.UTF8.GetBytes(xmlStr);
                //channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);


                //Console.ReadKey();
                //channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                //xmlStr = "<Message> <header> <!-- type of message --> <MessageType>Visitor</MessageType> <!--What your Message does --> <description>Deletion of a visitor</description> <!--Who sent it--> <!--(fronted, crm, facturatie, kassa, monitor, planning, uuid) --> <sender>front-end</sender> <!-- kassa, crm, front-end --> </header> <datastructure> <!-- required fields = UUID name + email & hashing. --> <UUID>200</UUID> <!-- id of the user --> <name> <firstname>Anthe234</firstname> <lastname>Boets</lastname> </name> <!-- kassa , front-end --> <email>anthe.boets@student.ehb.be</email> <GDPR>true</GDPR> <timestamp>1999-04-30T00:00:00+00:00</timestamp> <version>1</version> <isActive>true</isActive> <banned>false</banned> <!-- Not required fields --> <geboortedatum>1999-04-30T00:00:00+00:00</geboortedatum> <btw-nummer/> <gsm-nummer/> <extraField/> </datastructure> </Message>";

                //body = Encoding.UTF8.GetBytes(xmlStr);
                //channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);

            }

        }

    }
}
