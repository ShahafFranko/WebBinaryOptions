using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using BinaryOption.DAL;
using BinaryOption.OptionServer.Contract;
using BinaryOption.OptionServer.Contract.Events;
using BinaryOptions.OptionServer.Handlers;
using BinaryOptions.OptionServer.Services;

namespace BinaryOptions.OptionServer
{
    /// <summary>
    /// This Binary Options application Based on two major components:
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
        /// We are schedualing all our event that happen from time to time.
        /// </summary>
        /// <param name="actorSystem"></param>
        private static void ScheduleSystemEvents(ActorSystem actorSystem)
        {
            var expireService = actorSystem.ActorOf(actorSystem.DI().Props<ExpireOptionEventHandler>());
            var ratesService = actorSystem.ActorOf(actorSystem.DI().Props<RatesService>());
            actorSystem.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(1), expireService, new OneMinuteElapsed(), expireService);
            actorSystem.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10), ratesService, new TenSecondsElapsed(), ratesService);
        }
    }
}
