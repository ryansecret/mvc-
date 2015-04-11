using Autofac;
using Hsr.Core.Infrastructure;
using Hsr.Core.Infrastructure.DependencyManagement;
using Hsr.Model.CustomValidators;

namespace Hsr.Service
{
    public class ValidatorRegister:IDependencyRegister
    {
        public int Order { get; private set; }
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<EqualValidator>().As<IValidator>().Named<IValidator>("EqualValidator").SingleInstance();
            
        }
    }
}
