using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.RegistrationByConvention;

namespace Guanwu.NotifyConsoleApp
{
    internal sealed class WidgetContainer
    {
        private static readonly Lazy<WidgetContainer> lazy =
            new Lazy<WidgetContainer>(() => new WidgetContainer());
        public static WidgetContainer Instance { get { return lazy.Value; } }

        private UnityContainer Container;

        private WidgetContainer()
        {
            Container = new UnityContainer();
        }

        public ICollection<T> ResolveWidgets<T>()
        {
            RegisterWidgetContainer();

            var widgetTypes = Container.Registrations
                .Where(t => typeof(T).IsAssignableFrom(t.RegisteredType))
                .Where(t => t.MappedToType.IsClass)
                .Select(t => t.MappedToType)
                .Distinct();

            var widgets = widgetTypes
                .Select(t => (T)Container.Resolve(t.UnderlyingSystemType))
                .ToArray();

            return widgets;
        }

        private void RegisterWidgetContainer()
        {
            Container.RegisterTypes(
                WidgetHelper.FromWidgetTypes(),
                WithMappings.FromAllInterfaces,
                (Type t) => { return t.AssemblyQualifiedName; },
                WithLifetime.ContainerControlled);
        }
    }
}
