using Microsoft.Maui.Controls;

namespace Minigame
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ArtLabel.Text = LocationArt.Cow;
            UpdateGatheringStatus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GameResources.Changed += UpdateGatheringStatus;
            UpdateGatheringStatus();
        }

        protected override void OnDisappearing()
        {
            GameResources.Changed -= UpdateGatheringStatus;
            base.OnDisappearing();
        }

        private async void OnGatherClicked(object sender, EventArgs e)
        {
            await GameResources.GatherAsync(ResourceType.Milk);
        }

        private void UpdateGatheringStatus()
        {
            var isGatheringHere = GameResources.CurrentResource == ResourceType.Milk;
            GatherButton.IsEnabled = !GameResources.IsGathering;
            GatherButton.Text = GameResources.IsGathering
                ? isGatheringHere ? "Збирання триває..." : "Збір уже триває"
                : "Зібрати 5 ресурсів";

            var count = GameResources.GetCount(ResourceType.Milk);
            StatusLabel.Text = isGatheringHere
                ? $"Збирання триватиме 5 секунд. Усього молока: {count}."
                : $"Усього молока: {count}. За одне збирання: 5.";
        }
    }
}
