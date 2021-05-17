using BcFileTool.CGUI.Exceptions;
using BcFileTool.CGUI.Interfaces;
using System;

namespace BcFileTool.CGUI.Controllers
{
    public class BaseController<TView> : IReactOnChange where TView : IHandleExceptions
    {
        protected TView _view;
        protected IReactOnChange _onChange;

        public BaseController()
        {
        }

        public virtual void SetView(TView view)
        {
            _view = view;
        }

        public virtual void SetOnChange(IReactOnChange onChange)
        {
            _onChange = onChange;
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

        public virtual void OnChange()
        {
            _onChange?.OnChange();
        }
    }
}
