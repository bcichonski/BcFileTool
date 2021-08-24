using BcFileTool.CGUI.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Terminal.Gui;

namespace BcFileTool.CGUI.Utils
{
    public class SelectableListWrapper<T> : IListDataSource where T : IAmSelectable
    {
        private IList<T> _source;
        private ListWrapper _underlyingListWrapper;

        public int Count => _underlyingListWrapper.Count;

        public int Length => _underlyingListWrapper.Length;

        public event Action<int> Marked;

        public SelectableListWrapper(IList<T> source)
        {
            _source = source;
            _underlyingListWrapper = new ListWrapper((IList)_source);
        }

        public void SetMark(int item, bool value)
        {
            _underlyingListWrapper.SetMark(item, value);

            _source[item].Selected = value;

            Marked?.Invoke(item);
        }

        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width, int start = 0)
        {
            _underlyingListWrapper.Render(container, driver, selected, item, col, line, width, start);
        }

        public bool IsMarked(int item)
        {
            return _underlyingListWrapper.IsMarked(item);
        }

        public IList ToList()
        {
            return _underlyingListWrapper.ToList();
        }
    }
}
