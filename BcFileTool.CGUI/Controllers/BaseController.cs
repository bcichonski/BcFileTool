using BcFileTool.CGUI.Exceptions;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using System;

namespace BcFileTool.CGUI.Controllers
{
    public class BaseController<TView> : IReactOnChange, IValidateModel where TView : IHandleExceptions
    {
        protected TView _view;
        protected IReactOnChange _onChange;
        protected IValidateModel _onValidate;

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

        public virtual void SetValidationParent(IValidateModel validator)
        {
            _onValidate = validator;
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

        public virtual IValidationResult ValidateModel()
        {
            return new ValidationResult();
        }

        public virtual IValidationResult Validate()
        {
            return _onValidate?.Validate();
        }
    }
}
