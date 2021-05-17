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

        public void LoadSources()
        {
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));
            _model.Add(new Source(@"Z:\katalog"));

            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto                                  \Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto                       \Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
            _model.Add(new Source(@"C:\Users\barto\Pictures\Saved Pictures"));
        }

        internal void Remove(int selectedItem)
        {
            _model.RemoveAt(selectedItem);

            OnChange();
        }
    }
}
