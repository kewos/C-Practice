using System.Linq;
using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.Algorithm.Implement
{
    public class SingleNumberó╣ : AbstractExecuteModule
    {
        [AopTarget]
        public override void Execute()
        {
            var nums = new[] {1, 1, 2, 2, 10, 5, 10, 3, 3, 4, 4};

            GetSingleNumberó╣(nums).ToConsole();
        }

        private unsafe int GetSingleNumberó╣(int[] nums)
        {
            fixed (int* pNums = nums)
            {
                for (var i = 0; i < nums.Count() - 1; i++)
                {
                    *(pNums + i + 1) ^= *(pNums + i);
                }

                return nums[nums.Count() - 1];
            }
        }
    }
}