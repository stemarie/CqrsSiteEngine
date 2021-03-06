﻿using AzureReadModel;
using CommandExecutors;
using CommandServiceLibrary.Properties;
using Ncqrs.Commanding.ServiceModel;
using Ncqrs.Eventing.ServiceModel.Bus;
using Ncqrs.Eventing.Storage;
using ReadModel.Denormalizers;

namespace CommandServiceLibrary
{
    public static class Bootstrapper
    {
        public static void BootUp()
        {
            Ncqrs.NcqrsEnvironment.SetDefault<ICommandService>(InitializeCommandService());
            Ncqrs.NcqrsEnvironment.SetDefault<IEventStore>(InitializeEventStore());
            Ncqrs.NcqrsEnvironment.SetDefault<ReadModel.IReadModelStore>(InitializeReadModelStore());
            Ncqrs.NcqrsEnvironment.SetDefault<IEventBus>(InitializeEventBus());
        }

        private static ICommandService InitializeCommandService()
        {
            var service = new Ncqrs.Commanding.ServiceModel.CommandService();
            service.RegisterExecutor(new PostNewTweetCommandExecutor());
            service.RegisterExecutor(new DeleteTweetCommandExecutor());
            service.RegisterExecutor(new CreateChannelCommandExecutor());
            service.RegisterExecutor(new CreateUserCommandExecutor());
            service.RegisterExecutor(new DeleteUserCommandExecutor());
            service.RegisterExecutor(new SetUserPropertyCommandExecutor());
            service.RegisterExecutor(new SetUserPasswordCommandExecutor());

            return service;
        }

        private static IEventStore InitializeEventStore()
        {
            //return new SimpleMicrosoftSqlServerEventStore("Data Source=.\\SQLEXPRESS;Integrated Security=SSPI;User Instance=True;AttachDbFilename=|DataDirectory|\\EventStore.mdf;");
            return new Eventing.AzureSystemEventStore();
        }

        private static ReadModel.IReadModelStore InitializeReadModelStore()
        {
            return new AzureReadModelStore(Settings.Default.AzureConnectionString, Settings.Default.AzureContainerName);
        }

        private static IEventBus InitializeEventBus()
        {
            var bus = new InProcessEventBus();
            //bus.RegisterHandler(new SqlTweetIndexItemDenormalizer());
            bus.RegisterHandler(new FileTweetIndexItemDenormalizer());
            bus.RegisterHandler(new FileTweetIndexItemDeleteDenormalizer());
            bus.RegisterHandler(new ChannelIndexItemDenormalizer());
            bus.RegisterHandler(new UserIndexDenormalizer());
            bus.RegisterHandler(new UserIndexDeleteDenormalizer());
            bus.RegisterHandler(new UserIndexPropertySetDenormalizer());
            bus.RegisterHandler(new UserIndexPasswordSetDenormalizer());

            return bus;
        }
    }
}