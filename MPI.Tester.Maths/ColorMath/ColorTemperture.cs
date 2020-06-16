using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths.ColorMath
{
    public static class CCTtable
    {
        //{mired: 20, temperature (K): 50000, u: 0.181325242146571, v: 0.268455315364412, slope: -0.268764018066496}

        #region  <<   Tk   Ut  Vt Mt Table  >>
        public static double[] Ut = new double[]
        {
                0.18006,    
                0.18066,   
                0.18133,   
                0.18208,   
                0.18293,   
                0.18388,   
                0.18494,   
                0.18611,   
                0.18740,   
                0.18880,   
                0.19032,   
                0.19462,   
                0.19962,   
                0.20525,   
                0.21142,   
                0.21807,   
                0.22511,   
                0.23247,
                0.24010,
                0.25032,
                0.25591,
                0.26400,
                0.27218,
                0.28039,
                0.28863,
                0.29685,
                0.30505,
                0.31320,
                0.32129,
                0.32931,
                0.33724,
        };

        public static double[] Tk = new double[]
        {
                1.00E+99,  
                100000,   
                50000,
                33333,   
                25000,   
                20000,   
                16667,   
                14286,   
                12500,   
                11111,   
                10000,   
                8000,    
                6667,    
                5714,    
                5000,    
                4444,    
                4000,    
                3636,    
                3333,    
                3007,    
                2857,    
                2677,    
                2500,    
                2353,    
                2222,    
                2105,    
                2000,    
                1905,    
                1818,    
                1739,    
                1667,   
        };

        public static double[] Vt = new double[]
        {
                0.26352,   
                0.26589,   
                0.26848,   
                0.27119,   
                0.27407,   
                0.27709,   
                0.28021,   
                0.28342,   
                0.28668,   
                0.28997,   
                0.29326,   
                0.30141,   
                0.30921,   
                0.31647,   
                0.32312,   
                0.32909,   
                0.33439,   
                0.33904,   
                0.34308,   
                0.34754,   
                0.34951,   
                0.35200,   
                0.35407,   
                0.35577,   
                0.35714,   
                0.35823,   
                0.35907,   
                0.35968,   
                0.36011,   
                0.36038,   
                0.36051,  
        };

        public static double[] Mt = new double[]
        {
             -0.24341,
            -0.25479 ,
            -0.26876 ,
            -0.28539 ,
            -0.3047 ,
            -0.32675 ,
            -0.35156 ,
            -0.37915 ,
            -0.40955 ,
            -0.44278 ,
            -0.47888 ,
            -0.58204 ,
            -0.70471 ,
            -0.84901 ,
            -1.0182  ,
            -1.2168  ,
            -1.4512  ,
            -1.7298  ,
            -2.0637  ,
            -2.4681  ,
            -2.9641  ,
            -3.5814  ,
            -4.3633  ,
            -5.3762  ,
            -6.7262  ,
            -8.5955  ,
            -11.32400,        
            -15.62800,       
            -23.32500,       
            -40.77000,      
            -116.4500,        
        };
        #endregion
    }

    public static class CommonCCTCalculate
    {
        //------------------------------------------
        //   20130221 Paul
        //   新增CCT演算法 McCamy法 (1992)
        //    n=(x-0.3320)/(y-0.1858)
        //    CCT=-437*n^3+3601*n^2-6861*n+5514.31
        //------------------------------------------          

        public static double McCamyMethod(double cieX, double cieY)
        {
            if (cieX > 1 || cieX < 0 || Double.IsNaN(cieX))
            {
                return 0;
            }

            double nFactor = (cieX - 0.3320) / (cieY - 0.1858);
            return (-437 * Math.Pow(nFactor, 3)) + (3601 * Math.Pow(nFactor, 2)) - (6861 * nFactor) + 5514.31;
        }

        //  Method 2

        public static double computeCorrelatedColorTemperature2(double u_s, double v_s)
        {
            // 1960 u v

            int t = 0;

            double Tk_t, Tk_tt, u_t, v_t, m_t, d_t, d_tt, Tc_t;  /*dratio_t,*/

            double CCT = 0.0;

            int length = CCTtable.Ut.Length;

            double[] d = new double[length];

            double[] dratio = new double[length];

            for (t = 0; t < length; t++)
            {
                u_t = CCTtable.Ut[t];

                v_t = CCTtable.Vt[t];

                m_t = CCTtable.Mt[t];

                d[t] = ((v_s - v_t) - m_t * (u_s - u_t)) / Math.Sqrt(1 + m_t * m_t);
            }

            for (t = 0; t < 30; t++)
            {
                d_t = d[t];

                d_tt = d[t + 1];

                if (d_tt == 0.0)
                {
                    dratio[t] = 9999.0;
                }
                else
                {
                    dratio[t] = d_t / d_tt;
                }

                Tk_t = CCTtable.Tk[t];

                Tk_tt = CCTtable.Tk[t + 1];

                if (dratio[t] <= 0.0)
                {
                    Tc_t = 1 / (1 / Tk_t + ((d_t) / (d_t - d_tt)) * (1 / Tk_tt - 1 / Tk_t));
                }
                else
                {
                    Tc_t = 0.0;
                }

                CCT += Tc_t;
            }

            return CCT;
        }

        public static double Robertson31PointsMethod(double cieX, double cieY)
        {
            // Convert (x, y) or (u’, v’) to (u, v)

            double vPrime = 0;

            double uPrime = 0;

            double num = -2.0 * cieX + 12.0 * cieY + 3.0;

            if (num != 0.0)
            {
                uPrime = 4.0 * cieX / num;
                vPrime = 9.0 * cieY / num;
            }

            double u = uPrime;

            double v = 2.0 * vPrime / 3.0;

            return computeCorrelatedColorTemperature2(u, v);
        }

    }
}
