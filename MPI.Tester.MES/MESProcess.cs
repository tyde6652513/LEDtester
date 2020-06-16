using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.MES
{
    public class MESProcess
    {
        public static EErrorCode LoadRecipe(UISetting uiSetting, MachineConfig machineConfig, out string alarmMsg)
        {
            IMES _iMES;

            alarmMsg = string.Empty;

            try
            {
                switch (uiSetting.UserID)
                {
                    case EUserID.EPITOP:
                        _iMES = new MPI.Tester.MES.User.EPITop.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.AquaLite:
                        _iMES = new MPI.Tester.MES.User.AquaLite.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.EnRayTek:
                        _iMES = new MPI.Tester.MES.User.EnRay.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.HCSemiTek:
                        _iMES = new MPI.Tester.MES.User.WuHum.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.EpiStar:
                        _iMES = new MPI.Tester.MES.User.EPIStar.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.Eti:
                        _iMES = new MPI.Tester.MES.User.ETI.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.CanYang:
                        _iMES = new MPI.Tester.MES.User.CanYang.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.ZhonGke:
                        _iMES = new MPI.Tester.MES.User.ZhonGke.MESProcess();
                        break;
                    //-----------------------------------------------------------------
					case EUserID.EverVision:
						_iMES = new MPI.Tester.MES.User.EverVision.MESProcess();
						break;
					//-----------------------------------------------------------------
					case EUserID.LPC00:
						_iMES = new MPI.Tester.MES.User.LPC00.MESProcess();
						break;
					//-----------------------------------------------------------------
					case EUserID.KAISTAR:
						_iMES = new MPI.Tester.MES.User.KAISTAR.MESProcess();
						break;
					//-----------------------------------------------------------------
					case EUserID.Lumitek:
						_iMES = new MPI.Tester.MES.User.Lumitek.MESProcess();
						break;
					//-----------------------------------------------------------------
					case EUserID.DELI:
						_iMES = new MPI.Tester.MES.User.DELI.MESProcess();
						break;
					//-----------------------------------------------------------------
					case EUserID.GPI:
						_iMES = new MPI.Tester.MES.User.GPI.MESProcess();
						break;
					//-----------------------------------------------------------------
					case EUserID.ForEpi:
						_iMES = new MPI.Tester.MES.User.ForEpi.MESProcess();
						break;
					//-----------------------------------------------------------------
                    case EUserID.ChangeLight:
                        _iMES = new MPI.Tester.MES.User.ChangeLight.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.Silan:
                        _iMES = new MPI.Tester.MES.User.Silan.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    case EUserID.Sanan6138:
                        _iMES = new MPI.Tester.MES.User.Sanan6138.MESProcess();
                        break;
                    //-----------------------------------------------------------------
                    default:
                        _iMES = null;
                        return EErrorCode.NONE;
                }

                if (_iMES != null)
                {
                    return _iMES.LoadRecipe(uiSetting, machineConfig, out alarmMsg);
                }
                else
                {
					return EErrorCode.MES_LoadTaskError;
                }
            }
            catch
            {
                Console.WriteLine("[MESProcess],UnDefined ERROR");
                return EErrorCode.MES_LoadTaskError;
            }
        }


    }
}
