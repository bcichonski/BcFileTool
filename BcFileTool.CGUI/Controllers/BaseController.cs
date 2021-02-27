using BcFileTool.CGUI.Exceptions;
using BcFileTool.CGUI.Interfaces;
using System;

namespace BcFileTool.CGUI.Controllers
{
    public class BaseController<TView> where TView:IHandleExceptions
    {
        protected TView _view;
        
        public BaseController()
        {
        }

        public virtual void SetView(TView view)
        {
            _view = view;
        }

        protected void HandleExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                _view.ShowException(exception);
            }
        }
    }
}
