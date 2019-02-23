[assembly: WebActivator.PreApplicationStartMethod(typeof(MyExams.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MyExams.App_Start.NinjectWebCommon), "Stop")]

namespace MyExams.App_Start
{
    using System;
    using System.Web;
    using KontaktGame.Database;
    using KontaktGame.Database.Contracts;
    using KontaktGame.Database.Repositories;
    using KontaktGame.Services;
    using KontaktGame.Services.Contracts;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel
                .Bind<IDatabase>()
                .To<Database>()
            .InRequestScope();
            kernel
                .Bind<IPlayerRepository>()
                .To<PlayerRepository>()
            .InRequestScope();
            kernel
                .Bind<IQuestionRepository>()
                .To<QuestionRepository>()
            .InRequestScope();
            kernel
                .Bind<IUsedWordRepository>()
                .To<UsedWordRepository>()
            .InRequestScope();
            kernel
                .Bind<IWordToGuessRepository>()
                .To<WordToGuessRepository>()
            .InRequestScope();
            kernel
               .Bind<IPlayerService>()
               .To<PlayerService>()
           .InRequestScope();
            kernel
              .Bind<IQuestionService>()
              .To<QuestionService>()
          .InRequestScope();
            kernel
              .Bind<IUsedWordService>()
              .To<UsedWordService>()
          .InRequestScope();
            kernel
              .Bind<IWordToGuessService>()
              .To<WordToGuessService>()
          .InRequestScope();
        }
    }
}