
namespace bot
{
    public class Player
    {
        public int playerId;
        public string displayName;
        public List<Hero> heroes;
        public HashSet<GemType> heroGemType;

        public Player(int playerId, string name)
        {
            this.playerId = playerId;
            this.displayName = name;

            heroes = new List<Hero>();
            heroGemType = new HashSet<GemType>();
        }

        public Hero anyBothHeroFullMana()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isFullMana() && hero.id != HeroIdEnum.CERBERUS) return hero;
            }

            return null;
        }

        public Hero anyHeroFullMana()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isFullMana()) return hero;
            }

            return null;
        }

        public Hero IsSeaGodFullMana()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isFullMana() && hero.id == HeroIdEnum.CERBERUS) return hero;
            }

            return null;
        }

        public bool IsFireSpiritFullMana()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isFullMana() && hero.id == HeroIdEnum.FIRE_SPIRIT) return true;
            }

            return false;
        }

        public bool IsFireSpiritManaEqual3()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isManaEqual3() && hero.id == HeroIdEnum.FIRE_SPIRIT) return true;
            }

            return false;
        }

        public Hero IsSeaSpiritFullMana()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && hero.isFullMana() && hero.id == HeroIdEnum.SEA_SPIRIT) return hero;
            }

            return null;
        }

        public Hero maxAttack(List<Gem> gridGems)
        {
            var notHeroSelfSkill = new List<Hero>();
            var gemRedCount = gridGems.Count(x => x.type == GemType.RED);
            foreach (var hero in heroes)
            {
                if (hero.isAlive() && (hero.id != HeroIdEnum.SEA_SPIRIT && hero.id != HeroIdEnum.MONK))
                {
                    notHeroSelfSkill.Add(hero);
                }
            }

            if(notHeroSelfSkill.Any(x=> x.isAlive() && ((x.attack + gemRedCount) >= x.presentHp())))
            {
                return notHeroSelfSkill.Where(x => x.isAlive() && ((x.attack + gemRedCount) >= x.presentHp())).First();
            }
            return notHeroSelfSkill.Where(x => x.isAlive()).Aggregate((i1, i2) => i1.presentHp() - (i1.attack + gemRedCount) < i2.presentHp()- (i2.attack + gemRedCount) ? i1 : i2);
        }

        public Hero maxAttackForLastMoment()
        {
            return heroes.Where(x => x.isAlive()).Aggregate((i1, i2) => i1.attack + i1.maxMana >= i2.attack + i2.maxMana ? i1 : i2);
        }

        public Hero firstHeroAlive()
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive()) return hero;
            }

            return null;
        }

        public bool threeHeroAliveOrNotNearlyDead()
        {
            int count = 0;
            int only = 0;
            foreach (var hero in heroes)
            {
                if (hero.isAlive())
                {
                    if (!hero.isNearlyDead())
                    {
                        count++;
                    }
                }
                else
                {
                    only++;
                }

            }
            if (count < 3)
            {
                if (only > 1)
                {
                    return true;
                }
                return false;
            }
            else return true;
        }

        public Hero affectSeagod()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.CERBERUS) return hero;
            }

            return null;
        }

        public Hero affectFireSpirit()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.FIRE_SPIRIT) return hero;
            }

            return null;
        }

        public Hero affectSeaSpirit()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.SEA_SPIRIT) return hero;
            }

            return null;
        }

        public bool seaGodHeroAlive()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.CERBERUS && hero.isAlive()) return true;
            }

            return false;
        }

        public bool seaGodHeroNearlyDead()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.CERBERUS && hero.isAlive() && hero.presentHp() < 10) return true;
            }

            return false;
        }

        public bool fireSpiritHeroAlive()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.FIRE_SPIRIT && hero.isAlive()) return true;
            }

            return false;
        }

        public bool seaSpiritHeroAlive()
        {
            foreach (var hero in heroes)
            {
                if (hero.id == HeroIdEnum.SEA_SPIRIT && hero.isAlive()) return true;
            }

            return false;
        }

        public bool isEnemyHPRunOut(List<Hero> botHeroes)
        {
            foreach (var hero in heroes)
            {
                if (hero.isAlive())
                {
                    foreach (var botHero in botHeroes)
                    {
                        if (hero.presentHp() < botHero.presentAttack())
                            return true;
                    }
                }
            }

            return false;
        }

        public HashSet<GemType> getRecommendGemType()
        {
            heroGemType.Clear();
            foreach (var hero in heroes)
            {
                if (!hero.isAlive()) continue;

                foreach (var gt in hero.gemTypes)
                {
                    heroGemType.Add((GemType)gt);
                }
            }

            return heroGemType;
        }


    }
}