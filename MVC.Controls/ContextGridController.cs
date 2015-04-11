using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls
{
    public abstract class ContextGridController<TContextType, TEntityType> :
       GridController<TEntityType>
        where TEntityType : class
        where TContextType : class, IDisposable
    {
        protected ContextGridController()
        {
            DataContext = CreateContext();
        }

        protected TContextType DataContext { get; private set; }
        protected abstract TContextType CreateContext();
        protected override void Dispose(bool disposing)
        {
            DataContext.Dispose();
            base.Dispose(disposing);
        }
    }
}
