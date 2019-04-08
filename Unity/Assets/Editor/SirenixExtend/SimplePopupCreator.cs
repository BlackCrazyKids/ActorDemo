namespace Sirenix.OdinInspector.Editor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class SimplePopupCreator
    {
        public static void ShowDialog<T>(List<T> list, Action<T> onSelected = null)
        {
            var selector = new SimpleSelector<T>(list, onSelected);

            if (selector.SelectionTree.EnumerateTree().Count() == 1)
            {
                selector.SelectionTree.EnumerateTree().First().Select();
                selector.SelectionTree.Selection.ConfirmSelection();
            }
            else
            {
                selector.ShowInPopup(200);
            }
        }

        private class SimpleSelector<T> : OdinSelector<T>
        {
            private Action<T> _onSelected;
            private List<T> _list;

            public SimpleSelector(List<T> list, Action<T> onSelected = null)
            {
                this._onSelected = onSelected;
                this._list = list;
                this.SelectionConfirmed += this.Confirmed;
            }

            private void Confirmed(IEnumerable<T> selection)
            {
                if (_onSelected != null)
                {
                    _onSelected(selection.FirstOrDefault());
                }
            }

            protected override void BuildSelectionTree(OdinMenuTree tree)
            {
                tree.AddRange(_list, x => Convert.ToString(x));
            }
        }
    }
}
