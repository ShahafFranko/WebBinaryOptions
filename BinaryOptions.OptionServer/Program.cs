using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.Events;
using BinaryOptions.DAL;
using BinaryOptions.OptionServer.Handlers;
using BinaryOptions.OptionServer.Services;

namespace BinaryOptions.OptionServer
{
    /// <summary>
    /// This Binary Options server based on two major components:
    /// Akka.NET - a .net port of java Akka (we used it for distribution, scalability and concurrency)
    /// Autofac - as a Dependency Injection.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
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
                            port = 6671
                            hostname = localhost
                        }
                    }
                }");

            ActorSystem actorSystem = ActorSystem.Create("OptionServer", config);

            var builder = new ContainerBuilder();

            builder.RegisterInstance(actorSystem).SingleInstance();
            Protocol protocol = new Protocol("OptionServer", "BinaryOptionsWebserver", 6670);
            builder.RegisterInstance(protocol).SingleInstance();

            RegisterActors(builder);
            RegisterDAL(builder);

            IContainer container = builder.Build();
            AutoFacDependencyResolver resolver = new AutoFacDependencyResolver(container, actorSystem);

            WarmUp(actorSystem);
            ScheduleSystemEvents(actorSystem);

            Console.ReadKey();
        }

        /// <summary>
        /// Register all Actors in the system.
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterActors(ContainerBuilder builder)
        {
            builder.RegisterType<ExpireOptionEventHandler>().SingleInstance();
            builder.RegisterType<LoginRequestHandler>().SingleInstance();
            builder.RegisterType<OpenPositionCommandHandler>().SingleInstance();
            builder.RegisterType<AccountsHandler>().SingleInstance();
            builder.RegisterType<RatesService>().SingleInstance();
        }

        /// <summary>
        /// Register all DAL components.
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterDAL(ContainerBuilder builder)
        {
            builder.RegisterType<DBContext>().SingleInstance();
            builder.RegisterType<AccountsRepository>().SingleInstance();
            builder.RegisterType<PositionsRepository>().SingleInstance();
        }

        /// <summary>
        /// We are schedualing all our event that happen in the system.
        /// </summary>
        /// <param name="actorSystem"></param>
        private static void ScheduleSystemEvents(ActorSystem actorSystem)
        {
            var expireService = actorSystem.ActorOf(actorSystem.DI().Props<ExpireOptionEventHandler>());
            var ratesService = actorSystem.ActorOf(actorSystem.DI().Props<RatesService>(), "RatesService");

            // we want that the execution will occure every minute, so let's schedule it correctly.
            int from = 60 - DateTime.Now.Second;
            actorSystem.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(from), TimeSpan.FromMinutes(1), expireService, new OneMinuteElapsed(), expireService);
            actorSystem.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1), ratesService, new OneSecondElapsed(), ratesService);
        }

        /// <summary>
        // Afer we wiring our actors using autofac, let's instanciate them
        /// We are doing it now, because basically actor is a thread, which is a bit costly to create
        /// So we create them during load.
        /// Since they are actors, the Garbage collector will not collect them.
        /// </summary>
        public static void WarmUp(ActorSystem actorSystem)
        {
            var accountsHandler = actorSystem.ActorOf(actorSystem.DI().Props<AccountsHandler>(), "AccountsHandler");
            var positionsHandler = actorSystem.ActorOf(actorSystem.DI().Props<OpenPositionCommandHandler>(), "OpenPositionCommandHandler");
            var loginHandler = actorSystem.ActorOf(actorSystem.DI().Props<LoginRequestHandler>(), "LoginRequestHandler");
            var ExpirePositionsHandler = actorSystem.ActorOf(actorSystem.DI().Props<ExpireOptionEventHandler>(), "ExpireOptionEventHandler");
        }
    }
}
