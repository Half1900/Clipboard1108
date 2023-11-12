using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Clipboards.Class;

namespace Clipboards.Styles
{
    public class ListBoxEx : ListBox
    {
        public event Action UnHook;
        public event Action Hook;


        protected override DependencyObject GetContainerForItemOverride()
        {
            var listbox = new ListBoxItemEx();
            listbox.UnHook += () => UnHook.Invoke();
            listbox.Hook += () => Hook?.Invoke();
            return listbox;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Reset)
                MainVM.DataTemplateWithCtrl.Clear();
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                MainVM.DataTemplateWithCtrl.Remove(e.OldItems[0] as ClipboardItem);
                int i = 1;
                foreach (KeyValuePair<ClipboardItem, ListBoxItemEx> item in MainVM.DataTemplateWithCtrl.OrderBy(kv => kv.Key.Index))
                {
                    item.Key.Index = i;
                    item.Value.TxbIndex.Text = $"{i}";
                    i++;
                }
            }
        }
    }

    public class ListBoxItemEx : ListBoxItem
    {
        public event Action UnHook;
        public event Action Hook;
        protected override void OnSelected(RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            ListBoxItem item = (ListBoxItem)dep;
            if (item.IsSelected)
            {
                item.IsSelected = !item.IsSelected;
                //e.Handled = true;
            }
            base.OnSelected(e);
        }

        public TextBlock TxbIndex;
        private TextBlock textbox;
        public Button button;
        private Image image;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textbox = Template.FindName("tb", this) as TextBlock;
            textbox.MouseLeftButtonDown -= Textbox_MouseLeftButtonDown;
            textbox.MouseLeftButtonDown += Textbox_MouseLeftButtonDown;
            button = Template.FindName("closeBtn", this) as Button;
            button.Click -= Button_Click;
            button.Click += Button_Click;
            image = Template.FindName("img", this) as Image;
            TxbIndex = Template.FindName("lblIndex", this) as TextBlock;

            image.MouseLeftButtonDown -= Textbox_MouseLeftButtonDown;
            image.MouseLeftButtonDown += Textbox_MouseLeftButtonDown;

            var item = MainVM.Instance.ClipboardsItems.FirstOrDefault(c => c.HashCode == GetSourceHashCode());
            if (!MainVM.DataTemplateWithCtrl.ContainsKey(item))
                MainVM.DataTemplateWithCtrl.Add(item, this);
            MainVM.DataTemplateWithCtrl[item] = this;
        }

        private int GetSourceHashCode()
        {
            int hashCode;
            if (string.IsNullOrEmpty(textbox.Text))
            {
                hashCode = image.Source.GetHashCode();
            }
            else
            {
                hashCode = textbox.Text.GetHashCode();
            }
            return hashCode;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int hashCode = GetSourceHashCode();
            if (string.IsNullOrEmpty(textbox.Text))
            {
                var item = MainVM.Instance.ClipboardsItems.FirstOrDefault(c => c.HashCode == hashCode);
                MainVM.Instance.ClipboardsItems.Remove(item);
            }
            else
            {
                var item = MainVM.Instance.ClipboardsItems.FirstOrDefault(c => c.HashCode == hashCode);
                MainVM.Instance.ClipboardsItems.Remove(item);
            }
        }

        private void Textbox_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //HotKeyHelper.IsIgnorance = true;
                UnHook?.Invoke();
                if (!string.IsNullOrEmpty(textbox.Text))
                {
                    Clipboard.SetText(textbox.Text);
                }
                else
                {
                    Clipboard.SetImage((BitmapSource)image.Source);
                }
                Hook?.Invoke();
            }
            catch(Exception ex)
            {
                LogHelper.Instance._logger.Error(ex);
            }
        }
    }
}
