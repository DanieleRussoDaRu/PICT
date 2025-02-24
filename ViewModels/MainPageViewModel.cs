using PICT.Entity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    private const string APIKEY = "255ac8fdac4726aa339fa1c2161b9e5b";

    public MainPageViewModel()
    {
        _httpClient = new HttpClient();

        SearchCommand = new Command(async () => await SearchPhotosAsync());
    }

    private ObservableCollection<Photo> _photos = new ObservableCollection<Photo>();
    public ObservableCollection<Photo> Photos
    {
        get => _photos;
        set
        {
            _photos = value;
            OnPropertyChanged();
        }
    }

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            OnPropertyChanged();
        }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public Command SearchCommand { get; }

    public async Task SearchPhotosAsync()
    {
        if (IsBusy || string.IsNullOrEmpty(SearchQuery)) return;

        IsBusy = true;

        try
        {
            
            var url = $"https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key={APIKEY}&text={SearchQuery}&format=json&nojsoncallback=1";

            var response = await _httpClient.GetStringAsync(url);

            FlickrResponse flickrResponse = JsonSerializer.Deserialize<FlickrResponse>(response);

            Photos.Clear();

            foreach (var photo in flickrResponse.photos.photo)
            {
                var imageUrl = $"https://live.staticflickr.com/{photo.farm}/{photo.server}/{photo.id}_{photo.secret}.jpg";
                Photos.Add(new Photo
                {
                    Title = photo.title,
                    ImageUrl = imageUrl
                });
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Errore", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
