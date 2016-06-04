using System;
using System.Collections.Generic;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    /// <summary>
    ///     �j��85000bytes������إ߮ɤ��t��LOH(large object heap)
    ///     LOH�b�إߪ��ɭԴN�ݩ�G2�A�u���b�B�zG2���^���ɡA�~�|�B�zLOH����A�åB���|���Y�Ŷ�
    ///     ��t�m�@�Ӥj����ɷ|�u���bLOH�������i��t�m �p�G�Ŷ������|�V�{���ШD��h���Ŷ� �Y��l�Ŷ��]������
    ///     �~�|�Ҽ{�h�ϥΤ��e�Q�^����H�ϥΪ��Ŷ�
    ///     85k|16mb|85k
    ///     85k|16mb|85k|16mb+1|85k
    ///     <- GC.Collect
    ///         85 k|    |85 k|      |85 k
    ///     <- allocation 16 mb + 2 throw MemoryOutException
    /// </summary>
    public class TheDangersOfTheLargeObjectHeap : AbstractExecuteModule
    {
        [AopTarget("https://www.simple-talk.com/dotnet/.net-framework/the-dangers-of-the-large-object-heap/")]
        public override void Execute()
        {
            var handler = new LargeObjectHeapHandler();
            handler.Fill(true, true, false);
            //handler.Fill(true, true, true);
            //handler.Fill(false, true, false);
            //handler.Fill(true, false, false);
        }
    }

    public class LargeObjectHeapHandler
    {
        /// <summary>
        ///     Static variable used to store our 'big' block. This ensures that the block is always up for garbage collection.
        /// </summary>
        public byte[] bigBlock;

        /// <summary>
        ///     Allocates 90,000 byte blocks, optionally intersperced with larger blocks
        /// </summary>
        public void Fill(bool allocateBigBlocks, bool grow, bool alwaysGc)
        {
            // Number of bytes in a small block
            // 90000 bytes, just above the limit for the LOH
            const int blockSize = 90000;

            // Number of bytes in a larger block: 16Mb initially
            var largeBlockSize = 1 << 24;

            // Number of small blocks allocated
            var count = 0;

            try
            {
                // We keep the 'small' blocks around
                // (imagine an algorithm that allocates memory in chunks)
                var smallBlocks = new List<byte[]>();

                while (true)
                {
                    // Write out some status information
                    if ((count%1000) == 0)
                    {
                        Console.CursorLeft = 0;
                        Console.Write(new string(' ', 20));
                        Console.CursorLeft = 0;
                        Console.Write("{0}", count);
                        Console.CursorLeft = 0;
                    }

                    if (alwaysGc)
                    {
                        GC.Collect();
                    }
                    if (allocateBigBlocks)
                    {
                        bigBlock = new byte[largeBlockSize];
                    }
                    if (grow)
                    {
                        largeBlockSize++;
                    }
                    smallBlocks.Add(new byte[blockSize]);
                    count++;
                }
            }
            catch (OutOfMemoryException)
            {
                bigBlock = null;
                GC.Collect();
                Console.WriteLine("{1}Mb allocated", (count*blockSize)/(1024*1024));
            }
        }
    }
}