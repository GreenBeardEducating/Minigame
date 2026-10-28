using Microsoft.Maui.Controls;

namespace Minigame
{
    public partial class HillPage : ContentPage
    {
        public HillPage()
        {
            InitializeComponent();
            ArtLabel.Text = LocationArt.Mountains;
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
            await GameResources.GatherAsync(ResourceType.Stone);
        }

        private void UpdateGatheringStatus()
        {
            var isGatheringHere = GameResources.CurrentResource == ResourceType.Stone;
            GatherButton.IsEnabled = !GameResources.IsGathering;
            GatherButton.Text = GameResources.IsGathering
                ? isGatheringHere ? "Збирання триває..." : "Збір уже триває"
                : "Зібрати 5 ресурсів";

            var count = GameResources.GetCount(ResourceType.Stone);
            StatusLabel.Text = isGatheringHere
                ? $"Збирання триватиме 5 секунд. Усього каменю: {count}."
                : $"Усього каменю: {count}. За одне збирання: 5.";
        }
    }
}
