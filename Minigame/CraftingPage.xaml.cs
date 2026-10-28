namespace Minigame
{
    public partial class CraftingPage : ContentPage
    {
        public CraftingPage()
        {
            InitializeComponent();
            UpdateStatus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GameResources.Changed += UpdateStatus;
            UpdateStatus();
        }

        protected override void OnDisappearing()
        {
            GameResources.Changed -= UpdateStatus;
            base.OnDisappearing();
        }

        private void OnCraftClicked(object sender, EventArgs e)
        {
            if (sender is not Button { CommandParameter: string toolName } ||
                !Enum.TryParse<ToolType>(toolName, out var tool))
            {
                return;
            }

            StatusLabel.Text = GameResources.TryCraft(tool)
                ? $"Створено: {GetToolName(tool)}."
                : "Недостатньо ресурсів для крафту.";
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            ResourcesLabel.Text = $"Дерево: {GameResources.Wood} | Камінь: {GameResources.Stone}";
            WoodenPickaxeCountLabel.Text = $"Створено: {GameResources.GetCraftedCount(ToolType.WoodenPickaxe)}";
            StonePickaxeCountLabel.Text = $"Створено: {GameResources.GetCraftedCount(ToolType.StonePickaxe)}";
            WoodenAxeCountLabel.Text = $"Створено: {GameResources.GetCraftedCount(ToolType.WoodenAxe)}";
            StoneAxeCountLabel.Text = $"Створено: {GameResources.GetCraftedCount(ToolType.StoneAxe)}";
            WoodenSwordCountLabel.Text = $"Створено: {GameResources.GetCraftedCount(ToolType.WoodenSword)}";
            StoneSwordCountLabel.Text = $"Створено: {GameResources.GetCraftedCount(ToolType.StoneSword)}";
        }

        private static string GetToolName(ToolType tool) => tool switch
        {
            ToolType.WoodenPickaxe => "дерев'яну кирку",
            ToolType.StonePickaxe => "кам'яну кирку",
            ToolType.WoodenAxe => "дерев'яну сокиру",
            ToolType.StoneAxe => "кам'яну сокиру",
            ToolType.WoodenSword => "дерев'яний меч",
            ToolType.StoneSword => "кам'яний меч",
            _ => "предмет"
        };
    }
}
