using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGame
{
    public class heightMapGauss
    {

        int[,] gaussNew;
        TestSimpleRNG.SimpleRNG myRandom = new TestSimpleRNG.SimpleRNG();


        public heightMapGauss()
        {
            int n = 9; // this should be of the form 2^n + 1
            int[,] gaussOld = new int[n, n];
            int standDev = 512;

            TestSimpleRNG.SimpleRNG.SetSeedFromSystemTime();

            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n - 1; k++)
                {
                    gaussOld[i, k] = (int)(TestSimpleRNG.SimpleRNG.GetNormal(0, standDev));
                    if (k == 0)
                    {
                        gaussOld[i, n - 1] = gaussOld[i, k];
                    }
                    if (i == 0 || i == gaussOld.GetUpperBound(0))
                    {
                        gaussOld[i, k] = -500;
                    }
                }
            }
            standDev /= 2;

            for (int i = 2 * (n - 1) + 1; i < 2000; i = 2 * (i - 1) + 1)
            {
                int j = 0;
                gaussNew = new int[i, i];
                for (int k = 0; k < i; k++)
                {
                    for (int l = 0; l < i; l++)
                    { // -1 to ensure same left as right
                        if (k % 2 == 0 && l % 2 == 0)
                        {
                            gaussNew[k, l] = gaussOld[j / (gaussOld.GetUpperBound(1) + 1), j % (gaussOld.GetUpperBound(1) + 1)]; // I should figure this out. I should be able to treat a 2D array like this
                            j++;
                        }
                        else if (k % 2 == 1 && l % 2 == 1)
                        {
                            gaussNew[k, l] = (int)(TestSimpleRNG.SimpleRNG.GetNormal(0, 2 * standDev));
                        }
                        else
                        {
                            gaussNew[k, l] = (int)(TestSimpleRNG.SimpleRNG.GetNormal(0, standDev));
                        }
                        if ((k == 0 || k == i - 1) && gaussNew[k, l] > 0)
                        {
                            gaussNew[k, l] = -gaussNew[k, l];
                        }
                    }
                }
                for (int k = 0; k < i; k++)
                {
                    gaussNew[k, gaussNew.GetUpperBound(0)] = gaussNew[k, 0];
                }
                average(gaussNew);
                // showElevation( gaussOld );
                gaussOld = gaussNew;
                standDev /= 2;
            }

            //showElevation( gaussOld );
        }

        public static void showElevation(int[,] ele)
        {
            for (int i = 0; i < ele.GetUpperBound(0) + 1; i++)
            {
                string aLine = "";
                for (int j = 0; j < ele.GetUpperBound(1) + 1; j++)
                {
                    aLine += ele[i, j] + ",";
                }
                //System.out.println( aLine );
            }
        }

        public void average(int[,] ele)
        {
            for (int i = 0; i < ele.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < ele.GetUpperBound(1) + 1; j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        ele[i, j] += (ele[i - 1, j - 1] + ele[i - 1, j + 1] + ele[i + 1, j - 1] + ele[i + 1, j + 1]) / 4;
                    }
                }
            }
            for (int i = 0; i < ele.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < ele.GetUpperBound(1) + 1; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        if (i == 0)
                        {
                            ele[i, j] += (ele[i, j - 1] + ele[i, j + 1] + ele[i + 1, j]) / 3;
                        }
                        else if (j == 0)
                        {
                            ele[i, j] += (ele[i - 1, j] + ele[i + 1, j] + ele[i, j + 1]) / 3;
                        }
                        else if (j == ele.GetUpperBound(1) + 1 - 1)
                        {
                            ele[i, j] += (ele[i - 1, j] + ele[i + 1, j] + ele[i, j - 1]) / 3;
                        }
                        else if (i == ele.GetUpperBound(1) + 1 - 1)
                        {
                            ele[i, j] += (ele[i, j - 1] + ele[i, j + 1] + ele[i - 1, j]) / 3;
                        }
                        else
                        {
                            ele[i, j] += (ele[i, j - 1] + ele[i, j + 1] + ele[i - 1, j] + ele[i + 1, j]) / 4;
                        }
                    }
                }
            }
        }


        public int findMaxHeight()
        {
            int highest = gaussNew[0, 0];
            for (int i = 0; i < gaussNew.GetUpperBound(0); i++)
            {
                for (int j = 0; j < gaussNew.GetUpperBound(1); j++)
                {
                    if (gaussNew[i, j] > highest)
                    {
                        highest = gaussNew[i, j];
                    }
                }
            }
            return highest;
        }

        public int findMinHeight()
        {
            int lowest = gaussNew[0, 0];
            for (int i = 0; i < gaussNew.GetUpperBound(0); i++)
            {
                for (int j = 0; j < gaussNew.GetUpperBound(1); j++)
                {
                    if (gaussNew[i, j] < lowest)
                    {
                        lowest = gaussNew[i, j];
                    }
                }
            }
            return lowest;

        }


        public int[,] getHeightMap()
        {
            return gaussNew; 
        }

    }
}
