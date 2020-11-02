using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interactivity;

namespace BehaviorsLab.Behaviors
{
    class ListDragBlendBehavior : Behavior<ListBox>
    {
        private ListBox _dragSource;
        private object _dragData;
        private Point _dragStart;

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += AttachDraggedData;
            
            AssociatedObject.PreviewMouseMove += InitiateDrapDropOperation;
            
            AssociatedObject.PreviewMouseLeftButtonUp += SetDragDataToNull;
        }

        private void AttachDraggedData(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _dragStart = mouseButtonEventArgs.GetPosition(null);
            
            _dragSource = sender as ListBox;
            
            if (_dragSource == null) return;
            
            var i = GetIndexOfTheSelectedItem(AssociatedObject);
            
            _dragData = i != -1 ? _dragSource.Items.GetItemAt(i) : null;
        }

        void InitiateDrapDropOperation(object sender, MouseEventArgs e)
        {
            if (_dragData == null) return;

            var currentPosition = e.GetPosition(null);
            var difference = _dragStart - currentPosition;

            if ((MouseButtonState.Pressed == e.LeftButton) &&
                ((Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                 (Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance)))
            {
                var data = new DataObject("Custom", _dragData);
                DragDrop.DoDragDrop(_dragSource, data, DragDropEffects.Copy);

                _dragData = null;
            }
        }

        private void SetDragDataToNull(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _dragData = null;
        }

        private int GetIndexOfTheSelectedItem(ListBox l)
        {
            var index = -1;
            for (var i = 0; i < l.Items.Count; ++i)
            {
                var item = l.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;

                if (item != null && item.IsMouseOver)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
