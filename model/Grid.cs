
using Sfs2X.Entities.Data;

namespace bot
{
    public class Grid
    {
        public List<Gem> gems = new List<Gem>(); // toan bo gem
        private ISFSArray gemsCode;
        public HashSet<GemType> gemTypes = new HashSet<GemType>();
        private HashSet<GemType> myHeroGemType;
        private int count = 0;

        public Grid(ISFSArray gemsCode, ISFSArray gemModifiers, HashSet<GemType> gemTypes)
        {
            updateGems(gemsCode, gemModifiers);
            this.myHeroGemType = gemTypes;
        }

        public void updateGems(ISFSArray gemsCode, ISFSArray gemModifiers)
        {
            gems.Clear();
            gemTypes.Clear();
            for (int i = 0; i < gemsCode.Size(); i++)
            {
                Gem gem = new Gem(i, (GemType)gemsCode.GetByte(i), gemModifiers != null ? (GemModifier)gemModifiers.GetByte(i) : GemModifier.NONE);
                gems.Add(gem);
                gemTypes.Add(gem.type);
            }
        }

        public int getNumberOfRedGem()
        {
            List<GemSwapInfo> listMatchGem = suggestMatch();
            int numberOfRedGem = listMatchGem.Count(gemMatch => gemMatch.type == GemType.RED);
            return numberOfRedGem;
        }

        public Pair<int> recommendSwapGem(Player botPlayer, Player enemyPlayer)
        {
            count++;
            List<GemSwapInfo> listMatchGem = suggestMatch();

            Console.WriteLine("recommendSwapGem " + listMatchGem.Count);
            if (listMatchGem.Count == 0)
            {
                return new Pair<int>(-1, -1);
            }
            GemSwapInfo matchGemSwordThanFour = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 4 && gemMatch.type == GemType.SWORD).FirstOrDefault();
            if (matchGemSwordThanFour != null)
            {
                return matchGemSwordThanFour.getIndexSwapGem();
            }
            GemSwapInfo matchGemSizeThanFour = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 4).FirstOrDefault();
            GemSwapInfo matchGemSizeThanFourForBlueAndBlack = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 4 && (gemMatch.type == GemType.BLUE || gemMatch.type == GemType.BROWN)).FirstOrDefault();
            GemSwapInfo matchGemSizeThanFourForRedAndPurple = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 4 && (gemMatch.type == GemType.RED || gemMatch.type == GemType.PURPLE)).FirstOrDefault();
            GemSwapInfo matchGemSizeThanFourForYelloAndGreen = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 4 && (gemMatch.type == GemType.YELLOW || gemMatch.type == GemType.GREEN)).FirstOrDefault();
            if (matchGemSizeThanFourForBlueAndBlack != null && botPlayer.seaGodHeroAlive())
            {
                matchGemSizeThanFourForBlueAndBlack.getIndexSwapGem();
            }
            else if (matchGemSizeThanFourForRedAndPurple != null && botPlayer.fireSpiritHeroAlive())
            {
                return matchGemSizeThanFourForRedAndPurple.getIndexSwapGem();
            }
            else if (matchGemSizeThanFourForYelloAndGreen != null && botPlayer.seaSpiritHeroAlive())
            {
                return matchGemSizeThanFourForYelloAndGreen.getIndexSwapGem();
            }
            else if (matchGemSizeThanFour != null)
            {
                return matchGemSizeThanFour.getIndexSwapGem();
            }
            GemSwapInfo matchGemSizeThanThree = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3).FirstOrDefault();
            GemSwapInfo matchGemSizeThanThreeForBlueAndBlack = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3 && (gemMatch.type == GemType.BLUE || gemMatch.type == GemType.BROWN)).FirstOrDefault();
            GemSwapInfo matchGemSizeThanThreeForRedAndPurple = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3 && (gemMatch.type == GemType.RED || gemMatch.type == GemType.PURPLE)).FirstOrDefault();
            GemSwapInfo matchGemSizeThanThreeForGreenAndYellow = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3 && (gemMatch.type == GemType.YELLOW || gemMatch.type == GemType.GREEN)).FirstOrDefault();
            if (matchGemSizeThanThreeForBlueAndBlack != null && botPlayer.seaGodHeroAlive() && botPlayer.heroes.Any(x => x.id == HeroIdEnum.CERBERUS && !x.isFullMana()))
            {
                return matchGemSizeThanThreeForBlueAndBlack.getIndexSwapGem();
            }
            else if (matchGemSizeThanFourForRedAndPurple != null && botPlayer.fireSpiritHeroAlive() && botPlayer.heroes.Any(x => x.id == HeroIdEnum.FIRE_SPIRIT))
            {
                return matchGemSizeThanThreeForRedAndPurple.getIndexSwapGem();
            }
            else if (matchGemSizeThanThreeForGreenAndYellow != null && botPlayer.seaSpiritHeroAlive() && botPlayer.heroes.Any(x => x.id == HeroIdEnum.SEA_SPIRIT))
            {
                return matchGemSizeThanThreeForGreenAndYellow.getIndexSwapGem();
            }
            GemSwapInfo matchGemSword = listMatchGem.Where(gemMatch => gemMatch.type == GemType.SWORD).FirstOrDefault();
           
            GemSwapInfo matchGemSwordThanThree = listMatchGem.Where(gemMatch => gemMatch.sizeMatch > 3 && gemMatch.type == GemType.SWORD).FirstOrDefault();
           
            if (count > 5 && matchGemSwordThanThree != null)
            {
                return matchGemSwordThanThree.getIndexSwapGem();
            }
            if (matchGemSword != null && enemyPlayer.isEnemyHPRunOut(botPlayer.heroes))
            {
                return matchGemSword.getIndexSwapGem();
            }

            foreach (GemType type in myHeroGemType)
            {
                GemSwapInfo matchGem = listMatchGem.Where(gemMatch => gemMatch.type == type).FirstOrDefault();
                GemSwapInfo matchGemForBlueAndBlack = listMatchGem.Where(gemMatch => gemMatch.type == GemType.BLUE || gemMatch.type == GemType.BROWN).FirstOrDefault();
                GemSwapInfo matchGemForRedAndPurple = listMatchGem.Where(gemMatch => gemMatch.type == GemType.RED || gemMatch.type == GemType.PURPLE).FirstOrDefault();
                GemSwapInfo matchGemForGreenAndYellow = listMatchGem.Where(gemMatch => gemMatch.type == GemType.GREEN || gemMatch.type == GemType.YELLOW).FirstOrDefault();

                if (matchGemForBlueAndBlack != null && botPlayer.seaGodHeroAlive() && botPlayer.heroes.Any(x => x.id == HeroIdEnum.CERBERUS))
                {
                    return matchGemForBlueAndBlack.getIndexSwapGem();
                }
                else if (matchGemForRedAndPurple != null && botPlayer.fireSpiritHeroAlive() && botPlayer.heroes.Any(x => x.id == HeroIdEnum.FIRE_SPIRIT))
                {
                    return matchGemForRedAndPurple.getIndexSwapGem();
                }
                else if (matchGemForGreenAndYellow != null && botPlayer.seaSpiritHeroAlive() && botPlayer.heroes.Any(x => x.id == HeroIdEnum.SEA_SPIRIT))
                {
                    return matchGemForGreenAndYellow.getIndexSwapGem();
                }
                else if (matchGem != null)
                {
                    return matchGem.getIndexSwapGem();
                }
            }

            return listMatchGem[0].getIndexSwapGem();
        }

        private List<GemSwapInfo> suggestMatch()
        {
            var listMatchGem = new List<GemSwapInfo>();

            var tempGems = new List<Gem>(gems);
            foreach (Gem currentGem in tempGems)
            {
                Gem swapGem = null;
                // If x > 0 => swap left & check
                if (currentGem.x > 0)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x - 1, currentGem.y)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
                // If x < 7 => swap right & check
                if (currentGem.x < 7)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x + 1, currentGem.y)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
                // If y < 7 => swap up & check
                if (currentGem.y < 7)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x, currentGem.y + 1)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
                // If y > 0 => swap down & check
                if (currentGem.y > 0)
                {
                    swapGem = gems[getGemIndexAt(currentGem.x, currentGem.y - 1)];
                    checkMatchSwapGem(listMatchGem, currentGem, swapGem);
                }
            }
            return listMatchGem;
        }

        private void checkMatchSwapGem(List<GemSwapInfo> listMatchGem, Gem currentGem, Gem swapGem)
        {
            swap(currentGem, swapGem);
            HashSet<Gem> matchGems = matchesAt(currentGem.x, currentGem.y);

            swap(currentGem, swapGem);
            if (matchGems.Count > 0)
            {
                listMatchGem.Add(new GemSwapInfo(currentGem.index, swapGem.index, matchGems.Count, currentGem.type));
            }
        }

        private int getGemIndexAt(int x, int y)
        {
            return x + y * 8;
        }

        private void swap(Gem a, Gem b)
        {
            int tempIndex = a.index;
            int tempX = a.x;
            int tempY = a.y;

            // update reference
            gems[a.index] = b;
            gems[b.index] = a;

            // update data of element
            a.index = b.index;
            a.x = b.x;
            a.y = b.y;

            b.index = tempIndex;
            b.x = tempX;
            b.y = tempY;
        }

        private HashSet<Gem> matchesAt(int x, int y)
        {
            HashSet<Gem> res = new HashSet<Gem>();
            Gem center = gemAt(x, y);
            if (center == null)
            {
                return res;
            }

            // check horizontally
            List<Gem> hor = new List<Gem>();
            hor.Add(center);
            int xLeft = x - 1, xRight = x + 1;
            while (xLeft >= 0)
            {
                Gem gemLeft = gemAt(xLeft, y);
                if (gemLeft != null)
                {
                    if (!gemLeft.sameType(center))
                    {
                        break;
                    }
                    hor.Add(gemLeft);
                }
                xLeft--;
            }
            while (xRight < 8)
            {
                Gem gemRight = gemAt(xRight, y);
                if (gemRight != null)
                {
                    if (!gemRight.sameType(center))
                    {
                        break;
                    }
                    hor.Add(gemRight);
                }
                xRight++;
            }
            if (hor.Count >= 3) res.UnionWith(hor);

            // check vertically
            List<Gem> ver = new List<Gem>();
            ver.Add(center);
            int yBelow = y - 1, yAbove = y + 1;
            while (yBelow >= 0)
            {
                Gem gemBelow = gemAt(x, yBelow);
                if (gemBelow != null)
                {
                    if (!gemBelow.sameType(center))
                    {
                        break;
                    }
                    ver.Add(gemBelow);
                }
                yBelow--;
            }
            while (yAbove < 8)
            {
                Gem gemAbove = gemAt(x, yAbove);
                if (gemAbove != null)
                {
                    if (!gemAbove.sameType(center))
                    {
                        break;
                    }
                    ver.Add(gemAbove);
                }
                yAbove++;
            }
            if (ver.Count >= 3) res.UnionWith(ver);

            return res;
        }

        // Find Gem at Position (x, y)
        private Gem gemAt(int x, int y)
        {
            foreach (Gem g in gems)
            {
                if (g != null && g.x == x && g.y == y)
                {
                    return g;
                }
            }
            return null;
        }
    }
}