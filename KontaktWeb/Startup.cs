using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Ninject;
using KontaktGame.Services.Contracts;
using KontaktGame.Services;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using KontaktWeb.Hubs;
using KontaktGame.Database.Contracts;
using KontaktGame.Database.Repositories;
using KontaktGame.Database;

[assembly: OwinStartup(typeof(KontaktWeb.Startup))]

namespace KontaktWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectSignalRDependencyResolver(kernel);

            kernel.Bind<IPlayerService>()
                .To<PlayerService>()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MyHub>().Clients
                        ).WhenInjectedInto<IPlayerService>();

            kernel.Bind<IPlayerRepository>()
                .To<PlayerRepository>()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MyHub>().Clients
                        ).WhenInjectedInto<IPlayerRepository>();

            kernel.Bind<IQuestionRepository>()
                .To<QuestionRepository>()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MyHub>().Clients
                        ).WhenInjectedInto<IQuestionRepository>();

            kernel.Bind<IUsedWordRepository>()
                .To<UsedWordRepository>()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MyHub>().Clients
                        ).WhenInjectedInto<IUsedWordRepository>();

            kernel.Bind<IWordToGuessRepository>()
                .To<WordToGuessRepository>()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MyHub>().Clients
                        ).WhenInjectedInto<IWordToGuessRepository>();

            kernel.Bind<IDatabase>()
                .To<Database>()
                .InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MyHub>().Clients
                        ).WhenInjectedInto<IDatabase>();

            var config = new HubConfiguration();
            config.Resolver = resolver;
            Startup.ConfigureSignalR(app, config);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

        }
        private static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            app.MapSignalR(config);
        }
        internal class NinjectSignalRDependencyResolver : DefaultDependencyResolver
        {
            private readonly IKernel _kernel;
            public NinjectSignalRDependencyResolver(IKernel kernel)
            {
                _kernel = kernel;
            }

            public override object GetService(Type serviceType)
            {
                return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
            }

            public override IEnumerable<object> GetServices(Type serviceType)
            {
                return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
            }
        }
    }

}
