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
            _model.Action = fileAction;
        }

        internal void OnDateDirectioriesToggled(bool obj)
        {
            _model.DateDirectories = obj;
        }

        internal void OnPreserveSubdirectioriesToggled(bool obj)
        {
            _model.PreserveSubdirectories = obj;
        }

        internal void OnVerifyChecksumToggled(bool obj)
        {
            _model.VerifyChecksum = obj;
        }

        internal void OnSkipToggled(bool obj)
        {
            _model.Skip = obj;
        }

        internal void OnStart()
        {
            
        }
    }
}
