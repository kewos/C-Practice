﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDisplayCommon;
using System.Numerics;

namespace DisPlayProjectEuler
{
    [DisplayClassAttribue]
    public class DisplayProjectEuler : AbstractDisplayMethods
    {
        [DisplayMethod]
        public void LargeRepunitFactors()
        {
            //A number consisting entirely of ones is called a repunit. We shall define R(k) to be a repunit of length k.
            //For example, R(10) = 1111111111 = 11×41×271×9091, and the sum of these prime factors is 9414.
            //Find the sum of the first forty prime factors of R(109).
            Console.WriteLine(Calulate(10));
        }

        public int Calulate(int length)
        {
            var number = ConvertToNumberLoop(length);
            var query = GetFactorOfPrimes(number).Take(4);
            return query.Sum();
        }

        /// <summary>
        /// 取得數值質因數
        /// </summary>
        /// <param name="number">需判斷數字</param>
        /// <returns>質因數數列</returns>
        public IEnumerable<int> GetFactorOfPrimes(int number)
        {
            return Enumerable.Range(2, (int)Math.Sqrt(number) - 1)
                    .AsParallel().AsOrdered()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .Where(x =>
                        Enumerable.Range(2, (int)Math.Sqrt(x)).All(y => x % y != 0) ||
                        x == 2)
                    .Where(x => number % x == 0);
        }

        public int ConvertToNumberLoop(int length)
        {
            var sb = new StringBuilder();
            foreach (var n in Enumerable.Repeat("1", length)) sb.Append(n);
            return int.Parse(sb.ToString());
        }

        [DisplayMethod]
        public void CoinPartitions()
        {
            Console.WriteLine(CaculateCoinPartitions(5));
        }

        public int CaculateCoinPartitions(int total)
        {
            var sum = 0;
            for (int i = 1; i <= total; i++)
            {
                var leak = total - i;
                if (leak > 0)
                {
                    sum += CaculateCoinPartitions(leak);
                    continue;
                }
                else if (leak == 0)
                {
                    return sum++;
                }
            }
            return sum;
        }

        [DisplayMethod]
        public void DivisorSquareSum()
        {
            //Problem 211
            //For a positive integer n, let σ2(n) be the sum of the squares of its divisors. For example,
            //σ2(10) = 1 + 4 + 25 + 100 = 130.
            //Find the sum of all n, 0 < n < 64,000,000 such that σ2(n) is a perfect square.
            object obj = new object();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, 64000), (number) =>
            {
                lock (obj)
                {
                    var factors = GetFactor(number);
                    var sum = GetSequenceSquareSum(factors);
                    if (CheckPerfectSquare(sum))
                        Console.WriteLine(number);
                }
            });
            sw.Stop();
            Console.WriteLine("SpentTime:{0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 判斷是否為完全平方數
        /// </summary>
        /// <param name="numbers">需判斷數字</param>
        /// <returns></returns>
        public bool CheckPerfectSquare(double number)
        {
            return (Math.Sqrt(number) % 1) == 0;
        }

        /// <summary>
        /// 取得全部數值的平方加總
        /// </summary>
        /// <param name="numbers">需平方加總數列</param>
        /// <returns>平方加總</returns>
        public double GetSequenceSquareSum(List<int> numbers)
        {
            var obj = new object();
            double sum = 0;

            Parallel.ForEach(numbers, (number) =>
                {
                    lock (obj)
                    {
                        sum += Math.Pow(number, 2);
                    }
                });
            return sum;
        }

        /// <summary>
        /// 取得因子
        /// </summary>
        /// <param name="number">需取得因子數字</param>
        /// <returns>全部因子</returns>
        public List<int> GetFactor(int number)
        {
            List<int> factors = new List<int>();
            foreach (var factor in Enumerable.Range(1, (int)Math.Floor(Math.Sqrt(number))).Where(x => number % x == 0))
            {
                factors.Add(factor);
                if (!factors.Contains(number / factor))
                    factors.Add(number / factor);
            }
            return factors;
        }

        [DisplayMethod]
        public void GCDSequence()
        {
            //Problem 443
            //Let g(n) be a sequence defined as follows:
            //g(4) = 13,
            //g(n) = g(n-1) + gcd(n, g(n-1)) for n > 4.
            //The first few values are:
            //          n	4	5	6	7	8	9	10	11	12	13	14	15	16	17	18	19	20	...
            //        g(n)	13	14	16	17	18	27	28	29	30	31	32	33	34	51	54	55	60	...
            //        You are given that g(1 000) = 2524 and g(1 000 000) = 2624152.
            //Find g(1015).

            Dictionary<int, int> g = new Dictionary<int, int>();
            g.Add(4, 13);

            foreach (var number in Enumerable.Range(5, 10000))
            {
                var currentResult = g[number - 1] + GetGcd(number, g[number - 1]);
                g.Add(number, currentResult);
                Console.WriteLine("index:{0} number:{1}", number, currentResult);
            }
        }

        /// <summary>
        /// 取得最大公因數
        /// </summary>
        /// <param name="number1">需判斷數字1</param>
        /// <param name="number2">需判斷數字2</param>
        /// <returns>最大公因數</returns>
        public int GetGcd(int number1, int number2)
        {
            var factors1 = GetFactor(number1);
            var factors2 = GetFactor(number2);
            return factors1.Intersect(factors2).Max();
        }

        [DisplayMethod]
        public void CountingNumbersWithatLeastFourDistinctPrimeFactorsLessThan100()
        {
            //It can be verified that there are 23 positive integers less than 1000 that are divisible by at least four distinct primes less than 100.
            //Find how many positive integers less than 1016 are divisible by at least four distinct primes less than 100.
            var primes = GetPrimesWithinRange(100);
            var sum = 0;
            foreach (var number in Enumerable.Range(1, 1000))
            {
                var matchPrime = primes.Where(prime => number % prime == 0).Count();
                if (matchPrime >= 4) sum++;
            }
            Console.WriteLine(sum);
        }

        /// <summary>
        /// 取得2~Range之間的prime
        /// </summary>
        /// <param name="range">範圍</param>
        /// <returns>Primes within 2 and range</returns>
        public IEnumerable<int> GetPrimesWithinRange(int range)
        {
            return Enumerable.Range(2, range - 1).Where(x => GetFactor(x).Count == 2);
        }

        [DisplayMethod]
        public void HowManyReversibleNumbersAreThereBelowOneBillion()
        {
            var sum = 0;
            foreach (var number in Enumerable.Range(1, 900))
            {
                if (number % 10 == 0) continue;
                var reverseNumber = GetReverseNumber(number);
                if (IsReverseNumber(reverseNumber)) sum++;
            }
            Console.WriteLine(sum);
        }

        public bool IsReverseNumber(int number)
        {
            return number == GetReverseNumber(number);
        }

        public int GetReverseNumber(int number)
        {
            return int.Parse(Reverse(number.ToString()));
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        [DisplayMethod]
        public void CountingFractions72()
        {
            //Consider the fraction, n/d, where n and d are positive integers. If n<d and HCF(n,d)=1, it is called a reduced proper fraction.
            //If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get:
            //1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
            //It can be seen that there are 21 elements in this set.
            //How many elements would be contained in the set of reduced proper fractions for d ≤ 1,000,000?
            var max = 100;
            var sum = 0;
            var obj = new object();

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var primes = GetPrimesWithinRange(max);
            Parallel.ForEach(Enumerable.Range(1, max), (fractionM) =>
            {
                Parallel.ForEach(Enumerable.Range(1, fractionM - 1), (fractionC) =>
                {
                    lock (obj)
                    {
                        if (primes.Where(prime => prime <= fractionC).Any(prime => fractionM % prime == 0 && fractionC % prime == 0))
                            sum++;
                        //if (GetGcd(fractionM, fractionC) == 1) sum ++;
                    }
                });
            });
            sw.Stop();
            Console.WriteLine("{0}:{1}", sum, sw.ElapsedMilliseconds);
        }

        [DisplayMethod]
        public void ConsecutivePrimeSum50()
        {
            //The prime 41, can be written as the sum of six consecutive primes:
            //41 = 2 + 3 + 5 + 7 + 11 + 13
            //This is the longest sum of consecutive primes that adds to a prime below one-hundred.
            //The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953.
            //Which prime, below one-million, can be written as the sum of the most consecutive primes?
            var max = 1000;
            var count = 0;
            var sum = 0;
            var primes = GetPrimesWithinRange(max).ToList();
            foreach (var prime in primes)
            {
                if (sum + prime < max && primes.Contains(sum + prime))
                {
                    Console.WriteLine(sum);
                    break;
                }
                count++;
                sum += prime;
            }
        }

        [DisplayMethod]
        public void PrimePermutations49()
        {
            //The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways: (i) each of the three terms are prime, and, (ii) each of the 4-digit numbers are permutations of one another.
            //There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property, but there is one other 4-digit increasing sequence.
            //What 12-digit number do you form by concatenating the three terms in this sequence?
            var digit = 4;
            foreach (var number in Enumerable.Range((int)Math.Pow(10, digit - 1), (int)(Math.Pow(10, digit) - Math.Pow(10, digit - 1))))
            {
                foreach (var step in Enumerable.Range(0, (int)(Math.Pow(10, digit) - number) / 3))
                {
                    var term1 = number + step;
                    var term2 = number + step * 2;
                    var term3 = number + step * 3;
                    if (term1 == 1487 && IsPrime(term1) && IsPrime(term2) && IsPrime(term3))
                    {
                        Console.WriteLine("{0}:{1}:{2}", term1, term2, term3);
                    }
                }
            }
            Console.WriteLine(IsPrime(53));
        }

        public bool IsPrime(int number)
        {
            if (number < 1) return false;
            if (number == 2) return true;
            return GetPrimesWithinRange(number).Contains(number);
        }

        [DisplayMethod]
        public void FindMinimumInRotatedSortedArray()
        {
            //Suppose a sorted array is rotated at some pivot unknown to you beforehand.
            //(i.e., 0 1 2 4 5 6 7 might become 4 5 6 7 0 1 2).
            //Find the minimum element.
            //You may assume no duplicate exists in the array.
            var max = new List<int>();
            List<int> sortArray = new List<int> { 0, 1, 2, 4, 5, 6, 7 };

            for (int i = 0; i < sortArray.Count; i++)
            {
                if (sortArray[0] != 0)
                {
                    max.Add(IntArrayToInt(sortArray));
                }
                sortArray.Add(sortArray[0]);
                sortArray.Remove(sortArray[0]);
            }
            Console.WriteLine(max.Min());
        }

        public int IntArrayToInt(List<int> sortArray)
        {
            var sum = 0;
            for (int i = 0; i < sortArray.Count; i++)
            {
                var term = sortArray.Count - i - 1;
                sum += sortArray[term] * (int)Math.Pow(10, i);
            }
            return sum;
        }

        [DisplayMethod]
        public void GivenAnInputStringReverseTheStringWordByWord()
        {
            //https://oj.leetcode.com/problems/reverse-words-in-a-string/
            //Given an input string, reverse the string word by word.
            //For example,
            //Given s = "the sky is blue",
            //return "blue is sky the".
            var needReverse = "the sky is blue";
            var reverse = string.Join(" ", needReverse.Split(null).Reverse());
            Console.WriteLine(reverse);

        }

        [DisplayMethod]
        public void FindTheContiguousSubarrayWithinAnArray()
        {
            //https://oj.leetcode.com/problems/maximum-product-subarray/
            //Find the contiguous subarray within an array (containing at least one number) which has the largest product.
            //For example, given the array [2,3,-2,4],
            //the contiguous subarray [2,3] has the largest product = 6.
            var list = new List<int> { 2, 3, -2, 4, 10, -3, 5, -1 };
            var subList = new List<int>();
            var max = 0;
            for (int x = 0; x <= list.Count; x++)
                for (int y = 1; x + y <= list.Count; y++)
                {
                    var sub = list.Skip(x).Take(y);
                    var subSumValue = sub.Aggregate((a, b) => a * b);
                    if (subSumValue > max)
                    {
                        max = subSumValue;
                        subList = sub.ToList();
                    }
                }
            var sb = new StringBuilder();
            subList.ForEach(n => sb.Append("[" + n + "]"));
            sb.Append("=" + max);
            Console.WriteLine(sb.ToString());
        }

        [DisplayMethod]
        public void EvaluateReversePolishNotation()
        {
            //https://oj.leetcode.com/problems/evaluate-reverse-polish-notation/
            //Evaluate the value of an arithmetic expression in Reverse Polish Notation.
            //Valid operators are +, -, *, /. Each operand may be an integer or another expression.
            //Some examples:
            //["2", "1", "+", "3", "*"] -> ((2 + 1) * 3) -> 9
            //["4", "13", "5", "/", "+"] -> (4 + (13 / 5)) -> 6
            List<string> evaluate = new List<string> { "4", "13", "5", "/", "+" };
            List<string> notation = new List<string> { "+", "-", "*", "/" };
            while (evaluate.Count != 1)
                for (int i = 0; i < evaluate.Count - 2; i++)
                {
                    int value1, value2;
                    if (int.TryParse(evaluate[i], out value1)
                        && int.TryParse(evaluate[i + 1], out value2)
                        && notation.Any(n => n == evaluate[i + 2])
                    )
                    {
                        Caculate(value1, value2, i + 2, evaluate);
                        break;
                    }
                }
            Console.WriteLine(evaluate.First());
        }

        public void Caculate(int x, int y, int postion, List<string> evaluate)
        {
            switch (evaluate[postion])
            {
                case "+": evaluate[postion] = (x + y).ToString();
                    break;
                case "-": evaluate[postion] = (x - y).ToString();
                    break;
                case "*": evaluate[postion] = (x * y).ToString();
                    break;
                case "/": evaluate[postion] = (x / y).ToString();
                    break;
            }
            evaluate.Remove(evaluate[postion - 1]);
            evaluate.Remove(evaluate[postion - 2]);
        }

        [DisplayMethod]
        public void Candy()
        {
            //https://oj.leetcode.com/problems/candy/
            //There are N children standing in a line. Each child is assigned a rating value.
            //You are giving candies to these children subjected to the following requirements:
            //Each child must have at least one candy.
            //Children with a higher rating get more candies than their neighbors.
            //What is the minimum candies you must give?
            var childredRates = new List<int> { 2, 10, 1, 9, 100, 8, 7, 6 };
            var candies = Enumerable.Repeat(0, 8).ToList();
            while (Enumerable.Range(0, childredRates.Count()).Any(n =>
            {
                var state = false;
                while (candies[n] == 0 || CompareWithNeighbors(childredRates, candies, n))
                {
                    state = true;
                    candies[n]++;
                }
                return state;
            })) ;
            Console.WriteLine(candies.Aggregate((a, b) => a + b));
        }

        public bool CompareWithNeighbors(List<int> childredRates, List<int> candies, int position)
        {
            if (position == 0)
                return (checkNeedAdd(childredRates[position], candies[position], childredRates[position + 1], candies[position + 1])
                    || checkNeedAdd(childredRates[position], candies[position], childredRates[childredRates.Count - 1], candies[childredRates.Count - 1]))
                    ? true : false;
            if (position >= childredRates.Count - 1)
            {
                return (checkNeedAdd(childredRates[position], candies[position], childredRates[position - 1], candies[position - 1])
                    || checkNeedAdd(childredRates[position], candies[position], childredRates[0], candies[0]))
                    ? true : false;
            }
            return (checkNeedAdd(childredRates[position], candies[position], childredRates[position - 1], candies[position - 1])
                    || checkNeedAdd(childredRates[position], candies[position], childredRates[position + 1], candies[position + 1]))
                    ? true : false;
        }

        public bool checkNeedAdd(int childredRate1, int candy1, int childredRate2, int candy2)
        {
            return (childredRate1 > childredRate2 && candy1 <= candy2) ? true : false;
        }

        [DisplayMethod]
        public void WordLadder()
        {
            //https://oj.leetcode.com/problems/word-ladder/
            //Given two words (start and end), and a dictionary, find the length of shortest transformation sequence from start to end, such that:
            //Only one letter can be changed at a time
            //Each intermediate word must exist in the dictionary
            //For example,
            //Given:
            //start = "hit"
            //end = "cog"
            //dict = ["hot","dot","dog","lot","log"]
            //As one shortest transformation is "hit" -> "hot" -> "dot" -> "dog" -> "cog",
            //return its length 5.
            //Note:
            //Return 0 if there is no such transformation sequence.
            //All words have the same length.
            //All words contain only lowercase alphabetic characters.

            var start = "hit";
            var end = "cog";
            var dict = new List<string> { "hot", "dot", "dog", "lot", "log" };
            var path = new Stack<string>();
            Translate(start, end, dict, path).ToList().ForEach(n => Console.WriteLine(n));
        }

        public Stack<string> Translate(string start, string end, List<string> dict, Stack<string> path)
        {
            if (path.Count == 0) path.Push(start);
            if (checkTranslate(start, end))
            {
                path.Push(end);
                return path;
            }
            foreach (var canTranlate in dict.Where(s => checkTranslate(start, s) && !path.Contains(s)))
            {
                path.Push(canTranlate);
                return Translate(canTranlate, end, dict, path);
            }
            path.Pop();
            return path;
        }

        public bool checkTranslate(string s1, string s2)
        {
            return s1.ToList().Intersect(s2.ToList()).Count() == 2;
        }

        [DisplayMethod]
        public void WildcardMatching()
        {
            //https://oj.leetcode.com/problems/wildcard-matching/
            //'?' Matches any single character.
            //'*' Matches any sequence of characters (including the empty sequence).
            //The matching should cover the entire input string (not partial).
            //The function prototype should be:
            //bool isMatch(const char *s, const char *p)
            //Some examples:
            //isMatch("aa","a") → false
            //isMatch("aa","aa") → true
            //isMatch("aaa","aa") → false
            //isMatch("aa", "*") → true
            //isMatch("aa", "a*") → true
            //isMatch("ab", "?*") → true
            //isMatch("aab", "c*a*b") → false
            Console.WriteLine(isMatch("aa", "a"));
            Console.WriteLine(isMatch("aa", "aa"));
            Console.WriteLine(isMatch("aaa", "aa"));
            Console.WriteLine(isMatch("aa", "*"));
            Console.WriteLine(isMatch("aa", "a*"));
            Console.WriteLine(isMatch("ab", "?*"));
            Console.WriteLine(isMatch("aab", "c*a*b"));

        }

        public bool isMatch(string checkString, string validateString)
        {
            if (!validateString.Contains("*") && !validateString.Contains("?")) return checkString.Equals(validateString);
            var postion = 0;
            var bondary = checkString.Length - 1;
            foreach (var c in validateString)
            {
                if (postion > bondary) break;
                if (c.Equals('*')) continue;
                if (c.Equals('?'))
                {
                    postion++;
                    continue;
                }
                if (checkString[postion].Equals(c))
                {
                    postion++;
                    continue;
                }
                while (postion <= bondary && !checkString[postion].Equals(c)) postion++;
            }
            return postion <= bondary;
        }

        [DisplayMethod]
        public void JumpGame()
        {
            //https://oj.leetcode.com/problems/jump-game/
            //Given an array of non-negative integers, you are initially positioned at the first index of the array.
            //Each element in the array represents your maximum jump length at that position.
            //Determine if you are able to reach the last index.
            //For example:
            //A = [2,3,1,1,4], return true.
            //A = [3,2,1,0,4], return false.
            var testA = new List<int> { 2, 3, 1, 1, 4 };
            var testB = new List<int> { 3, 2, 1, 0, 4 };
            Console.WriteLine(CheckJumpGame(testA));
            Console.WriteLine(CheckJumpGame(testB));
        }

        public bool CheckJumpGame(List<int> array)
        {
            var position = 0;
            var lastIndex = array.Count() - 1;
            while (position < lastIndex)
            {
                if (array[position] == 0) break;
                position += array[position];
            }
            return (position == lastIndex) ? true : false;
        }

        [DisplayMethod]
        public void JumpGameII()
        {
            //https://oj.leetcode.com/problems/jump-game-ii/
            //Given an array of non-negative integers, you are initially positioned at the first index of the array.
            //Each element in the array represents your maximum jump length at that position.
            //Your goal is to reach the last index in the minimum number of jumps.
            //For example:
            //Given array A = [2,3,1,1,4]
            //The minimum number of jumps to reach the last index is 2. (Jump 1 step from index 0 to 1, then 3 steps to the last index.)
            var testA = new List<int> { 2, 3, 1, 1, 4 };
            Console.WriteLine(CheckJumpGameMinStep(testA));

        }

        public int CheckJumpGameMinStep(List<int> array, int position = 0, int step = 0)
        {
            var lastIndex = array.Count() - 1;
            if (array[position] == 0 || position > lastIndex) return default(int);
            if (position == lastIndex) return step;
            return Enumerable.Range(1, array[position])
                .Min(n =>
                    CheckJumpGameMinStep(array, position + n, step + 1)
                );
        }

        [DisplayMethod]
        public void MaxPointsOnALine()
        {
            //https://oj.leetcode.com/problems/jump-game-ii/
            //Given n points on a 2D plane, find the maximum number of points that lie on the same straight line.
            Random random = new Random();
            var points = Enumerable.Range(1, 20).Select(n => new { X = (int)random.Next(1, 100), Y = (int)random.Next(1, 100) }).ToList<dynamic>();
            CaculateMaxPointOnALine(points);
            Console.WriteLine(CaculateMaxPointOnALine(points));
        }

        public int CaculateMaxPointOnALine(List<dynamic> points)
        {
            return points.Max(pointA =>
            {
                return points
                    .GroupBy(pointB => CaculateSlope(pointA, pointB))
                    .Max(g => g.Count());
            });
        }

        public double CaculateSlope(dynamic point1, dynamic point2)
        {
            if (point1.Y == point2.Y && point1.X == point2.X) return 100;
            if (point1.Y == point2.Y) return 0;
            if (point1.X == point2.X) return 1;
            return (double)((point1.X - point2.X) / (point1.Y - point2.Y));
        }

        [DisplayMethod]
        public void SingleNumberII()
        {
            //https://oj.leetcode.com/problems/single-number-ii/
            //Given an array of integers, every element appears three times except for one. Find that single one.
            //Note:
            //Your algorithm should have a linear runtime complexity. Could you implement it without using extra memory?
            List<int> TestArray = new List<int> { 3, 1, 4, 1, 4, 2, 4, 1, 5, 2, 2, 3, 3 };
            int ones = 0, twos = 0;
            for (int i = 0; i < TestArray.Count(); i++)
            {
                ones = (ones ^ TestArray[i]) & ~twos;//1100 1101
                twos = (twos ^ TestArray[i]) & ~ones;//0000 0010
            }
            Console.WriteLine(ones);
        }

        [DisplayMethod]
        public void RotateList()
        {
            //https://oj.leetcode.com/problems/rotate-list/
            //Given a list, rotate the list to the right by k places, where k is non-negative.
            //For example:
            //Given 1->2->3->4->5->NULL and k = 2,
            //return 4->5->1->2->3->NULL.

            var rotateList = new List<int> { 1, 2, 3, 4, 5 };
            var k = 2;
            Rotate(ref rotateList, 2);
        }

        public void Rotate(ref List<int> list, int k)
        {
            foreach (var n in Enumerable.Range(0, k))
            {
                list.Insert(0, list.Last());
                list.RemoveAt(list.Count - 1);
            }
        }

        [DisplayMethod]
        public void AddTwoNumbers()
        {
            //https://oj.leetcode.com/problems/add-two-numbers/
            //You are given two linked lists representing two non-negative numbers. The digits are stored in reverse order and each of their nodes contain a single digit. Add the two numbers and return it as a linked list.
            //Input: (2 -> 4 -> 3) + (5 -> 6 -> 4)
            //Output: 7 -> 0 -> 8

            var n1 = new List<int> { 2, 4, 6 };
            var n2 = new List<int> { 5, 6, 4 };
            var count = new List<int>();
            var maxLength = Math.Max(n1.Count, n2.Count);
            var temp = 0;
            foreach (var index in Enumerable.Range(0, maxLength))
            {
                int x = 0, y = 0;
                if (index <= n1.Count - 1) x = n1[index];
                if (index <= n2.Count - 1) y = n2[index];
                count.Add((int)((x + y) % 10) + temp);
                temp = (int)((x + y) / 10);
            }
            count.Add(temp);
        
        }

        [DisplayMethod]
        public void InsertInterval()
        {
            //https://oj.leetcode.com/problems/insert-interval/
            //Given a set of non-overlapping intervals, insert a new interval into the intervals (merge if necessary).
            //You may assume that the intervals were initially sorted according to their start times.
            //Example 1:
            //Given intervals [1,3],[6,9], insert and merge [2,5] in as [1,5],[6,9].
            //Example 2:
            //Given [1,2],[3,5],[6,7],[8,10],[12,16], insert and merge [4,9] in as [1,2],[3,10],[12,16].
            //This is because the new interval [4,9] overlaps with [3,5],[6,7],[8,10].

            var intSet = new List<List<int>> { new List<int> { 1, 2 }, new List<int> { 3, 5 }, new List<int> { 6, 7 }, new List<int> { 8, 10 }, new List<int> { 12, 16 } };
            var newInterval = new List<int> { 1, 100 };
            Insert(intSet, newInterval).ForEach(set => Console.WriteLine("[{0},{1}]", set[0], set[1]));
        }

        public List<List<int>> Insert(List<List<int>> intSet, List<int> interval)
        {
            intSet.Add(interval);
            var list = intSet.Where(set => !(interval[0] < set[0] && interval[1] > set[1]));
            var start = list.Where(set => interval[0] <= set[1] && interval[1] >= set[1]).Min(set => set[0]);
            var end = list.Where(set => interval[1] >= set[0] && interval[0] <= set[0]).Max(set => set[1]);
            var newInterval = new List<int> { start, end };
            var need = list.Where(set => set[1] <= interval[0] || set[0] >= interval[1]).ToList();
            need.Add(newInterval);
            return need;
        }

        [DisplayMethod]
        public void FlattenBinaryTreetoLinkedList()
        {
            //https://oj.leetcode.com/problems/flatten-binary-tree-to-linked-list/
            //Given a binary tree, flatten it to a linked list in-place.
            TreeToList(GetRootOfTree()).ForEach(n => Console.WriteLine(n));
        }

        public Node GetRootOfTree()
        {
            var root = new Node { Key = 1 };
            var node2 = new Node { Key = 2 };
            var node3 = new Node { Key = 3 };
            var node4 = new Node { Key = 4 };
            var node5 = new Node { Key = 5 };
            var node6 = new Node { Key = 6 };
            root.Left = node2;
            root.Right = node5;
            node2.Right = node4;
            node2.Left = node3;
            node5.Right = node6;

            return root;
        }

        public List<int> TreeToList(Node head)
        {
            var newList = new List<int>();

            newList.Add(head.Key);
            if (head.Left != null)
                newList.AddRange(TreeToList(head.Left));

            if (head.Right != null)
                newList.AddRange(TreeToList(head.Right));

            return newList;
        }

        public class Node
        {
            public Node Right { get; set; }
            public Node Left { get; set; }
            public int Key { get; set; }
        }

        [DisplayMethod]
        public void MinimumDepthOfBinaryTree()
        {
            //https://oj.leetcode.com/problems/minimum-depth-of-binary-tree/
            //Given a binary tree, find its minimum depth.
            //The minimum depth is the number of nodes along the shortest path from the root node down to the nearest leaf node.
            Console.WriteLine(GetMinimumDepthOfBinaryTree(GetRootOfTree()));
        }

        public int GetMinimumDepthOfBinaryTree(Node head)
        {
            int leftDepth = 0, rightDepth = 0;

            if (head.Left != null)
                leftDepth = GetMinimumDepthOfBinaryTree(head.Left);

            if (head.Right != null)
                rightDepth = GetMinimumDepthOfBinaryTree(head.Right);

            if ((leftDepth == 0 && rightDepth != 0) || (rightDepth == 0 && leftDepth != 0))
                return 1 + Math.Max(leftDepth, rightDepth);

            return 1 + Math.Min(leftDepth, rightDepth);
        }

        [DisplayMethod]
        public void ValidNumber()
        {
            //https://oj.leetcode.com/problems/valid-number/
            //Validate if a given string is numeric.
            //Some examples:
            //"0" => true
            //" 0.1 " => true
            //"abc" => false
            //"1 a" => false
            //"2e10" => true
            //Note: It is intended for the problem statement to be ambiguous. You should gather all requirements up front before implementing one.
            IsNumberic("0");
            IsNumberic("0.1");
            IsNumberic("abc");
            IsNumberic("1 a");
            IsNumberic("2e");
        }

        public void IsNumberic(string validationString)
        {
            //一般整數開頭不為0
            var integerRegex1 = @"^[0]$";
            //一般整數開頭不為0
            var integerRegex = @"^[-]??[1-9]{1}[0-9]+$";
            //一般浮點結尾不為0
            var floatRegex = @"^[-]??[0-9]+[.]{1}[0-9]+$";
            //一般浮點結尾不為0 開頭0
            var floatRegex1 = @"^[-]??[0-9]{1}[.]{1}[0-9]+$";
            //
            var scientificNotation = @"^[-]??[1-9]{1}[0-9]{0,}[e]{1}$";

            List<string> regexList = new List<string>
            {
                integerRegex,
                integerRegex1,
                floatRegex,
                floatRegex1,
                scientificNotation
            };

            var state = regexList.Any(regex =>
                {
                    return new System.Text.RegularExpressions.Regex(regex).IsMatch(validationString);
                });

            Console.WriteLine("{0}:{1}", validationString, state);
        }

        [DisplayMethod]
        public void TrappingRainWater()
        {
            //https://oj.leetcode.com/problems/trapping-rain-water/
            //Given n non-negative integers representing an elevation map where the width of each bar is 1, compute how much water it is able to trap after raining.
            //For example, 
            //Given [0,1,0,2,1,0,1,3,2,1,2,1], return 6.
            List<int> elements = new List<int> { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
            var sum = 0;
            var Intervals = new List<List<int>>();
            for (int i = 0; i < elements.Count; i++)
            {
                for (int j = i + 1; j < elements.Count; j++)
                {
                    if (elements[j] >= elements[i])
                    {
                        if (!(Intervals.Any(interval => interval[0] <= i && interval[1] >= j))
                            && j - i > 1)
                        {
                            Intervals.Add(new List<int> { i, j });
                            var min = Math.Min(elements[i], elements[j]);
                            sum += Enumerable.Range(i + 1, j - i - 1).Sum(index => min - elements[index]);
                        }
                        break;
                    }
                }
            }

            Console.WriteLine(sum);
            Intervals.ForEach(n => Console.WriteLine("[{0},{1}]", n[0], n[1]));
        }

        [DisplayMethod]
        public void MinimumWindowSubstring()
        {
            //https://oj.leetcode.com/problems/minimum-window-substring/ 
            //Given a string S and a string T, find the minimum window in S which will contain all the characters in T in complexity O(n).
            //For example,
            //S = "ADOBECODEBANC"
            //T = "ABC"
            //Minimum window is "BANC".
            //Note:
            //If there is no such window in S that covers all characters in T, return the emtpy string "".
            //If there are multiple such windows, you are guaranteed that there will always be only one unique minimum window in S.

            string s = "ADOBECODEBANC";
            string t = "ABC";
        }

        public string FindMinimumWindow(string s, string t)
        {
            var a = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (t.Contains(s[i].ToString()))
                {
                    a.Add(i);
                }
            }


            return null;
        }

        [DisplayMethod]
        public void Triangle()
        {
            //https://oj.leetcode.com/problems/triangle/
            //Given a triangle, find the minimum path sum from top to bottom. Each step you may move to adjacent numbers on the row below.
            //For example, given the following triangle
            //[
            //     [2],
            //    [3,4],
            //   [6,5,7],
            //  [4,1,8,3]
            //]
            //The minimum path sum from top to bottom is 11 (i.e., 2 + 3 + 5 + 1 = 11).
            //Note:
            //Bonus point if you are able to do this using only O(n) extra space, where n is the total number of rows in the triangle.

            var triangle = new List<List<int>> { new List<int> { 2 }, new List<int> { 3, 4 }, new List<int> { 6, 5, 7 }, new List<int> { 4, 1, 8, 3 } };
            var level = 0;
            var index = 0;
            Console.WriteLine(GetMinValueByTrianglePath(triangle, level, index).Min(n => n.Sum(x => x)));
        }

        public List<List<int>> GetMinValueByTrianglePath(List<List<int>> triangle, int level, int index)
        {
            if (level + 1 >= triangle.Count) return new List<List<int>> { new List<int> { triangle[level][index] } };
            var leftValue = GetMinValueByTrianglePath(triangle, level + 1, index);
            var righttValue = GetMinValueByTrianglePath(triangle, level + 1, index + 1);
            leftValue.AddRange(righttValue);
            leftValue.ForEach(n => n.Add(triangle[level][index]));
            return leftValue;
        }

        [DisplayMethod]
        public void TwoSum()
        {
            //https://oj.leetcode.com/problems/two-sum/
            //Given an array of integers, find two numbers such that they add up to a specific target number.
            //The function twoSum should return indices of the two numbers such that they add up to the target, where index1 must be less than index2. Please note that your returned answers (both index1 and index2) are not zero-based.
            //You may assume that each input would have exactly one solution.
            //Input: numbers={2, 7, 11, 15}, target=9
            //Output: index1=1, index2=2

            var input = new List<int> { 2, 7, 11, 15 };
            var target = 9;
            FindMatchTargetIndex(input, target);
        }

        public void FindMatchTargetIndex(List<int> numberList, int target)
        {
            numberList.ForEach(x =>
            {
                foreach (var y in numberList.Skip(numberList.BinarySearch(x) + 1).Where(y => x != y && target == x + y))
                {
                    Console.WriteLine("{0},{1}", numberList.BinarySearch(x), numberList.BinarySearch(y));
                }
            });
        }

        [DisplayMethod]
        public void LongestSubstringWithoutRepeatingCharacters()
        {
            //https://oj.leetcode.com/problems/longest-substring-without-repeating-characters/
            //Given a string, find the length of the longest substring without repeating characters. For example, the longest substring without repeating letters for "abcabcbb" is "abc", 
            //which the length is 3. For "bbbbb" the longest substring is "b", with the length of 1.
            var test = "aaaaaaaaaaaaaaaaaaefc";
            Console.WriteLine(GetLengthWithoutRepeat(test));
        }

        public int GetLengthWithoutRepeat(string checkString)
        {
            var stringMax = checkString.Distinct().Count();
            var noRepeatLength = 3;
            var index = 0;
            if (stringMax <= 3) return stringMax;
            while (index < checkString.Length)
            {
                var stringInternal = noRepeatLength + 1;
                if (index + stringInternal > checkString.Length) 
                    break;

                if (stringInternal != checkString.Substring(index, stringInternal).Distinct().Count())
                {
                    index++;
                    continue;
                }

                if (stringInternal > noRepeatLength) 
                    noRepeatLength = stringInternal;
                else 
                    index++;

                if (noRepeatLength == stringMax)
                    break;
            }
            return noRepeatLength;
        }

        [DisplayMethod]
        public void BestTimetoBuyandSellStock()
        {
            //https://oj.leetcode.com/problems/best-time-to-buy-and-sell-stock/
            //Say you have an array for which the ith element is the price of a given stock on day i.
            //If you were only permitted to complete at most one transaction (ie, buy one and sell one share of the stock), design an algorithm to find the maximum profit.
            List<int> price = new List<int> { 10, 5, 8, 9, 9, 4, 2, 6, 7 };
            Console.WriteLine(GetBestTimeToBuyAndSell(price));
        }

        public int GetBestTimeToBuyAndSell(List<int> price)
        {
            if (price.Count() == 0 || price == null || price.Any(p => p < 0)) return 0;
            int min = 0, max = 0;
            for (int i = 0; i < price.Count(); i++)
            {
                if (price[i] < price[min]) min = i;
                if (price[i] > price[max]) max = i;
            }
            return price[max] - price[min];
        }

        [DisplayMethod]
        public void BestTimetoBuyandSellStockII()
        {
            //https://oj.leetcode.com/problems/best-time-to-buy-and-sell-stock-ii/
            //Say you have an array for which the ith element is the price of a given stock on day i.
            //Design an algorithm to find the maximum profit. You may complete as many transactions as you like 
            //(ie, buy one and sell one share of the stock multiple times). However, 
            //you may not engage in multiple transactions at the same time (ie, you must sell the stock before you buy again).
            List<int> price = new List<int> { 10, 5, 8, 1, 5, 3, 9, 4, 2, 6, 7 };
            int totalProfit = 0;
            for (int i = 0; i < price.Count() - 1; i++)
            {
                if (price[i + 1] > price[i]) totalProfit += price[i + 1] - price[i];
            }
            Console.WriteLine(totalProfit);
        }

        [DisplayMethod]
        public void BestTimetoBuyandSellStockIII()
        {
            //https://oj.leetcode.com/problems/best-time-to-buy-and-sell-stock-ii/
            //Say you have an array for which the ith element is the price of a given stock on day i.
            //Design an algorithm to find the maximum profit. You may complete at most two transactions.
            //Note:
            //You may not engage in multiple transactions at the same time (ie, you must sell the stock before you buy again).
            List<int> price = new List<int> { 10, 5, 8, 1, 5, 3, 9, 4, 2, 6, 5 };
            List<List<int>> set = new List<List<int>>();
            var p = 0;
            for (int i = 0; i < price.Count() - 1; i++)
            {
                if (price[i + 1] < price[i])
                {
                    if (i != p) set.Add(new List<int> { p, i });
                    p = i + 1;
                }
                if (i == price.Count() - 2)
                {
                    if (i != p) set.Add(new List<int> { p, i + 1 });
                }
            }
            var maxProfitOfTwoTransaction =
                set.Select(n => price[n[1]] - price[n[0]])
                .OrderByDescending(n => n)
                .Take(2)
                .Sum();
            Console.WriteLine(maxProfitOfTwoTransaction);
        }

        [DisplayMethod]
        public void UniquePaths()
        {
            //https://oj.leetcode.com/problems/unique-paths/
            //A robot is located at the top-left corner of a m x n grid (marked 'Start' in the diagram below).
            //The robot can only move either down or right at any point in time. The robot is trying to reach the bottom-right corner of the grid (marked 'Finish' in the diagram below)
            //How many possible unique paths are there?
            //Above is a 3 x 7 grid. How many possible unique paths are there?
            //Note: m and n will be at most 100.
            int arrayX = 10, arrayY = 9;
            int allStep = arrayX + arrayY - 2;
            int goDown = arrayX - 1;
            double res = 1;
            for (int step = 1; step <= goDown; step++)
            {
                var goRight = allStep - goDown;
                res = res * (goRight + step) / step;
            }

            Console.WriteLine(res);
        }

        [DisplayMethod]
        public void SearchMaxMatrix()
        {
            //Given a matrix consisting of 0's and 1's, find the maximum size sub-matrix consisting of only 1's.
            var array = new int[4, 4] { { 0, 1, 1, 0 }, { 0, 1, 1, 0 }, { 0, 1, 1, 0 }, { 0, 1, 1, 0 } };
            FindMatrix(1, 2, new int[4, 5]);
            array.Dump();
        }

        public void FindMatrix(int x, int y, Array array)
        {
            Console.WriteLine("l{0}r{1}", array.Length, array.Rank);
            for (int i = x; i < array.Length; i++)
            {
                for (int j = y; j < array.Rank; i++)
                {
                }
            }
        }

        [DisplayMethod]
        public void Anagrams()
        {
            //https://oj.leetcode.com/problems/anagrams/
            //Given an array of strings, return all groups of strings that are anagrams.
            //Note: All inputs will be in lower-case.
            List<string> anagrams = new List<string> { "dog", "god", "cab", "bac", "zdz", "aac"};
            anagrams.Where(n =>
            {
                return anagrams.Any(m =>
                {
                    if (n == m) return false;
                    return n.All(c => m.Contains(c));
                });
            }).Dump();
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/sort-colors/")]
        public void SortColors()
        {
            //Given an array with n objects colored red, white or blue, sort them so that objects of the same color are adjacent, with the colors in the order red, white and blue.
            //Here, we will use the integers 0, 1, and 2 to represent the color red, white, and blue respectively.
            //Note:
            //You are not suppose to use the library's sort function for this problem.
            //Follow up:
            //A rather straight forward solution is a two-pass algorithm using counting sort.
            //First, iterate the array counting number of 0's, 1's, and 2's, then overwrite array with total number of 0's, then 1's and followed by 2's.
            //Could you come up with an one-pass algorithm using only constant space?

            List<int> colors = new List<int> { 1, 100, 2, 1, 1, 4, 1, 4, 1, 5, 1, 2 };
            int postion = 0;
            while (postion < colors.Count - 1)
            {
                if (postion < 0) postion = 0;
                if (colors[postion + 1] < colors[postion])
                {
                    Swap(ref colors, postion, postion + 1);
                    postion--;
                    continue;
                }
                postion++;
            }
            colors.Dump();
        }

        public void Swap(ref List<int> items, int index1, int index2)
        {
            var temp = items[index1];
            items[index1] = items[index2];
            items[index2] = temp;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/sort-colors/")]
        public void SortColorsII()
        {
            //Given an array with n objects colored red, white or blue, sort them so that objects of the same color are adjacent, with the colors in the order red, white and blue.
            //Here, we will use the integers 0, 1, and 2 to represent the color red, white, and blue respectively.
            //Note:
            //You are not suppose to use the library's sort function for this problem.
            //Follow up:
            //A rather straight forward solution is a two-pass algorithm using counting sort.
            //First, iterate the array counting number of 0's, 1's, and 2's, then overwrite array with total number of 0's, then 1's and followed by 2's.
            //Could you come up with an one-pass algorithm using only constant space?

            List<int> colors = new List<int> { 1, 1, 2, 1, 0, 1, 2, 1, 0, 1, 1, 2 };
            int blue = -1, white = -1, red = -1;
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i] == 0)
                {
                    colors[++blue] = 0;
                    colors[++white] = 1;
                    colors[++red] = 2;
                }
                else if (colors[i] == 1)
                {
                    colors[++white] = 1;
                    colors[++red] = 2;
                }
                else
                {
                    colors[++red] = 2;
                }
            }
            colors.Dump();
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/longest-common-prefix/")]
        public void LongestCommonPrefix()
        {
            //Write a function to find the longest common prefix string amongst an array of strings.

            List<string> list = new List<string> { "abbd", "bababd", "", "bababd", "", "bababd", "bababd", "bababd", "aababd" };
            int max = 0, current = 0;
            for (int i = 0; i < list.Count() - 1; i++)
            {
                if (list[i].Length == 0 || list[i + 1].Length == 0)
                    continue;
                max = Math.Max(max, ++current);
                if (list[i][0] != list[i + 1][0]) 
                    current = 0;
            }
            Console.WriteLine(max);
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/longest-common-prefix/")]
        public void PalindromeNumber()
        {
            //Determine whether an integer is a palindrome. Do this without extra space.

            long i = 77801120877;
            Console.WriteLine("{0}:IsPalindromeNumber:{1}", i, IsPalindromeNumber(i));
        }
        public bool IsPalindromeNumber(long i)
        {
            if (i > Int64.MaxValue && i < 0) return false;
            var s = i.ToString();
            for (int j = 0; j < (int)(s.Length / 2); j++)
            {
                if (s[j] != s[s.Length - 1 - j]) return false;
            }
            return true;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/longest-common-prefix/")]
        public void PalindromeNumberII()
        {
            //Determine whether an integer is a palindrome. Do this without extra space.

            long i = 7780110877;
            Console.WriteLine("{0}:IsPalindromeNumber:{1}", i, IsPalindromeNumberII(i));
        }

        public bool IsPalindromeNumberII(long i)
        {
            if (i > Int64.MaxValue && i < 0) return false;
            for (int j = 0; j < (int)((int)(Math.Log10(i) + 1) / 2); j++)
            {
                var one = (int)(i / Math.Pow(10, j) % 10);
                var two = (int)(i / Math.Pow(10, (int)(Math.Log10(i) - j) % 10));
                if (one != two) return false;
            }
            return true;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/longest-common-prefix/")]
        public void LengthofLastWord()
        {
            //Determine whether an integer is a palindrome. Do this without extra space.
            //Given a string s consists of upper/lower-case alphabets and empty space characters ' ', return the length of last word in the string.
            //If the last word does not exist, return 0.
            //Note: A word is defined as a character sequence consists of non-space characters only.
            //For example, 
            //Given s = "Hello World",
            //return 5.
            Console.WriteLine(GetLastWordLength("Hello World"));
        }

        public int GetLastWordLength(string s)
        {
            int postion = s.Length - 1;
            while (postion >= 0 && s[postion] != ' ')
                postion--;
            return s.Length - 1 - postion;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/3sum-closest/")]
        public void Sum3Closest()
        {
            //Given an array S of n integers, find three integers in S such that the sum is closest to a given number, target. Return the sum of the three integers. You may assume that each input would have exactly one solution.
            //For example, given array S = {-1 2 1 -4}, and target = 1.
            //The sum that is closest to the target is 2. (-1 + 2 + 1 = 2).

            List<int> set = new List<int> { 1, 2, 5, 7, -4, -4, 10, 9, 7 };
            Console.WriteLine(GetCloseSet(set, -10));
        }

        public int GetCloseSet(List<int> set, int target)
        {
            int x = 0, y = 1, z = 2;
            int min = set.Max();
            while (! (x == set.Count() - 3 && y == set.Count() - 2 && z == set.Count() - 1))
            {
                var close = Math.Abs(target - (set[x] + set[y] + set[z]));
                min = Math.Min(min, close);
                if (y == set.Count() - 2 && z == set.Count() - 1)
                {
                    y = ++x + 1;
                    z = x + 2;
                }
                else if(z == set.Count() - 1)
                {
                    z = ++y + 1;
                }
                else
                {
                    ++z;
                }
            }
            return min;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/largest-rectangle-in-histogram/")]
        public void LargestRectangleinHistogram ()
        {
            //Given n non-negative integers representing the histogram's bar height where the width of each bar is 1, find the area of largest rectangle in the histogram.
            //The largest rectangle is shown in the shaded area, which has area = 10 unit.
            //For example,
            //Given height = [2,1,5,6,2,3],
            //return 10.
            List<int> set = new List<int> { 2, 1, 5, 6, 2, 3 };
            int max = 0;
            for (int x = 0; x < set.Count() - 1; x++)
                for (int y = x; y < set.Count(); y++)
                {
                    var area = Math.Min(set[x], set[y]) * (y - x);
                    max = Math.Max(max, area);
                }
            Console.WriteLine(max);
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/restore-ip-addresses/")]
        public void RestoreIPAddresses()
        {
            //Given a string containing only digits, restore it by returning all possible valid IP address combinations.
            //For example:
            //Given "25525511135",
            //return ["255.255.11.135", "255.255.111.35"]. (Order does not matter)
            RestoreIPAddresses("25525511135").Dump();
        }
        public List<string> RestoreIPAddresses(string ip)
        {
            List<List<string>> ipSet = new List<List<string>>();
            foreach(var x in Enumerable.Range(1, 3))
                foreach (var y in Enumerable.Range(1, 3))
                    foreach (var z in Enumerable.Range(1, 3))
                    {
                        ipSet.Add(new List<string> { ip.Substring(0, x), ip.Substring(x, y), ip.Substring(x + y + 1, z), ip.Substring(x + y + z + 2) });
                    }
            return ipSet.Where(n => n.All(x => check(x))).Select(n => string.Join(".", n)).ToList();
        }

        public bool check(string ipNumber)
        {
            if (ipNumber.Length <= 0 || ipNumber.Length > 3) return false;
            var n = Convert.ToInt32(ipNumber);
            return n > 0 && n <= 255;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/first-missing-positive/")]
        public void FirstMissingPositive ()
        {
            //Given an unsorted integer array, find the first missing positive integer.
            //For example,
            //Given [1,2,0] return 3,
            //and [3,4,-1,1] return 2.
            //Your algorithm should run in O(n) time and uses constant space.
            List<int> set = new List<int> { 3, 4, -1, 1 };
            Console.WriteLine(Enumerable.Range(set.Min(), set.Max()).Where(n => !set.Contains(n)).First());
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/climbing-stairs/")]
        public void ClimbingStairs()
        {   
            //You are climbing a stair case. It takes n steps to reach to the top.
            //Each time you can either climb 1 or 2 steps. In how many distinct ways can you climb to the top?
            Console.WriteLine(ClimbingStairs(100));
        }

        public int ClimbingStairs(int distinct)
        {
            if (distinct < 0) return 0;
            if (distinct <= 2) return distinct;
            return ClimbingStairs(distinct - 2) + ClimbingStairs(distinct - 1);
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/climbing-stairs/")]
        public void ClimbingStairsⅠ()
        {
            //You are climbing a stair case. It takes n steps to reach to the top.
            //Each time you can either climb 1 or 2 steps. In how many distinct ways can you climb to the top?
            Console.WriteLine(ClimbingStairs(100));
        }

        public int ClimbingStairsⅠ(int distinct)
        {
            if (distinct < 0) return 0;
            if (distinct <= 2) return distinct;
            List<int> set = new List<int> { 1, 2 };
            for (int i = 2; i < distinct; i++)
                set.Add(set[i - 2] + set[i - 1]);
            return set.Last();
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/plus-one/")]
        public void PlusOne()
        {
            //Given a non-negative number represented as an array of digits, plus one to the number.
            //The digits are stored such that the most significant digit is at the head of the list.
            List<int> num1 = new List<int> { 9, 2, 5 };
            List<int> num2 = new List<int> { 3, 2, 5 };
            PlusOne(num1, num2).Dump();
        }

        public List<int> PlusOne(List<int> num1, List<int> num2)
        {
            Stack<int> sum = new Stack<int>();
            var max = Math.Max(num1.Count, num2.Count);
            var nextPlus = 0;
            for (int i = 0; i < max; i++)
            {
                var current = 0;
                current += nextPlus;
                if (i < num1.Count) current += num1[num1.Count - 1 - i];
                if (i < num2.Count) current += num2[num2.Count - 1 - i];
                nextPlus = current / 10;
                sum.Push(current % 10);
            }
            if (nextPlus != 0) sum.Push(nextPlus);
            return sum.ToList();
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/rotate-image/")]
        public void RotateImage()
        {
            //You are given an n x n 2D matrix representing an image.
            //Rotate the image by 90 degrees (clockwise).
            //Follow up:
            //Could you do this in-place?
            int[,] matrix = new int[5, 5] { { 1, 5, 5, 5, 5 }, { 1, 5, 5, 5, 5 }, { 1, 5, 5, 5, 5 }, { 1, 5, 5, 5, 5 }, { 1, 5, 5, 5, 5 } };
            var result = RotateDegree90(matrix);
        }

        public int[,] RotateDegree90(int[,] array)
        {
            var result = new int[array.GetLength(1), array.GetLength(0)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    result[j, array.GetLength(0) - i - 1] = array[i, j];
                }
            return result;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/search-a-2d-matrix/")]
        public void SearchA2dMatrix()
        {
            //Write an efficient algorithm that searches for a value in an m x n matrix. This matrix has the following properties:
            //Integers in each row are sorted from left to right.
            //The first integer of each row is greater than the last integer of the previous row.
            //For example,
            //Consider the following matrix:
            //  [1,   3,  5,  7],
            //  [10, 11, 16, 20],
            //  [23, 30, 34, 50]
            //Given target = 3, return true.
            int[,] matrix = new int[3, 4] { { 1, 3, 5, 7 }, { 10, 11, 16, 20 }, { 23, 30, 34, 50 }};
            var target = 9;
            Console.WriteLine(SearchA2dMatrix(matrix, target));
        }

        public bool SearchA2dMatrix(int[,] matrix, int target)
        {
            var end = matrix.GetLength(0) * matrix.GetLength(1);
            var start = matrix.GetLength(1);
            while (start != end)
            {
                var x = (int)(start / matrix.GetLength(1));
                var y = (int)(start % matrix.GetLength(0));
                if (matrix[x, y] - target <= matrix[x - 1, y]) 
                    return false;
                start++;
            }
            return true;

        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/search-a-2d-matrix/")]
        public void ImplementPow()
        {
            Console.WriteLine(ImplementPow(3, 3));
        }

        public double ImplementPow(double x, int y)
        {
            double temp = x;
            if (y == 0) return 1;
            if (y == 1) return x;
            temp = ImplementPow(x, y / 2);
            if (y % 2 == 0) return temp * temp;
            return x * temp * temp;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/divide-two-integers/")]
        public void DivideTwoIntegers()
        {
            //Divide two integers without using multiplication, division and mod operator.
            Console.WriteLine(DivideTwoIntegers(100, 33));
        }

        public int DivideTwoIntegers(int x, int y)
        {
            if (x == 0 || y == 0) return 0;
            var state = 1;
            if (!((x > 0 && y > 0) || (x < 0 && y < 0))) state = 1;
            var sum = 0;
            while ((x >= y))
            {
                x -= y;
                sum++;
            }
            return sum * state;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/search-insert-position/")]
        public void SearchInsertPosition()
        {
            //Given a sorted array and a target value, return the index if the target is found. If not, return the index where it would be if it were inserted in order.
            //You may assume no duplicates in the array.
            //Here are few examples.
            //[1,3,5,6], 5 → 2
            //[1,3,5,6], 2 → 1
            //[1,3,5,6], 7 → 4
            //[1,3,5,6], 0 → 0
            var testArray = new List<int> { 1, 3, 5, 6 };
            Console.WriteLine(SearchInsertPositionⅠ(testArray, 5));
        }

        public int SearchInsertPosition(List<int> array, int target)
        {
            var n = 0;
            while (n < array.Count())
            {
                if (target <= array[n]) return n;
                n++;
            }
            return n;
        }

        public int SearchInsertPositionⅠ(List<int> array, int target)
        {
            var low = 0;
            var height = array.Count - 1;

            if (target <= array[low]) return low;
            if (target > array[height]) return height + 1;

            while (low < height)
            {
                var mid = low + (height - low) / 2;
                if (target > array[mid]) low = mid + 1;
                else if (target < array[mid]) height = mid - 1;
                else return mid;
            }
            return low;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/generate-parentheses/")]
        public void GenerateParentheses()
        {
            //Given n pairs of parentheses, write a function to generate all combinations of well-formed parentheses.
            //For example, given n = 3, a solution set is:
            //"((()))", "(()())", "(())()", "()(())", "()()()"
            var set = new List<string>();
            GenerateParenthesesⅠ(6).Dump();
        }

        public void GenerateParentheses(List<string> set,int n, string s = "")
        {
            if (n == 0)
            {
                if (!set.Contains(s)) set.Add(s);
                return;
            }
            if (s != "()") GenerateParentheses(set, n - 1, s + "()");
            GenerateParentheses(set, n - 1, "()" + s);
            GenerateParentheses(set, n - 1, "(" + s + ")");
        }

        public List<string> GenerateParenthesesⅠ(int n)
        {
            var set = new List<string> { "()" };
            var tempSet = new List<string>();
            for (var i = 1; i < n; i++)
            {
                tempSet.Clear();
                tempSet.AddRange(set);
                set.Clear();
                tempSet.ForEach(s =>
                {
                    if (s != "()") set.Add(s + "()");
                    set.Add("()" + s);
                    set.Add("(" + s + ")");
                });
            }
            return set;
        }

        [DisplayMethod(@"http://community.topcoder.com/tc?module=ProblemDetail&rd=5009&pm=2402")]
        public void BadNeighbors()
        {
            var donations = new List<int>{ 10, 3, 2, 5, 7, 8};
            Console.WriteLine(MaxDonations(donations));
            var donations1 = new List<int> { 11, 15 };
            Console.WriteLine(MaxDonations(donations1));
            var donations2 = new List<int> { 7, 7, 7, 7, 7, 7, 7 };
            Console.WriteLine(MaxDonations(donations2));
            var donations3 = new List<int> { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };
            Console.WriteLine(MaxDonations(donations3));
            var donations4 = new List<int> 
            { 
                94, 40, 49, 65, 21, 21, 106, 80, 92, 81, 679, 4, 61,  
                6, 237, 12, 72, 74, 29, 95, 265, 35, 47, 1, 61, 397,
                52, 72, 37, 51, 1, 81, 45, 435, 7, 36, 57, 86, 81, 72
            };
            Console.WriteLine(MaxDonations(donations4));
        }

        public int MaxDonations(List<int> donations)
        {
            var max = new Dictionary<int, int>();
            max.Add(0, donations[0]);
            max.Add(1, donations[1]);
            var index = 2; 
            while (index < donations.Count() - 1)
            {
                if (index - 2 == 0)
                {
                    max.Add(2, Math.Max(donations[0], donations[donations.Count() - 1]) + donations[2]);
                    index++;
                    continue;
                }
                max.Add(index, Math.Max(max[index - 2], max[index - 3]) + donations[index]);
                index++;
            }
            return max.Max(m => m.Value);
        }

        [DisplayMethod(@"http://community.topcoder.com/tc?module=ProblemDetail&rd=4493&pm=1259")]
        public void ZigZag()
        {
            //var zigZagSequence = new List<int> { 1, 7, 4, 9, 2, 5 };
            //Console.WriteLine(LongestZigZag(zigZagSequence));
            var zigZagSequence1 = new List<int> { 1, 17, 5, 10, 13, 15, 10, 5, 16, 8 };
            Console.WriteLine(LongestZigZag(zigZagSequence1));
            //var zigZagSequence2 = new List<int> { 44 };
            //Console.WriteLine(LongestZigZag(zigZagSequence2));
        }

        public int LongestZigZag(List<int> zigZagSequence)
        {
            int index = 0;
            int max = 1;
            int tempMax = 1;
            bool? state = null;
            while (index < zigZagSequence.Count() - 1)
            {
                var nextState = (zigZagSequence[index] > zigZagSequence[index + 1]);
                if (state == null)
                {
                    state = nextState;
                    tempMax = 2;
                    max = Math.Max(tempMax, max);
                    index++;
                    continue;
                }
                if (nextState == state)
                {
                    state = !state;
                    max = Math.Max(++tempMax, max);
                    index++;
                }
                else
                {
                    state = null;
                }
            }
            return max;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/pascals-triangle-ii/")]
        public void PascalsTriangleII()
        {
            //Given an index k, return the kth row of the Pascal's triangle.
            //For example, given k = 3,
            //Return [1,3,3,1].
            //Note:
            //Could you optimize your algorithm to use only O(k) extra space?
            PascalsTriangleII(3).Dump();
        }

        public List<int> PascalsTriangleII(int k)
        {
            var space = new List<int> { 1 };
            var index = 0;
            while (index < k)
            {
                space.Add(space[0]);
                for (int i = index; i > 0; i--)
                {
                    space[i] += space[i - 1];
                }
                index++;
            }
            return space;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/pascals-triangle/")]
        public void PascalsTriangleI()
        {
            //Given numRows, generate the first numRows of Pascal's triangle.
            PascalsTriangleI(5).DumpMany();
        }

        public List<List<int>> PascalsTriangleI(int k)
        {
            var set = new List<List<int>> { new List<int>{ 1 } };
            for (int i = 1; i <= k; i++)
            {
                var newSet = new List<int>();
                var frontSet = set[i - 1];
                newSet.Add(frontSet[0]);
                for (int j = 1; j < i; j++)
                {
                    newSet.Add(frontSet[j] + frontSet[j - 1]);
                }
                newSet.Add(frontSet[frontSet.Count - 1]);
                set.Add(newSet);
            }
            return set;
        }

        [DisplayMethod]
        public void ArrayListAllSubSet()
        {
            List<int> set = new List<int> { 1, 5, 9 , 100, 4, 99, 88};
            ArrayListAllSubSet(set).DumpMany();
        }

        public List<List<int>> ArrayListAllSubSet(List<int> set, List<int> subset = null, int index = 0)
        {
            if (index >= set.Count) return new List<List<int>> { subset };
            var newset1 = new List<int>();
            var newset2 = new List<int>();
            if (subset != null)
            {
                newset1.AddRange(subset);
                newset2.AddRange(subset);
            }
            newset2.Add(set[index]);
            var result1 = ArrayListAllSubSet(set, newset1, index + 1);
            var result2 = ArrayListAllSubSet(set, newset2, index + 1);

            result1.AddRange(result2);
            return result1;
        }

        [DisplayMethod(@"http://community.topcoder.com/stat?c=problem_statement&pm=7558")]
        public void AdvertisingAgency()
        {
            var input1 = new List<int> { 1, 2, 3 };
            Console.WriteLine(numberOfRejections(input1));
            var input2 = new List<int> { 1, 1, 1 };
            Console.WriteLine(numberOfRejections(input2));
            var input3 = new List<int> { 1, 2, 1, 2 };
            Console.WriteLine(numberOfRejections(input3));
        }

        public int numberOfRejections(List<int> requests)
        {
            if (requests.Count > 50) return -1;

            var reject = 0;
            List<int> accepts = new List<int>();
            for (int i = 0; i < requests.Count; i++)
            {
                if (requests[i] > 100 || requests[i] < 1)
                {
                    reject++;
                    continue;
                }
                if (accepts.Contains(requests[i]))
                {
                    reject++;
                }
                else
                {
                    accepts.Add(requests[i]);
                }
            }
            return reject;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/remove-duplicates-from-sorted-list/")]
        public void RemoveDuplicatesfromSortedList()
        {
            //Given a sorted linked list, delete all duplicates such that each element appear only once.
            //For example,
            //Given 1->1->2, return 1->2.
            //Given 1->1->2->3->3, return 1->2->3.
            var array = new List<int> { 1, 1, 2, 3, 3 };
            RemoveDuplicatesfromSortedList(array);
            array.Dump();
        }

        public void RemoveDuplicatesfromSortedList(List<int> array)
        {
            var index = 0;
            while (index < array.Count() - 1)
            {
                while (index + 1 < array.Count && array[index] == array[index + 1])
                    array.RemoveAt(index + 1);
                index++;
            }
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/merge-sorted-array/")]
        public void MergeSortedArray()
        {
            //Given two sorted integer arrays A and B, merge B into A as one sorted array.
            //Note:
            //You may assume that A has enough space (size that is greater or equal to m + n) to hold additional elements from B. The number of elements initialized in A and B are m and n respectively.
            List<int> list1 = new List<int> { 1,1,1,5,6,7};
            List<int> list2 = new List<int> { 2,3,7,9};
            MergeSortedArray(list1, list2).Dump();
        }

        public List<int> MergeSortedArray(List<int> list1, List<int> list2)
        { 
            int p1 = 0, p2 = 0;
            while (p2 < list2.Count)
            {
                while (p1 < list1.Count && list1[p1++] < list2[p2]) {}
                list1.Insert(p1, list2[p2]);
                p2++;
            }
            return list1;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/maximum-subarray/")]
        public void MaximumSubarray ()
        {
            //Find the contiguous subarray within an array (containing at least one number) which has the largest sum.
            //For example, given the array [−2,1,−3,4,−1,2,1,−5,4],
            //the contiguous subarray [4,−1,2,1] has the largest sum = 6.
            var array = new List<int> { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
            Console.WriteLine(MaximumSubarray(array));
        }

        public int MaximumSubarray(List<int> array)
        {
            int index1 = 0;
            int index2 = 0;
            int currentSum = 0;
            int max = array[0];
            while (index1 < array.Count - 1)
            {
                if (index2 >= array.Count)
                {
                    index2 = ++index1;
                    currentSum = 0;
                }
                currentSum += array[index2++];
                max = Math.Max(max, currentSum);
            }
            return max;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/maximum-subarray/", true)]
        public void MaximumSubarrayⅠ()
        {
            var array = new List<int> { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
            Console.WriteLine(MaximumSubarrayⅠ(array));
        }

        public int MaximumSubarrayⅠ(List<int> array)
        {
            int max = array[0], sum = 0;
            for (var i = 0; i < array.Count; i++)
            {
                sum += array[i];
                max = Math.Max(sum, max);
                sum = Math.Max(sum, 0);
            }
            return max;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/combinations/", true)]
        public void Combinations()
        {
            //given two integers n and k, return all possible combinations of k numbers out of 1 ... n.
            //for example,
            //if n = 4 and k = 2, a solution is
            var set = new List<List<int>>();
            var n = 4;
            var k = 2;
            var pass = n - k;
            Combinations(n, k, pass, new List<int>(), set);
            set.DumpMany();
        }

        public void Combinations(int n, int k, int pass, List<int> subset, List<List<int>> set)
        {
            if (k <= 0)
            {
                set.Add(subset);
                return;
            }
            if (pass > 0)
            {
                Combinations(n - 1, k, pass - 1, subset.ToList(), set);
            }
            var subSet1 = subset.ToList();
            subSet1.Add(n);
            Combinations(n - 1, k - 1, pass, subSet1, set);
        }

        public class Aspect
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
   
        public List<int> getCubeList(int max)
        {
            return Enumerable.Range(0, max + 1).Select(n => n * n * n).ToList();
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/remove-duplicates-from-sorted-array-ii/")]
        public void RemoveDuplicatesFromSortedArrayⅡ()
        {
            //Follow up for "Remove Duplicates":
            //What if duplicates are allowed at most twice?
            //For example,
            //Given sorted array A = [1,1,1,2,2,3],
            //Your function should return length = 5, and A is now [1,1,2,2,3].
            var elements = new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 3, 3 };
            Console.WriteLine(RemoveDuplicatesFromSortedArrayⅡ(elements));
        }

        public int RemoveDuplicatesFromSortedArrayⅡ(List<int> elements)
        {
            if (elements.Count() <= 2) return elements.Count();
            for (int i = elements.Count - 1; i >= 2; i--)
                if (elements[i] == elements[i - 2]) elements.RemoveAt(i);
            return elements.Count();
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/sqrtx/")]
        public void ImplementSqrt()
        {
            //Implement int sqrt(int x).
            //Compute and return the square root of x.
            Console.WriteLine(ImplementSqrt(15));
        }

        public long ImplementSqrt(int x)
        {
            long ans = 0;
            long bit = 1l << 16;
            while (bit > 0)
            {
                ans |= bit;
                if (ans * ans > x)
                {
                    ans ^= bit;
                }
                bit >>= 1;
            }
            return (long)ans;
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/distinct-subsequences/")]
        public void DistinctSubsequences()
        {
            //Given a string S and a string T, count the number of distinct subsequences of T in S.
            //A subsequence of a string is a new string which is formed from the original string by deleting some (can be none) of the characters without disturbing the relative positions of the remaining characters. (ie, "ACE" is a subsequence of "ABCDE" while "AEC" is not).
            //Here is an example:
            //S = "rabbbit", T = "rabbit"
            //Return 3.
            var s = "aarabbbit";
            var t = "rabbit";
            Console.WriteLine(DistinctSubsequences(s, t));
        }

        public int DistinctSubsequences(string s, string t)
        {
            var stime = SequenceRepeatTimes(s);
            var ttime = SequenceRepeatTimes(t);
            if (stime.Count != ttime.Count) return 0;
            var all = 1;
            for (var i = 0; i < stime.Count; i++)
            {
                if (stime[i].Key != ttime[i].Key || stime[i].Value < ttime[i].Value) 
                    return 0;
                if (stime[i].Value > ttime[i].Value)
                {
                    all *= AllGroup(stime[i].Value, ttime[i].Value);
                }
            }
            return all;
        }

        public int AllGroup(int x, int y)
        {
            var i = Enumerable.Range(1, y).Aggregate((a, b) => a * b);
            var j = Enumerable.Range(x - y + 1, y).Aggregate((a, b) => a * b);
            return i / j;
        }

        public List<KeyValuePair<char, int>> SequenceRepeatTimes(string s)
        {
            var repeat = new List<KeyValuePair<char, int>>();
            for (var i = 0; i < s.Length; i++)
            {
                if (i == 0 || !(s[i] == s[i - 1]))
                {
                    repeat.Add(new KeyValuePair<char, int>(s[i], 1));
                    continue;
                }
                repeat[repeat.Count - 1] = new KeyValuePair<char, int>(repeat.Last().Key, repeat.Last().Value + 1);
            }
            return repeat;
        }

        [DisplayMethod(@"https://oj.leetcode.com/discuss/2143/any-better-solution-that-takes-less-than-space-while-in-time")]
        public void DistinctSubsequencesⅠ()
        {
            var s = "aaa";
            var t = "a";         
            Console.WriteLine(DistinctSubsequencesⅠ(s, t));
        }

        public int DistinctSubsequencesⅠ(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            if (m < n) return 0;   

            var path = Enumerable.Range(0, n + 1).Select(i => 0).ToList();
            path[0] = 1;           

            for (int i = 1; i <= m; i++)
                for (int j = n; j >= 1; j--)
                    path[j] += (s[i - 1] == t[j - 1])? path[j - 1] : 0;
            return path[n];
        }

        [DisplayMethod(@"https://oj.leetcode.com/problems/permutation-sequence/")]
        public void PermutationSequence()
        {
            PermutationSequence(3).DumpMany();
        }

        public List<List<int>> PermutationSequence(int n)
        {
            var containElement = Enumerable.Range(1, n).ToList();
            List<List<int>> sets = containElement.Select(elements => new List<int> { elements }).ToList();
            containElement.Select(elements => new List<int> { elements });
            for (var i = 1; i < n; i++)
            {
                List<List<int>> temp = new List<List<int>>();
                sets.ForEach(set =>
                {
                    foreach (var element in containElement.Where(elements => !set.Contains(elements)))
                    {
                        var sub = set.ToList();
                        sub.Add(element);
                        temp.Add(sub);
                    }
                });
                sets = temp;
            }
            return sets;
        }

        [DisplayMethod(@"https://projecteuler.net/problem=21")]
        public void AmicableNumbers()
        {
            int max = 10000;
            var primes = GetPrimesWithinRange(max).ToList();
            var sumSet = Enumerable.Range(2, max - 1)
                .Select(number => new 
                    { 
                        Key = number, 
                        Value = GetFactor(number)
                            .Where(factor => factor < number)
                            .Sum() 
                    })
                    .ToDictionary(x => x.Key, x => x.Value);

            foreach (var element in 
                sumSet.Where(set => (sumSet.ContainsKey(set.Value) && set.Key == sumSet[set.Value]) ? true : false)
            )
            {
                Console.WriteLine("index:{0} sum:{1}", element.Key, element.Value);
            }
        }

        [DisplayMethod(@"https://projecteuler.net/problem=37")]
        public void TruncatablePrimes()
        {
            var valid = 3797;
            var validString = valid.ToString();
            var group = new List<int>();
            for (var index = 0; index < validString.Length; index++)
            {
                var sub1 = validString.Substring(validString.Length - index - 1, index + 1);
                var sub2 = validString.Substring(index, validString.Length - index);
                int value1, value2;
                if (int.TryParse(sub1, out value1)) group.Add(value1);
                if (int.TryParse(sub2, out value2)) group.Add(value2);
            }
            Console.WriteLine("All Primes{0}", group.All(i => IsPrime(i)));
        }
    }
}
