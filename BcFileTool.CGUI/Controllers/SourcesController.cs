using BcFileTool.CGUI.Exceptions;
using BcFileTool.CGUI.Interfaces;
using BcFileTool.CGUI.Models;
using BcFileTool.CGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcFileTool.CGUI.Controllers
{
    public class SourcesController : BaseController<SourcesView>
    {
        SourcesModel _model;

        public SourcesController(SourcesModel model)
        {
            _model = model;
        }

        public void Add(Source source)
        {
            HandleExceptions(() =>
            {
                _model.Add(source);

                OnChange();
            });
        }

        internal void Remove(int selectedItem)
        {
            _model.RemoveAt(selectedItem);

            OnChange();
        }

        public override IValidationResult ValidateModel()
        {
            var result = new ValidationResult();

            if(_model.Sources.All(x => !x.Selected))
            {
                result.AddIssue("No source directory has been selected");
            }

            return result;
        }
    }
}
