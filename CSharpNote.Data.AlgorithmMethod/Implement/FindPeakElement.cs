using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.Algorithm.Implement
{
    /// <summary>
    ///     context
    ///     �M��Ĥ@�Ӯp�����޼�
    ///     solution
    ///     ���s �Y�פW�ɥ�peakIndex�x�s��m�U���^��Index
    /// </summary>
    public class FindPeakElement : AbstractExecuteModule
    {
        [AopTarget]
        public override void Execute()
        {
            DoFindPeakElement(new[] {1, 2, 3, 1}).ToConsole();
        }

        public int DoFindPeakElement(int[] nums)
        {
            if (nums.Length < 1)
                return 0;

            var peakIndex = 0;
            for (var index = 0; index < nums.Length - 1; index++)
            {
                if (nums[index] < nums[index + 1])
                    peakIndex = index + 1;
                if (nums[index] > nums[index + 1])
                    return peakIndex;
            }

            return peakIndex;
        }
    }
}