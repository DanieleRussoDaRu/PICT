using PICT;
using PICT.Entity;
using PICT.View;

namespace PICT
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private async void OnCellTapped(object sender, ItemTappedEventArgs e)
        {
            ListView view = sender as ListView;

            if(view != null)
            {
                if (e.Item != null)
                {
                    var selectedPhoto = e.Item as Photo;
                    await Navigation.PushAsync(new PhotoDetailPage(selectedPhoto.ImageUrl, selectedPhoto.Title));
                }
                
            }
            
        }

    }

}
