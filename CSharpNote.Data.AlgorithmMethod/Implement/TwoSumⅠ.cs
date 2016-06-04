using System;
using System.Collections.Generic;
using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.Algorithm.Implement
{
    /// <summary>
    ///     context
    ///     ��Ӽƥ[�_�ӵ���target
    ///     solution
    ///     �Φr��O��target - num �� num �h��mapping
    /// </summary>
    public class TwoSum�� : AbstractExecuteModule
    {
        [MarkedItem]
        public override void Execute()
        {
            GetTwoSum��(new[] { 3, 2, 4 }, 6).Dump();
        }

        private int[] GetTwoSum��(int[] nums, int target)
        {
            var length = nums.Length;
            var dictionary = new Dictionary<int, int>();
            for (var i = 0; i < length; i++)
            {
                var num = nums[i];
                if (dictionary.ContainsKey(num))
                    return new[] { dictionary[num] + 1, i + 1 };

                dictionary[target - num] = i;
            }

            throw new ArgumentException("Invalid argument.");
        }
    }
}