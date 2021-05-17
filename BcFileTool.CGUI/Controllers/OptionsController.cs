using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Views;
using BcFileTool.Library.Enums;
using System;

namespace BcFileTool.CGUI.Controllers
{
    public class OptionsController : BaseController<OptionsView>
    {
        OptionsModel _model;

        public OptionsController(OptionsModel model)
        {
            _model = model;
        }

        internal void OnActionChanged(FileAction fileAction)
        {
            SetValue(_model.Action, fileAction, x => _model.Action = x);
        }

        internal void OnDateDirectioriesToggled(bool obj)
        {
            SetValue(_model.DateDirectories, obj, x => _model.DateDirectories = obj);
        }

        internal void OnPreserveSubdirectioriesToggled(bool obj)
        {
            SetValue(_model.PreserveSubdirectories, obj, x => _model.PreserveSubdirectories = obj);
        }

        internal void OnVerifyChecksumToggled(bool obj)
        {
            SetValue(_model.VerifyChecksum, obj, x => _model.VerifyChecksum = obj);
        }

        internal void OnSkipToggled(bool obj)
        {
            SetValue(_model.Skip, obj, x => _model.Skip = obj);
        }

        private void SetValue<T>(T oldValue, T newValue, Action<T> setter)
        {
            if(!oldValue.Equals(newValue))
            {
                setter(newValue);

                OnChange();
            }
        }

        internal void OnStart()
        {
            
        }
    }
}
