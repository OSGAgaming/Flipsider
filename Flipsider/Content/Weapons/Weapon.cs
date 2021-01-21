using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Flipsider.IStoreable;

namespace Flipsider.Weapons
{
    public abstract class Weapon : IStoreable
    {
        public ItemInfo itemInfo
        {
            get;
            set;
        }
        public int MaxStack
        {
            get => 1;
            set => MaxStack = value;
        }
        public Texture2D? inventoryIcon
        {
            get;
            set;
        }
        public void SetInventoryIcon(Texture2D icon)
        {
            inventoryIcon = icon;
            itemInfo = new ItemInfo();
        }
        public int damage;
        public int delay;
        protected int activeTimeLeft;

        public int comboState;
        protected readonly int maxCombo;

        public bool active => activeTimeLeft > 0;
        public int activeTime => delay - activeTimeLeft;

        public Weapon(int damage, int delay, int maxCombo)
        {
            this.damage = damage;
            this.delay = delay;
            this.maxCombo = maxCombo;
        }

        protected virtual void OnActivate() { }

        public virtual void UpdateActive() { }

        public virtual bool CanUse(Player player) => !player.usingWeapon;

        public virtual void DrawInventory(SpriteBatch spriteBatch, Vector2 pos)
        {
            if (inventoryIcon != null) spriteBatch.Draw(inventoryIcon, pos, Color.White);
        }

        public virtual void Activate(Player player)
        {
            if (!CanUse(player)) return;

            activeTimeLeft = delay;
            comboState++;

            if (comboState > maxCombo)
                comboState = 0;

            OnActivate();
        }

        public virtual void Update() { }

        public virtual void UpdatePassive()
        {
            Update();

            if (active)
            {
                activeTimeLeft--;
                UpdateActive();
            }
        }
    }
}
