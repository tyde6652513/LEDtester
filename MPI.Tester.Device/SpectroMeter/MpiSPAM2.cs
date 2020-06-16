using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SpectroMeter
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct OpticalDataPackage
    {
        public double _tristimulusX;
        public double _tristimulusY;
        public double _tristimulusZ;
        public double _CIE1931x;
        public double _CIE1931y;
        public double _CIE1931z;
        public double _CIE1976u;
        public double _CIE1976v;
        public double _purity;
        public double _CCT;

        public double _WLP;
        public double _WLP2;
        public double _WLPNIR;
        public double _WLD;
        public double _WLCv;
        public double _WLCp;
        public double _FWHM;

        public double _watt;
        public double _Lm;
        public double _Lx;

        public double _generalCRI;
        public fixed double _specialCRI[15];
        public double _colorDelta;
        public int criSampleCount;

        public double _Photons;

        int _peakIndex;
    };

    public class MpiSPAM2
    {
        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "StartAndInitial", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _startAndInitial();

        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "SetCIEParameter", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _setCIEParameter(int observerIndex, int illuminantIndex);

        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "SetIsCaculateCCTandCRI", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _setIsCaculateCCTandCRI(bool command);

        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "SetCaculateCRIDataNumbers", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _setCaculateCRIDataNumbers(int number);

        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "SetVLambdaType", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _setVLambdaType(int number);

        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "ComputeAbsSpectrum", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _computeAbsSpectrum(double[] dark, double[] sample, double[] wavelengths, double[] calibration, int inputArrayLength, double integrationTimeSeconds, double collectionArea, bool integratingSphere, double[] outputArray, ref int outputArrayLength);

        [DllImport("MPI.Tester.Spam2.dll", EntryPoint = "CaculateParameter", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _caculateParameter(ref OpticalDataPackage outputDataPackage, double[] wavelengths, double[] spectrum, int wavelengthsSize, int startWavelength, int endWavelength);

        public void StartAndInitial()
        {
            _startAndInitial();
        }

        public void SetCIEParameter()
        {
            _setCIEParameter(0, 0);
        }

        public void SetIsCaculateCCTandCRI(bool YN)
        {
            _setIsCaculateCCTandCRI(YN);
        }

        public void SetCaculateCRIDataNumbers(int number)
        {
            _setCaculateCRIDataNumbers(number);
        }

        public void SetVLambdaType(int number)
        {
            _setVLambdaType(number);
        }

        public double[] ComputeAbsSpectrum(double[] dark, double[] sample, double[] wavelengths, double[] calibration, double integrationTimeSeconds, double collectionArea, bool integratingSphere)
        {
            double[] outputArray = new double[wavelengths.Length];

            int outputLength = 0;

            _computeAbsSpectrum(dark, sample, wavelengths, calibration, sample.Length, integrationTimeSeconds, collectionArea, integratingSphere, outputArray, ref outputLength);

            return outputArray;
        }

        public void CaculateParameter(ref OpticalDataPackage outputDataPackage, double[] wavelengths, double[] spectrum, int startWavelength, int endWavelength)
        {
            _caculateParameter(ref outputDataPackage, wavelengths, spectrum, wavelengths.Length, startWavelength, endWavelength);
        }
    }
}
