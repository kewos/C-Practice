using System;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class EnumFlagOperation : AbstractExecuteModule
    {
        /// <summary>
        ///     Flag�ާ@
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            var all = Priority.None;

            //�ϥ�or�W�[�]��ﶵ
            foreach (var p in Enum.GetValues(typeof (Priority)) as Priority[])
            {
                all |= p;
            }

            Console.WriteLine(all.ToString());
            Console.WriteLine(all.HasFlag(Priority.Medium));

            //�ϥ�xor�h���ﶵ
            foreach (var p in Enum.GetValues(typeof (Priority)) as Priority[])
            {
                all ^= p;
            }

            Console.WriteLine(all.ToString());
            Console.WriteLine(all.HasFlag(Priority.Medium));
        }

        [Flags]
        //�i�ϥ�shift�ӷ�value
        private enum PriorityShift
        {
            None = 0,
            VeryLow = 1 << 0,
            Low = 1 << 1,
            Medium = 1 << 2,
            High = 1 << 3,
            VeryHigh = 1 << 4
        }

        //�ϥ�16�i��ӷ�value
        /// <summary>
        ///     �ϥ�16�i��ӷ�value
        ///     0x0 = 0
        ///     0x1 = 1
        ///     0x2 = 2
        ///     0x4 = 4
        ///     0x8 = 8
        ///     0x10 = 16
        ///     0x20 = 32
        ///     0x40 = 64
        ///     0x80 = 128
        ///     0x100 = 256
        ///     0x200 = 512
        ///     0x400 = 1024
        ///     0x800 = 2048
        /// </summary>
        [Flags]
        private enum PriorityHexadecimal
        {
            None = 0x0,
            VeryLow = 0x1,
            Low = 0x2,
            Medium = 0x4,
            High = 0x8,
            VeryHigh = 0x10
        }

        [Flags]
        private enum Priority
        {
            None = 0,
            VeryLow = 1,
            Low = 2,
            Medium = 4,
            High = 8,
            VeryHigh = 16,
            Default = None | VeryLow | Low | Medium
        }
    }
}