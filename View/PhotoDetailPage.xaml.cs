namespace PICT.View
{
    public partial class PhotoDetailPage : ContentPage
    {
        public PhotoDetailPage(string imageUrl, string title)
        {
            InitializeComponent();
            photoImage.Source = imageUrl;
            photoTitle.Text = title;
        }
    }
}
