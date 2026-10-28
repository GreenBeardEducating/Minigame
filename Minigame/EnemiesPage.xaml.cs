namespace Minigame
{
    public partial class EnemiesPage : ContentPage
    {
        private int enemyHealth = 30;
        private int enemyIndex;

        public EnemiesPage()
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

        private void OnAttackClicked(object sender, EventArgs e)
        {
            enemyHealth -= GameResources.GetAttackDamage();
            if (enemyHealth <= 0)
            {
                GameResources.RegisterEnemyDefeat();
                StatusLabel.Text = $"Ворога переможено! Всього перемог: {GameResources.DefeatedEnemies}.";
                enemyIndex++;
                enemyHealth = IsZombie ? 50 : 30;
                UpdateStatus();
                return;
            }

            var damage = IsZombie ? 15 : 8;
            var playerWasDefeated = GameResources.PlayerHealth <= damage;
            GameResources.DamagePlayer(damage);
            StatusLabel.Text = playerWasDefeated
                ? "Вас переможено. Ви відродилися з повним здоров'ям; ресурси збережено."
                : $"Ворог завдав {damage} шкоди.";
            UpdateStatus();
        }

        private bool IsZombie => enemyIndex % 2 == 1;

        private void UpdateStatus()
        {
            EnemyNameLabel.Text = IsZombie ? "Зомбі" : "Слайм";
            EnemyArtLabel.Text = IsZombie ? "\U0001F9DF" : "\U0001F7E2";
            EnemyHealthLabel.Text = $"Здоров'я ворога: {enemyHealth}";
            PlayerHealthLabel.Text = $"Ваше здоров'я: {GameResources.PlayerHealth}/{GameResources.MaxPlayerHealth} | Шкода: {GameResources.GetAttackDamage()}";
            KillsLabel.Text = $"Переможено ворогів: {GameResources.DefeatedEnemies}";
        }
    }
}
