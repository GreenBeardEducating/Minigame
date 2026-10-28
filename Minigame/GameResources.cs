using System.Text.Json;

namespace Minigame
{
    public enum ResourceType
    {
        Milk,
        Wood,
        Stone
    }

    public enum ToolType
    {
        WoodenPickaxe,
        StonePickaxe,
        WoodenAxe,
        StoneAxe,
        WoodenSword,
        StoneSword
    }

    public static class GameResources
    {
        private const string SaveKey = "minigame_save_v1";
        private static readonly Dictionary<ToolType, int> CraftedItems = new();

        static GameResources()
        {
            Load();
        }

        public static event Action? Changed;

        public static int Milk { get; private set; }
        public static int Wood { get; private set; }
        public static int Stone { get; private set; }
        public static int PlayerHealth { get; private set; } = 100;
        public static int DefeatedEnemies { get; private set; }
        public static int MaxPlayerHealth => 100;
        public static bool IsGathering { get; private set; }
        public static ResourceType? CurrentResource { get; private set; }

        public static int GetCount(ResourceType resource) => resource switch
        {
            ResourceType.Milk => Milk,
            ResourceType.Wood => Wood,
            ResourceType.Stone => Stone,
            _ => 0
        };

        public static int GetCraftedCount(ToolType tool) => CraftedItems.GetValueOrDefault(tool);

        public static int GetAttackDamage()
        {
            if (GetCraftedCount(ToolType.StoneSword) > 0)
            {
                return 30;
            }

            return GetCraftedCount(ToolType.WoodenSword) > 0 ? 20 : 10;
        }

        public static bool TryCraft(ToolType tool)
        {
            var (wood, stone) = tool switch
            {
                ToolType.WoodenPickaxe or ToolType.WoodenAxe or ToolType.WoodenSword => (3, 0),
                ToolType.StonePickaxe or ToolType.StoneAxe or ToolType.StoneSword => (2, 3),
                _ => (int.MaxValue, int.MaxValue)
            };

            if (Wood < wood || Stone < stone)
            {
                return false;
            }

            Wood -= wood;
            Stone -= stone;
            CraftedItems[tool] = GetCraftedCount(tool) + 1;
            Save();
            Changed?.Invoke();
            return true;
        }

        public static void DamagePlayer(int damage)
        {
            PlayerHealth = Math.Max(0, PlayerHealth - Math.Max(0, damage));
            if (PlayerHealth == 0)
            {
                PlayerHealth = MaxPlayerHealth;
            }

            Save();
            Changed?.Invoke();
        }

        public static void RegisterEnemyDefeat()
        {
            DefeatedEnemies++;
            Save();
            Changed?.Invoke();
        }

        public static async Task<bool> GatherAsync(ResourceType resource)
        {
            if (IsGathering)
            {
                return false;
            }

            IsGathering = true;
            CurrentResource = resource;
            Changed?.Invoke();

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));

                switch (resource)
                {
                    case ResourceType.Milk:
                        Milk += 5;
                        break;
                    case ResourceType.Wood:
                        Wood += 5;
                        break;
                    case ResourceType.Stone:
                        Stone += 5;
                        break;
                }

                Save();
                return true;
            }
            finally
            {
                IsGathering = false;
                CurrentResource = null;
                Changed?.Invoke();
            }
        }

        private static void Load()
        {
            try
            {
                var json = Preferences.Default.Get(SaveKey, string.Empty);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return;
                }

                var save = JsonSerializer.Deserialize<GameSaveData>(json);
                if (save is null)
                {
                    return;
                }

                Milk = Math.Max(0, save.Milk);
                Wood = Math.Max(0, save.Wood);
                Stone = Math.Max(0, save.Stone);
                PlayerHealth = Math.Clamp(save.PlayerHealth, 1, MaxPlayerHealth);
                DefeatedEnemies = Math.Max(0, save.DefeatedEnemies);

                foreach (var item in save.CraftedItems)
                {
                    if (Enum.TryParse<ToolType>(item.Key, out var tool) && item.Value > 0)
                    {
                        CraftedItems[tool] = item.Value;
                    }
                }
            }
            catch (JsonException)
            {
                Preferences.Default.Remove(SaveKey);
            }
        }

        private static void Save()
        {
            var save = new GameSaveData
            {
                Milk = Milk,
                Wood = Wood,
                Stone = Stone,
                PlayerHealth = PlayerHealth,
                DefeatedEnemies = DefeatedEnemies,
                CraftedItems = CraftedItems.ToDictionary(item => item.Key.ToString(), item => item.Value)
            };

            Preferences.Default.Set(SaveKey, JsonSerializer.Serialize(save));
        }

        private sealed class GameSaveData
        {
            public int Milk { get; set; }
            public int Wood { get; set; }
            public int Stone { get; set; }
            public int PlayerHealth { get; set; } = 100;
            public int DefeatedEnemies { get; set; }
            public Dictionary<string, int> CraftedItems { get; set; } = new();
        }
    }
}
