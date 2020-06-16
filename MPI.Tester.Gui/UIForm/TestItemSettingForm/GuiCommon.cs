using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Gui
{
    public class GuiCommon
    {
        public static double CalcOffTimeByDutyCycle(double pulseWidth, double duty)
        {
            double OffTime = 0.0d;

            OffTime = (pulseWidth / (duty / 100.0d)) - pulseWidth;

            OffTime = Math.Round(OffTime, 1, MidpointRounding.AwayFromZero);

            return OffTime;
        }

        public static double CalcDutyCycleByOffTime(double pulseWidth, double offTime)
        {
            double duty = 0.0d;

            duty = (pulseWidth / (pulseWidth + offTime));

            duty = Math.Round(duty, 3, MidpointRounding.AwayFromZero) * 100.0d;

            return duty;
        }
    }
}
