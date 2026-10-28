namespace Minigame
{
    public partial class InventoryPage : ContentPage
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GameResources.Changed += UpdateResourceCounts;
            UpdateResourceCounts();
        }

        protected override void OnDisappearing()
        {
            GameResources.Changed -= UpdateResourceCounts;
            base.OnDisappearing();
        }

        private void UpdateResourceCounts()
        {
            MilkCountLabel.Text = $"Молоко: {GameResources.Milk}";
            WoodCountLabel.Text = $"Деревина: {GameResources.Wood}";
            StoneCountLabel.Text = $"Камінь: {GameResources.Stone}";
            var tools = Enum.GetValues<ToolType>()
                .Select(tool => (Tool: tool, Count: GameResources.GetCraftedCount(tool)))
                .Where(item => item.Count > 0)
                .Select(item => $"{GetToolName(item.Tool)} ?{item.Count}")
                .ToArray();
            ToolsCountLabel.Text = tools.Length == 0
                ? "Інструменти: немає"
                : $"Інструменти: {string.Join(", ", tools)}";
        }

        private static string GetToolName(ToolType tool) => tool switch
        {
            ToolType.WoodenPickaxe => "дерев'яна кирка",
            ToolType.StonePickaxe => "кам'яна кирка",
            ToolType.WoodenAxe => "дерев'яна сокира",
            ToolType.StoneAxe => "кам'яна сокира",
            ToolType.WoodenSword => "дерев'яний меч",
            ToolType.StoneSword => "кам'яний меч",
            _ => tool.ToString()
        };
    }
}
