#region

using Autofac;

#endregion



namespace Hsr.Core.Infrastructure.DependencyManagement
{
    public interface IDependencyRegister
    {
        int Order { get; }
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);
    }
}