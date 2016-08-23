using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Akka.Actor;
using Akka.Configuration;
using Akka.Remote;
using BinaryOptions.WebServer.Actors;

namespace BinaryOptions.WebServer
{
    public class Global : HttpApplication
    {
        public static ActorSystem m_uiActorSystem;
        public static IActorRef m_eventsListener;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var config = ConfigurationFactory.ParseString(@"
                akka {  
                    actor {
                        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                    }
                    remote {
                        helios.tcp {
                            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
		                    applied-adapters = []
		                    transport-protocol = tcp
		                    port = 6670
		                    hostname = localhost
                        }
                    }
                }");

            m_uiActorSystem = ActorSystem.Create("BinaryOptionsWebserver", config);
            m_eventsListener = m_uiActorSystem.ActorOf<EventsListener>("EventsListener");
        }
    }
}