using System;
using Serilog;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace AtlasQuantumAPI.Infrastructure
{
    class SeriLogContextualLoggerInjectionBrehavior : IDependencyInjectionBehavior
    {
        private readonly IDependencyInjectionBehavior original;
        private readonly Container container;

        public SeriLogContextualLoggerInjectionBrehavior(ContainerOptions options)
        {
            original = options.DependencyInjectionBehavior;
            container = options.Container;
        }

        public void Verify(InjectionConsumerInfo consumer)
        {
            original.Verify(consumer);
        }

        public InstanceProducer GetInstanceProducer(InjectionConsumerInfo info, bool throwOnFailure)
        {
            return info.Target.TargetType == typeof(ILogger)
                ? GetLoggerInstanceProducer(info.ImplementationType)
                : original.GetInstanceProducer(info, throwOnFailure);
        }

        private InstanceProducer<ILogger> GetLoggerInstanceProducer(Type type)
        {
            return Lifestyle.Singleton.CreateProducer(() => Log.ForContext(type), container);
        }

    }
}
