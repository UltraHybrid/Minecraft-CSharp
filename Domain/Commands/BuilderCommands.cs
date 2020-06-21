using System;
using System.Collections.Generic;
using tmp.Domain.TrialVersion.Blocks;

namespace tmp.Domain.Commands
{
    public class PutCommand : ICommand
    {
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BreakCommand : ICommand
    {
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SwapBlock : ICommand
    {
        private readonly IReadOnlyList<BlockType> allBlockType = BaseBlocks.AllBlocks;
        private readonly Player player;
        private readonly int index;

        public SwapBlock(Player player, int number)
        {
            this.player = player;
            index = number % allBlockType.Count;
        }

        public void Execute()
        {
            Console.WriteLine(allBlockType[index].Name);
            player.ActiveBlock = allBlockType[index];
        }
    }
}