using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
    public class PIVTestItem : TestItemData
    {
        private LaserCalcSetting _calcSetting;

        public PIVTestItem() : base()
        {
            this._lockObj = new object();
            
            this._type = ETestType.PIV;

            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.PIV;
            this._elecSetting[0].IsAutoTurnOff = true;

            this._gainOffsetSetting = null;

            this._calcSetting = new LaserCalcSetting();

            // Then reset those keyname of this._msrtResult[] Array
            this.CreateGainAndMsrtItem();

            this.ResetKeyName();
        }

        #region >>> Public Property <<<

        public LaserCalcSetting CalcSetting
        {
            get { return this._calcSetting; }
            set { lock (this._lockObj) { this._calcSetting = value; } }
        }

        #endregion

        #region >>> Private Methods <<<

        private void SetKeyName(int num)
        {
            this._elecSetting[0].KeyName = this.KeyName;

            string[] str = Enum.GetNames(typeof(ELaserMsrtType));

            // Reset Tested Result KeyName and Gain Offset Seeting KeyName
            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                if (this._msrtResult[i] == null)
                    break;

                this._msrtResult[i].KeyName = str[i] + "_" + num.ToString();
                //this._msrtResult[i].Name = str[i]  + num.ToString("D2");
                this._msrtResult[i].Name = this._msrtResult[i].KeyName;
                //SetMsrtNameAsKey();
                this._gainOffsetSetting[i].KeyName = this._msrtResult[i].KeyName;
                this._gainOffsetSetting[i].Name = this._msrtResult[i].Name;
            }		
        }

        #endregion

        #region >>> Protected Methods <<<

        public void ResetMsrtItems(int index)
        {
            this.CreateGainAndMsrtItem();

            this.SetKeyName(index);
        }

        private void CreateGainAndMsrtItem()
        {
            // New the MsrtResult Data and GainOffsetSetting Data
            this._msrtResult = new TestResultData[Enum.GetNames(typeof(ELaserMsrtType)).Length];

            this._gainOffsetSetting = new GainOffsetData[Enum.GetNames(typeof(ELaserMsrtType)).Length];

            for (int i = 0; i < this._msrtResult.Length; i++)
            {
                this._msrtResult[i] = new TestResultData();

                this._gainOffsetSetting[i] = new GainOffsetData(false, EGainOffsetType.Offset);

                //this._gainOffsetSetting[i].IsVision = false;
                //this._gainOffsetSetting[i].IsEnable = false;
            }

            // Set Tested Result Items and Gain Offset Setting
            this._msrtResult[(int)ELaserMsrtType.Pop].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.Pop].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Pop].MaxLimitValue = 999.999d;
            this._msrtResult[(int)ELaserMsrtType.Pop].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Pop].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)ELaserMsrtType.Pop].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.Pop].Type = EGainOffsetType.None;

            this._msrtResult[(int)ELaserMsrtType.Iop].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Iop].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Iop].MaxLimitValue = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Iop].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Iop].MaxLimitValue2 = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Iop].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Vop].Unit = "V";
            this._msrtResult[(int)ELaserMsrtType.Vop].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Vop].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.Vop].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Vop].MaxLimitValue2 = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.Vop].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Imop].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Imop].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Imop].MaxLimitValue = 100.0d;
            this._msrtResult[(int)ELaserMsrtType.Imop].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Imop].MaxLimitValue2 = 100.0d;
            this._msrtResult[(int)ELaserMsrtType.Imop].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Pceop].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.Pceop].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.Pceop].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Pceop].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Pceop].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Pceop].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.Pceop].Type = EGainOffsetType.None;

            this._msrtResult[(int)ELaserMsrtType.Ipk].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Ipk].Formate = "0.0000";
            this._msrtResult[(int)ELaserMsrtType.Ipk].MaxLimitValue = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Ipk].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Ipk].MaxLimitValue2 = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Ipk].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Ppk].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.Ppk].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Ppk].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Ppk].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Ppk].MaxLimitValue2 = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Ppk].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Vpk].Unit = "V";
            this._msrtResult[(int)ELaserMsrtType.Vpk].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Vpk].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.Vpk].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Vpk].MaxLimitValue2 = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.Vpk].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Impk].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Impk].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Impk].MaxLimitValue = 100.0d;
            this._msrtResult[(int)ELaserMsrtType.Impk].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Impk].MaxLimitValue2 = 100.0d;
            this._msrtResult[(int)ELaserMsrtType.Impk].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Pcepk].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.Pcepk].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.Pcepk].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Pcepk].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Pcepk].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Pcepk].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.Pcepk].Type = EGainOffsetType.None;

            this._msrtResult[(int)ELaserMsrtType.Pth].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.Pth].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Pth].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Pth].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Pth].MaxLimitValue2 = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Pth].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.Pth].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.Pth].Type = EGainOffsetType.Gain;

            this._msrtResult[(int)ELaserMsrtType.Ith].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Ith].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Ith].MaxLimitValue = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Ith].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Ith].MaxLimitValue2 = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Ith].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.Ith].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.Ith].Type = EGainOffsetType.Offset;


            this._msrtResult[(int)ELaserMsrtType.Vth].Unit = "V";
            this._msrtResult[(int)ELaserMsrtType.Vth].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Vth].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.Vth].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Vth].MaxLimitValue2 = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.Vth].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.Vth].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.Vth].Type = EGainOffsetType.Offset;

            this._msrtResult[(int)ELaserMsrtType.SE].Unit = "W/A";
            this._msrtResult[(int)ELaserMsrtType.SE].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.SE].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELaserMsrtType.SE].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.SE].MaxLimitValue2 = 1.0d;
            this._msrtResult[(int)ELaserMsrtType.SE].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.SE].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.SE].Type = EGainOffsetType.Gain;

            this._msrtResult[(int)ELaserMsrtType.SE2].Unit = "W/A";
            this._msrtResult[(int)ELaserMsrtType.SE2].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.SE2].MaxLimitValue = 1.0d;
            this._msrtResult[(int)ELaserMsrtType.SE2].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.SE2].MaxLimitValue2 = 1.0d;
            this._msrtResult[(int)ELaserMsrtType.SE2].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.SE2].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.SE2].Type = EGainOffsetType.Gain;

            this._msrtResult[(int)ELaserMsrtType.RS].Unit = "ohm";
            this._msrtResult[(int)ELaserMsrtType.RS].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.RS].MaxLimitValue = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RS].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.RS].MaxLimitValue2 = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RS].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.RS].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.RS].Type = EGainOffsetType.Offset;

            this._msrtResult[(int)ELaserMsrtType.Kink].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.Kink].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.Kink].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Kink].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Kink].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Kink].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Ikink].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Ikink].Formate = "0.0000";
            this._msrtResult[(int)ELaserMsrtType.Ikink].MaxLimitValue = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Ikink].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Ikink].MaxLimitValue2 = 1500.0d;
            this._msrtResult[(int)ELaserMsrtType.Ikink].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Pkink].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.Pkink].Formate = "0.0000";
            this._msrtResult[(int)ELaserMsrtType.Pkink].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Pkink].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Pkink].MaxLimitValue2 = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Pkink].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Linearity].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.Linearity].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.Linearity].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Linearity].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Linearity].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Linearity].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Linearity2].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.Linearity2].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.Linearity2].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Linearity2].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Linearity2].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Linearity2].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Rollover].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.Rollover].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.Rollover].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Rollover].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Rollover].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.Rollover].MinLimitValue2 = 0.0d;

            this._msrtResult[(int)ELaserMsrtType.Icod].Unit = "mA";
            this._msrtResult[(int)ELaserMsrtType.Icod].Formate = "0.0000";
            this._msrtResult[(int)ELaserMsrtType.Icod].MaxLimitValue = 10000.0d;
            this._msrtResult[(int)ELaserMsrtType.Icod].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Icod].MaxLimitValue2 = 10000.0d;
            this._msrtResult[(int)ELaserMsrtType.Icod].MinLimitValue2 = 0.0d;

            // Pcod
            this._msrtResult[(int)ELaserMsrtType.Pcod].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.Pcod].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.Pcod].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.Pcod].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.Pcod].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)ELaserMsrtType.Pcod].MinLimitValue2 = 0.0d;

            // at point "a"
            this._msrtResult[(int)ELaserMsrtType.PfA].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.PfA].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.PfA].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.PfA].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.PfA].MaxLimitValue2 = 999.999d;
            this._msrtResult[(int)ELaserMsrtType.PfA].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.PfA].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.PfA].Type = EGainOffsetType.Gain;

            this._msrtResult[(int)ELaserMsrtType.VfA].Unit = "V";
            this._msrtResult[(int)ELaserMsrtType.VfA].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.VfA].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.VfA].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.VfA].MaxLimitValue2 = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.VfA].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.VfA].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.VfA].Type = EGainOffsetType.Offset;

            this._msrtResult[(int)ELaserMsrtType.PceA].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.PceA].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.PceA].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.PceA].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.PceA].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.PceA].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.PceA].Type = EGainOffsetType.None;

            this._msrtResult[(int)ELaserMsrtType.RdA].Unit = "ohm";
            this._msrtResult[(int)ELaserMsrtType.RdA].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.RdA].MaxLimitValue = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RdA].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.RdA].MaxLimitValue2 = 99.999d;
            this._msrtResult[(int)ELaserMsrtType.RdA].MinLimitValue2 = 0.0d;

            // at point "b"
            this._msrtResult[(int)ELaserMsrtType.PfB].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.PfB].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.PfB].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.PfB].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.PfB].MaxLimitValue2 = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.PfB].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.PfB].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.PfB].Type = EGainOffsetType.Gain;

            this._msrtResult[(int)ELaserMsrtType.VfB].Unit = "V";
            this._msrtResult[(int)ELaserMsrtType.VfB].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.VfB].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.VfB].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.VfB].MaxLimitValue2 = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.VfB].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.VfB].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.VfB].Type = EGainOffsetType.Offset;

            this._msrtResult[(int)ELaserMsrtType.PceB].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.PceB].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.PceB].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.PceB].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.PceB].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.PceB].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.PceB].Type = EGainOffsetType.None;

            this._msrtResult[(int)ELaserMsrtType.RdB].Unit = "ohm";
            this._msrtResult[(int)ELaserMsrtType.RdB].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.RdB].MaxLimitValue = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RdB].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.RdB].MaxLimitValue2 = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RdB].MinLimitValue2 = 0.0d;

            // at point "c"
            this._msrtResult[(int)ELaserMsrtType.PfC].Unit = "mW";
            this._msrtResult[(int)ELaserMsrtType.PfC].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.PfC].MaxLimitValue = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.PfC].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.PfC].MaxLimitValue2 = 999.99d;
            this._msrtResult[(int)ELaserMsrtType.PfC].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.PfC].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.PfC].Type = EGainOffsetType.Gain;

            this._msrtResult[(int)ELaserMsrtType.VfC].Unit = "V";
            this._msrtResult[(int)ELaserMsrtType.VfC].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.VfC].MaxLimitValue = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.VfC].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.VfC].MaxLimitValue2 = 10.0d;
            this._msrtResult[(int)ELaserMsrtType.VfC].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.VfC].IsEnable = true;
            this._gainOffsetSetting[(int)ELaserMsrtType.VfC].Type = EGainOffsetType.Offset;

            this._msrtResult[(int)ELaserMsrtType.PceC].Unit = "%";
            this._msrtResult[(int)ELaserMsrtType.PceC].Formate = "0.00";
            this._msrtResult[(int)ELaserMsrtType.PceC].MaxLimitValue = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.PceC].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.PceC].MaxLimitValue2 = 100.00d;
            this._msrtResult[(int)ELaserMsrtType.PceC].MinLimitValue2 = 0.0d;
            this._gainOffsetSetting[(int)ELaserMsrtType.PceC].Type = EGainOffsetType.None;

            this._msrtResult[(int)ELaserMsrtType.RdC].Unit = "ohm";
            this._msrtResult[(int)ELaserMsrtType.RdC].Formate = "0.000";
            this._msrtResult[(int)ELaserMsrtType.RdC].MaxLimitValue = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RdC].MinLimitValue = 0.0d;
            this._msrtResult[(int)ELaserMsrtType.RdC].MaxLimitValue2 = 99.99d;
            this._msrtResult[(int)ELaserMsrtType.RdC].MinLimitValue2 = 0.0d;
        }

		protected override void ResetKeyName()
        {
            base.ResetKeyName();

            int num = this._subItemIndex + 1;     // 0-base

            this.SetKeyName(num);
        }

        #endregion
    }
}