using Microsoft.Maui.Controls;

namespace Minigame
{
    public partial class ForestPage : ContentPage
    {
        public ForestPage()
        {
            InitializeComponent();
            ArtLabel.Text = LocationArt.Trees;
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
            await GameResources.GatherAsync(ResourceType.Wood);
        }

        private void UpdateGatheringStatus()
        {
            var isGatheringHere = GameResources.CurrentResource == ResourceType.Wood;
            GatherButton.IsEnabled = !GameResources.IsGathering;
            GatherButton.Text = GameResources.IsGathering
                ? isGatheringHere ? "Збирання триває..." : "Збір уже триває"
                : "Зібрати 5 ресурсів";

            var count = GameResources.GetCount(ResourceType.Wood);
            StatusLabel.Text = isGatheringHere
                ? $"Збирання триватиме 5 секунд. Усього деревини: {count}."
                : $"Усього деревини: {count}. За одне збирання: 5.";
        }
    }
}
