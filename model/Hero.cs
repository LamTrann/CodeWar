using Sfs2X.Entities.Data;
using System.Collections;

namespace bot
{
    public class Hero {
        public int playerId;
        public HeroIdEnum id;
        public String name;
        public ArrayList gemTypes = new ArrayList();
        private int maxHp; // Hp
        public int maxMana; // Mp
        public int attack;
        private int hp;
        private int mana;

        public Hero(ISFSObject objHero) {
            this.playerId = objHero.GetInt("playerId");
            this.id = EnumUtil.ParseEnum<HeroIdEnum>(objHero.GetUtfString("id"));
            //this.name = id.name();
            this.attack = objHero.GetInt("attack");
            this.hp = objHero.GetInt("hp");
            this.mana = objHero.GetInt("mana");
            this.maxMana = objHero.GetInt("maxMana");

            ISFSArray arrGemTypes = objHero.GetSFSArray("gemTypes");
            for (int i = 0; i < arrGemTypes.Size(); i++) {
                this.gemTypes.Add(EnumUtil.ParseEnum<GemType>(arrGemTypes.GetUtfString(i)));
            }
        }

        public void updateHero(ISFSObject objHero) {
            this.attack = objHero.GetInt("attack");
            this.hp = objHero.GetInt("hp");
            this.mana = objHero.GetInt("mana");
            this.maxMana = objHero.GetInt("maxMana");
        }

        public int presentHp()
        {
            return hp;
        }

        public int presentAttack()
        {
            return attack;
        }

        public bool isAlive() {
            return hp > 0;
        }

        public bool isNearlyDead()
        {
            return hp < 10;
        }


        public bool isFullMana() {
            return mana >= maxMana;
        }

        public bool isManaEqual3()
        {
            return mana == 3;
        }

        public bool isHeroSelfSkill() {
            return HeroIdEnum.SEA_SPIRIT == id;
        }
    }
}