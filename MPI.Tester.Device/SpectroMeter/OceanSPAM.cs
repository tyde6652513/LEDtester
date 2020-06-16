using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SpectroMeter
{

	public class OceanSPAM
	{
		private object _lockObj;

		private SPAM.CCoSpectrumPeak _spectrumPeak;
		private SPAM.CCoCIEConstants _cieConstants;
		private SPAM.CCoCIEObserver _cieObserver;
		private SPAM.CCoIlluminant _illuminant;
		private SPAM.CCoAdvancedColor _advancedColor;
		private SPAM.CCoCIEColor _cieColor;
		private SPAM.CCoXYZColor _XYZColor;
		private SPAM.CCoAdvancedPeakFinding _advSpectrumPeak;
		private SPAM.CCoDominantWavelengthPurity _dominantWavelengthPurity;
		private SPAM.CCoCorrelatedColorTemperature _computerCCT;
		private SPAM.CCoColorRenderingIndex _computerCRI;
		//private SPAM.CCoAdvancedPeakFinding _peakFinding;
		private SPAM.CCoAdvancedPhotometrics _photometry;
		private SPAM.CCoAdvancedAbsoluteIrradiance _absIrrad;
		private SPAM.CCoAdvancedIrradianceCalibration _irradianceCalibration;		//caculate uJ/cm^2
		private SPAM.CCoArrayMath _arrayMath;
		private SPAM.CCoNumericalMethods _numericalMethod;
        private SPAM.CCouvwPrime _uvwPrime;

		private double _FWHM;
		private double _WLP;
		private double _WLC;

		private double _CIEx;
		private double _CIEy;
		private double _purity;
		private double _WLD;
		private double _CCT;
		private double _CRI;
        private double _CIE1976u;
        private double _CIE1976v;

		private double _luminousFlux;
		private double _watt;
		private double _watt02;

        private double[] _specialCRI;
        private double _colorDelta;
        private const int CRI_SAMPLES_COUNT = 15;

        private double _tristimulusX;
        private double _tristimulusY;
        private double _tristimulusZ;

		public OceanSPAM()
		{
			this._lockObj = new object();

			this.ResetAllParam();
		}

		#region >>> Public Property <<<

		public double FWHM
		{
			get { return this._FWHM; }
		}

		public double WLC
		{
			get { return this._WLC; }
		}

		public double WLP
		{
			get { return this._WLP; }
		}

		public double CIEx
		{
			get { return this._CIEx; }
		}

		public double CIEy
		{
			get { return this._CIEy; }
		}

        public double CIE1976u
        {
            get { return this._CIE1976u; }
        }

        public double CIE1976v
        {
            get { return this._CIE1976v; }
        }

		public double CIEz
		{
			get { return ( 1.0d - this._CIEx - this._CIEy ) ; }
		}

		public double Purity
		{
			get { return this._purity; }
		}

		public double WLD
		{
			get { return this._WLD; }
		}

		public double CCT
		{
			get { return this._CCT; }
		}

		public double CRI
		{
			get { return this._CRI; }
		}

        public double ColorDelta
        {
            get { return this._colorDelta; }
        }

        public double[] SpecialCRI
        {
            get { return this._specialCRI; }
        }

		public double LuminousFlux
		{
			get { return this._luminousFlux; }
		}

		public double Watt
		{
			get { return this._watt; }
		}

		public double Watt02
		{
			get { return this._watt02; }
		}

        public double TristimulusX
        {
            get { return this._tristimulusX; }
            set { lock(this._lockObj) { this._tristimulusX = value; } }
        }

        public double TristimulusY
        {
            get { return this._tristimulusY; }
            set { lock(this._lockObj) { this._tristimulusY = value; } }
        }

        public double TristimulusZ
        {
            get { return this._tristimulusZ; }
            set { lock(this._lockObj) { this._tristimulusZ = value; } }
        }

		#endregion

		#region >>> Public Method <<<

		public bool Init()
		{
			try
			{
				//---------------------------------------------------------------------------
				// Create SPAM 
				//---------------------------------------------------------------------------               

				this._spectrumPeak = new SPAM.CCoSpectrumPeak();
				this._cieConstants = new SPAM.CCoCIEConstants();
				this._cieObserver = new SPAM.CCoCIEObserver();
				this._illuminant = new SPAM.CCoIlluminant();
				this._advancedColor = new SPAM.CCoAdvancedColor();
				this._cieColor = new SPAM.CCoCIEColor();
				this._XYZColor = new SPAM.CCoXYZColor();
				this._advSpectrumPeak = new SPAM.CCoAdvancedPeakFinding();
				this._dominantWavelengthPurity = new SPAM.CCoDominantWavelengthPurity();
				this._computerCCT = new SPAM.CCoCorrelatedColorTemperature();
				this._computerCRI = new SPAM.CCoColorRenderingIndex();
				//this._peakFinding = new SPAM.CCoAdvancedPeakFinding();
				this._photometry = new SPAM.CCoAdvancedPhotometrics();
				this._absIrrad = new SPAM.CCoAdvancedAbsoluteIrradiance();
				this._irradianceCalibration = new SPAM.CCoAdvancedIrradianceCalibration();
				this._arrayMath = new SPAM.CCoArrayMath();
				this._numericalMethod = new SPAM.CCoNumericalMethods();
                this._uvwPrime = new SPAM.CCouvwPrime();
			}
			catch
			{
				return false;
			}

			this._cieConstants.CreateCIEConstants();
			this._advancedColor.CreateAdvancedColor();
			//this._peakFinding.CreateAdvancedPeakFinding();
			this._photometry.CreateAdvancedPhotometrics();
			this._absIrrad.CreateAdvancedAbsoluteIrradiance();
			this._irradianceCalibration.CreateAdvancedIrradianceCalibration();
			this._arrayMath.CreateArrayMath();
			this._numericalMethod.CreateNumericalMethods();
            this.SetCIEConstant(0, 7);	
			return true;
		}

		public void SetCIEConstant(int ObserverIndex, int illuminantIndex)
		{
			this._cieObserver = this._cieConstants.getCIEObserverByIndex(ObserverIndex);			// 0 = 1931 , 1 = 1964
			this._illuminant = this._cieConstants.getIlluminantByIndex(illuminantIndex);					// Std. illuminance E		
		}

		public void ResetAllParam()
		{ 
			this._FWHM = 0.0d;
			this._WLP = 0.0d;
			this._WLC = 0.0d;

			this._CIEx = 0.0d;
			this._CIEy = 0.0d;
			this._purity = 0.0d;
			this._WLD = 0.0d;
			this._CCT = 0.0d;
			this._CRI = 0.0d;

			this._luminousFlux = 0.0d;
			this._watt = 0.0d;
			this._watt02 = 0.0d;
            this._colorDelta = 0.0d;
            this._specialCRI = new double[CRI_SAMPLES_COUNT];
		}

		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function :Creates a new instance of SpectrumPeak. 
		// Note that the centroid and integral calculations include the area starting at the peak wavelength and continuing outwards until the tails 
		//	drop below 5% of the peak value on each side. The default integration method is rectangular (given its correctness for point lines). 
		//
		// Parameters:	(1) newPeakIndex - the new index in xValues and yValues of the peak.
		//						(2) xValues - X value array.
		//						(3) yValues - Y value array.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public void ComputeSpectrumData(int peakIndex, double[] wavelength, double[] relativeIntensity)
		{
			this._spectrumPeak.CreateSpectrumPeakB(peakIndex, wavelength, relativeIntensity);

			this._WLP = this._spectrumPeak.getPeakXValue();
			this._FWHM = this._spectrumPeak.getWavelengthFullWidthAtHalfMaximum();
			this._WLC = this._spectrumPeak.getCentroid();
		}
		
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function : Absolute Irradiance Mode calculates the true irradiance of a spectrum, displayed in actual uW/cm²/nm incident on 
		//				   the probe's collection area. It requires per-pixel calibration data to be made available before it can be used. You do not 
		//					need to store reference spectrum first since absolute irradiance is not relative to another measurement. However, you must store
		//					a dark spectrum and have a calibration file before you can measure absolute irradiance. A Wavelength Calibration Data Sheet 
		//					containing the calibration file comes with your Ocean Optics spectrometer. Or, you can do your own calibration if desired.
		// 
		// Paramemters : 	(1) dark - a double array containing the dark spectrum needed to measure absolute irradiance.
		//							(2) sample - a double array containing the acquired spectrum of the sample to measure the absolute irradiance of.
		//							(3) wavelengths - a double array of the wavelegnths.
		//							(4) calibration - a double array of calibrations for each sample minus dark pixel used to calculate the energy of the sample in microjoules - uJoules.
		//							(5) integrationTimeSeconds - the current integration time of the sample spectrum.
		//							(6) collectionArea - The actual collection area. The default is 1.
		//							(7) integratingSphere - Enable this option only if you have an integrating sphere and use the LS1-CAL-INT calibrated light source.
		//								(In this case the source is inside the sphere and there is no collection associated normally with a probe.) When this flag is enabled 
		//								and you are using the integrating sphere to collect light externally (source is outside the integrating sphere), this method assumes 
		//								the value in the Fiber Diameter field is the diameter of the opening in the integrating sphere. 
		//
		// Return : a double array of the true irradiance of a spectrum, displayed in actual uW/cm2/nm incident on the probe's collection area.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double[] ComputeAbsSpectrum(double[] darkArray, double[] sampleData, double[] wavelength, double[] caliSpectrum,
																	double IntegrateTime, double SurfaceArea, bool isUseSphere)
		{
			return ((double[])this._absIrrad.processSpectrum(darkArray, sampleData, wavelength, caliSpectrum, IntegrateTime, SurfaceArea, isUseSphere));
		}

		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function : Computes the tristimulus response to the given emissive energy spectrum. In a given trichromatic system, the tristimulus 
		// values are the amounts of the three reference color stimuli required to match the light considered in a given chromatic system. 
		//
		// Parameters:	(1) energyWavelengths - Wavelengths that correspond to the energy spectrum.
		//						(2) energySpectrum - Sampled energy spectrum (e.g. from Irradiance).
		//						(3) observer - The CIE observer model to use for measuring color.
		//						(4) illuminant - Theoretical illuminant needed to compute the emissive chromaticity to scale the reference values of X, Y, Z 
		//							  used in calculating the tristimulus values. 
		//
		//	Returns : Returns an CIEColor object containing the computed X, Y, and Z values.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public void ComputeColorPurityWLD_CCT_CRI(double[] wavelength, double[] absSpectrum, bool isCalcCCTAndCRI)
		{
			this._cieColor = this._advancedColor.computeEmissiveChromaticity(wavelength, absSpectrum, this._cieObserver, this._illuminant);
			this._XYZColor.CreateXYZColorD(this._cieColor);
			this._dominantWavelengthPurity.CreateDominantWavelengthPurityB(this._cieColor);
            this._uvwPrime.CreateuvwPrimeB(this._cieColor);

			this._CIEx = this._XYZColor.getLittleX();
			this._CIEy = this._XYZColor.getLittleY();
			this._purity = this._dominantWavelengthPurity.getPurity();
			this._WLD = this._dominantWavelengthPurity.getDominantWavelength();
            this._tristimulusX = this._XYZColor.getX();
            this._tristimulusY = this._XYZColor.getY();
            this._tristimulusZ = this._XYZColor.getZ();
            //this._CIE1976u = this._uvwPrime.get_uPrime();
            //this._CIE1976v = this._uvwPrime.get_vPrime();

            if (this._CIEx == 0 || this._CIEy == 0)
            {
                this._CIE1976u = 0.0d;
                this._CIE1976v = 0.0d;
            }
            else
            {
                this._CIE1976u = (4 * this._CIEx) / (3 - 2 * this._CIEx + 12 * this._CIEy);
                this._CIE1976v = (9 * this._CIEy) / (3 - 2 * this._CIEx + 12 * this._CIEy);
            }

        //    this._CIE1976u = (4 * this._CIEx) / (3 - 2 * this._CIEx + 12 * this._CIEy);
        //    this._CIE1976v = (9 * this._CIEy) / (3 - 2 * this._CIEx + 12 * this._CIEy);

			if (isCalcCCTAndCRI)
			{
				this._computerCCT.CreateCorrelatedColorTemperatureB(this._cieColor);
				this._computerCRI.CreateColorRenderingIndexB(this._cieColor);
                this._colorDelta = this._computerCRI.getColorDelta();
				this._CCT = this._computerCCT.getCorrelatedColorTemperature();
				this._CRI = this._computerCRI.getGeneralCRI();

                for (int i = 0; i < this._specialCRI.Length; i++)
                {
                    this._specialCRI[i] = this._computerCRI.getSpecialCRI(i + 1);
                }
			}
			else
			{
				this._CCT = 0.0d;
				this._CRI = 0.0d;					 
			}
		}

		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function : This method computes luminous flux or energy per unit time that is radiated from a source over visible wavelengths.
		//
		// Paramemters :	(1) wavelengths - wavelengths in nm
		//							(2) energyWattsPerNanometer - irradiance of the area.
		//							(3) V_wavelengths - the range of wavelengths for which the relative luminous efficiency function corresponds
		//							(4) V - is the relative luminous efficiency function in lm/W. V is a weighting factor which allows for conversion of Radiant Flux to 
		//									Luminous Flux at any wavelength.
		//							(5) K_m - the maximum luminous efficacy in lm/W. 
		//
		// Return : the luminous flux measured in energy per unit time or lumen (lm).  The unit for luminous flux is the lumen (lm).
		//				// 20111018, Gilbertt, Error : The return luminous flux uint is = micro-lumen ( u-lm).  
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double ComputeLuminousFluxLumen(double[] wavelength, double[] absSpectrum)
		{
			this._luminousFlux = 0.0d;
			this._luminousFlux = this._photometry.computeLuminousFluxLumen(wavelength, absSpectrum,
																this._cieObserver.getWavelengths(),
																this._cieObserver.getV(),
																this._cieObserver.getKm());
			return this._luminousFlux;
		}
		
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function : Computes the power in microwatts for a given absolute irradiance spectrum and collection area. 
		//
		// Parameters:	(1) wavelengths - in nm.
		//						(2) uWPerCmSquaredPerNm - the given absolute irradiance spectrum in uW/cm²/nm.
		//						(3) startingWavelength - the first wavelength of a given wavelength range.
		//						(4) endingWavelength - the last wavelength of a given wavelength range.
		//						(5) method - the integration method or integral mode used to get the integral to compute the power emitted on a surface.
		//						(6) surfaceAreaCmSquared - the area in square centimeters that is being illuminated. 
		// Returns: the power in microwatts for a given absolute irradiance spectrum and collection area.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double ComputeWatt02(double[] wavelength, double[] absSpectrum, double  startWave, double endWave, EIntegrateMode mode,double surfaceArea)
		{
			this._watt02 = 0.0d;
			SPAM.CCoIntegrationMethod method = new SPAM.CCoIntegrationMethod();
			method.CreateIntegrationMethod();
			int  a  = method.getIntegrationMode();

			this._watt02 = this._photometry.compute_uWatt(wavelength, absSpectrum, startWave, endWave, method , surfaceArea);

			return this._watt02;
		}
		
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function :	Perform numerical integration using the three composite quadrature rules (Rectangular, Trapezoid, and Simpson) on the 
		//					function whose number is sent as a parameter. The endpoints of the interval and the number of subdivisions of the interval 
		//					are read in; the results are printed out. 
		//	
		// Parameters :	(1) x - the abscissas.
		//						(2) y - array of y values.
		//						(3) startX - the lower limit of the range of the integration.
		//						(4) endX - the upper limit of the range of the integration.
		//						(5) integraltype - the integral type. [a] Trapezoidal Rule. [b] Simpson's Rule. [c]Rectangular .
		//
		// Returns : returns the integral of the y from startX to endX.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double NumericalIntgrateA(double[] x, double[] y, double startXvalue, double endXValue, EIntegrateMode mode)
		{
			return ((double)this._numericalMethod.integrate(x, y, startXvalue, endXValue, (int)mode));
		}

		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Returns : returns the integral of the y from startX to endX.
		//				 return unit = micro-Watt ( uWatt )
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double ComputeWattA(double[] wavelength, double[] absSpectrum, double startWave, double endWave, EIntegrateMode mode)
		{
			this._watt = 0.0d;
			this._watt = ((double)this._numericalMethod.integrate(wavelength, absSpectrum, startWave, endWave, (int)mode));
			return this._watt;
		}

		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Function :	Perform numerical integration using the three composite quadrature rules (Rectangular, Trapezoid, and Simpson) on 
		//					the function whose number is sent as a parameter. The endpoints of the interval and the number of subdivisions of the 
		//					interval are read in; the results are printed out. 
		//
		// Parameters :	(1) x - the abscissas.
		//						(2) y - array of y values.
		//						(3) startX - the lower limit of the range of the integration.
		//						(4) endX - the upper limit of the range of the integration.
		//						(5) integraltype - the integral type.	[a] Trapezoidal Rule [b] Simpson's Rule [c] Rectangular 
		//
		//	Returns :  returns the integral of the y from startX to endX.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double NumericalIntgrateB(double[] x, double[] y, int startIndex, int endIndex, EIntegrateMode mode)
		{
			return ((double)this._numericalMethod.integrateB(x, y, startIndex, endIndex, (int)mode));
		}

		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Returns : returns the integral of the y from startX to endX.
		//				 return unit = micro-Watt ( uWatt )
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public double ComputeWattB(double[] wavelength, double[] absSpectrum, int startIndex, int endIndex, EIntegrateMode mode)
		{
			this._watt = 0.0d;
			this._watt = ((double)this._numericalMethod.integrateB(wavelength, absSpectrum, startIndex, endIndex, (int)mode));
			return this._watt;
		}

		public double[] MatrixScalarMultiplication(double[] doubleArray, double scalar)
		{
			return ((double[])this._arrayMath.multiplyConstant(doubleArray, scalar));
		}
		
		public double[] NumCublicSpline(double[] xInput, double[] yInput, double[] xOutput)
		{
			return ((double[])this._numericalMethod.cubicSpline(xInput, yInput, xOutput));
		}
				
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
		// Function :This method caluclates a calibration file using a light source needed in obtaining an absolute irradiance measurment. 
		//
		// Parameters :	(1) dark - the last dark spectrum acquired.
		//						(2) light - the spectrum to calulate the true irradiance in &muW/cm²/nm incident on the probe's collection area.
		//						(3) wavelengths - a double array of the reference spectrum's wavelengths.
		//						(4) interpolatedLampCalibration - absolute irradiance measurements for the given lamp file. The lamp file is interpolated with 
		//							  NIST measurements that are given for every 25-50 nm wavelengths.
		//						(5) integrationTimeSeconds - the integration time of the reference spectrum.
		//						(6) collectionArea - the collection area in cm².
		//						(7) integratingSphere - whether or not an integrating sphere will be used to collect light externally. This option should only 
		//   						  be enabled if an integrating sphere is being usedc and an LS1-CAL-INT calibrated light source.(In this case the source is 
		//							  inside the sphere and there is no collection area associated normally with a probe.) 
		//		
		//	Returns: the calibration using in obtaining an absolute irradiance measurment.
		//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
		public double[] IrradianceCalibrate(double[] darkArray, double[] relativeIntensity, double[] wavelength, double[] interpolateLampCalibration,
															 double integrateTime, double surfaceArea, bool isUsePhere)
		{
			return ((double[])this._irradianceCalibration.processSpectrum(darkArray, relativeIntensity, wavelength, interpolateLampCalibration,
																										integrateTime, surfaceArea, isUsePhere));
		}

		#endregion

	}

}
