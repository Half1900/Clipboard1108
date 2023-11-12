using System.Windows.Media.Imaging;

namespace Clipboards.Class
{
    public class ClipboardItem
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public BitmapSource Image { get; set; }
        public ClipboardType Type { get; set; }
        public string TextVisible { get => Type == ClipboardType.Text ? "Visible" : "Hidden"; }
        public string ImageVisible { get => Type == ClipboardType.Image ? "Visible" : "Hidden"; }
        //public ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        //{
        //    this.Image.s
        //    IntPtr hBitmap = bitmap.GetHbitmap();
        //    ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        //        hBitmap,
        //        IntPtr.Zero,
        //        Int32Rect.Empty,
        //        BitmapSizeOptions.FromEmptyOptions());

        //    if (!DeleteObject(hBitmap))
        //    {
        //        throw new System.ComponentModel.Win32Exception();
        //    }
        //    return wpfBitmap;
        //}
        public int HashCode
        {
            get => string.IsNullOrEmpty(Text) ? Image.GetHashCode() : Text.GetHashCode();
        }
    }

    public enum ClipboardType
    {
        Text = 0,
        Image = 1,
    }
}
