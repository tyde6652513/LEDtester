using System;
using System.Collections.Generic;
using System.Text;
using MPI.MCF.Communication.Command;
using MPI.RemoteControl2.Tester.Mpi.Command;
using MPI.RemoteControl2.Tester.Mpi.Command.Base;

namespace MPI.RemoteControl2.Tester
{
    /// <summary>
    /// TesterCommandAgent class.
    /// </summary>
    public class TesterCommandAgent : CommandAgent<MPIDS7600Packet, CmdMPIBased>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TesterCommandAgent()
            : base()
        {
			
        }

        public override CmdMPIBased CommandFactory(MPIDS7600Packet packet)
        {
            switch (packet.Command)
            {
                //---------------------------------------------------------------
                case CmdBarcodeInsert.COMMAND_ID:
                    return new CmdBarcodeInsert();
                //---------------------------------------------------------------
                case CmdWaferIn.COMMAND_ID:
                    return new CmdWaferIn();
                //---------------------------------------------------------------
                case CmdTestItem.COMMAND_ID:
                    return new CmdTestItem();
                //---------------------------------------------------------------
                case CmdWaferScanEnd.COMMAND_ID:
                    return new CmdWaferScanEnd();
                //---------------------------------------------------------------
                case CmdSOT.COMMAND_ID:
					switch (CmdMPIBased.ConstDefinition.ProtocolType)
					{
						case ConstDefinition.EMPIDS7600ConstDefinitionType.T200:
							return new CmdSOT1();
						default:
							return new CmdSOT();
					}
                //---------------------------------------------------------------
                case CmdEOT.COMMAND_ID:
                    return new CmdEOT();
                //---------------------------------------------------------------
                case CmdREOT.COMMAND_ID:
                    return new CmdREOT();
                //---------------------------------------------------------------
                case CmdLotEnd.COMMAND_ID:
                    return new CmdLotEnd();
                //---------------------------------------------------------------
                case CmdLotIn.COMMAND_ID:
                    return new CmdLotIn();
                //---------------------------------------------------------------
                case CmdWaferEnd.COMMAND_ID:
                    return new CmdWaferEnd();
                //---------------------------------------------------------------
                case CmdLotInfo.COMMAND_ID:
                    return new CmdLotInfo();
                //---------------------------------------------------------------
                case CmdError.COMMAND_ID:
                    return new CmdError();
                //---------------------------------------------------------------
                case CmdAutoStatusStart.COMMAND_ID:
                    return new CmdAutoStatusStart();
                //---------------------------------------------------------------
                case CmdAutoStatusStop.COMMAND_ID:
                    return new CmdAutoStatusStop();
                //---------------------------------------------------------------
				case CmdWaferInInfo.COMMAND_ID:
					return new CmdWaferInInfo();
                //---------------------------------------------------------------
                case CmdAutoContact.COMMAND_ID:
                    return new CmdAutoContact();
                //---------------------------------------------------------------
                case CmdAutoCalibrationStart.COMMAND_ID:
                    return new CmdAutoCalibrationStart();
                //---------------------------------------------------------------
                case CmdAutoCalibrationSOT.COMMAND_ID:
                    return new CmdAutoCalibrationSOT();
                //---------------------------------------------------------------
                case CmdAutoCalibrationEOT.COMMAND_ID:
                    return new CmdAutoCalibrationEOT();
                //---------------------------------------------------------------
                case CmdAutoCalibrationEnd.COMMAND_ID:
                    return new CmdAutoCalibrationEnd();
                //---------------------------------------------------------------
                case CmdErrorLotNoSetting.COMMAND_ID:
                    return new CmdErrorLotNoSetting();
                //---------------------------------------------------------------
                case CmdErrorNoBinFile.COMMAND_ID:
                    return new CmdErrorNoBinFile();
                //---------------------------------------------------------------
                case CmdErrorNotEqualItem.COMMAND_ID:
                    return new CmdErrorNotEqualItem();
                //---------------------------------------------------------------
                case CmdErrorNoTestItemFile.COMMAND_ID:
                    return new CmdErrorNoTestItemFile();
                //---------------------------------------------------------------
                case CmdAutoCalibrationFail.COMMAND_ID:
                    return new CmdAutoCalibrationFail();
                //---------------------------------------------------------------
				case CmdOverrideTesterReport.COMMAND_ID:
					return new CmdOverrideTesterReport();
				//---------------------------------------------------------------
				case CmdBinGrade.COMMAND_ID:
                    return new CmdBinGrade();
				//---------------------------------------------------------------
				case CmdQueryInformation.COMMAND_ID:
					return new CmdQueryInformation();
                //---------------------------------------------------------------
                case CmdWaferBegin.COMMAND_ID:
                    return new CmdWaferBegin();
                //---------------------------------------------------------------
                case CmdWaferFinish.COMMAND_ID:
                    return new CmdWaferFinish();
				//---------------------------------------------------------------
				case CmdSOT2.COMMAND_ID:
					return new CmdSOT2();
				//---------------------------------------------------------------
				case CmdEOT2.COMMAND_ID:
					return new CmdEOT2();
				//---------------------------------------------------------------
                case CmdTestAbort.COMMAND_ID:
                    return new CmdTestAbort();
                //---------------------------------------------------------------
				default:
                    return null;
            }
        }

    }
}
