using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.TestKernel
{
    public class KernelSequenceManagement
    {     
        private ETesterSequenceType _seqtype;

        private List<SequenceAssignment> _seqAssignmt;

        private List<SourceMeterAssignmentData> _smuAssignmt;
        //private List<SwitchAssignmentData> _switchAssignmt;

        private uint _maxSwitchChannelCount;
        private uint _maxSrcChannelCount;

        public KernelSequenceManagement(ETesterSequenceType type)
        {
            this._seqtype = type;

            this._seqAssignmt = new List<SequenceAssignment>();

            this._smuAssignmt = new List<SourceMeterAssignmentData>();

            //this._switchAssignmt = new List<SwitchAssignmentData>();
        }

        #region >>> Public Proberty <<<

        public List<SequenceAssignment> Table
        {
            get { return this._seqAssignmt; }
        }

        public List<SourceMeterAssignmentData> SrcAssignment
        {
            get { return this._smuAssignmt; }
        }

        //public List<SwitchAssignmentData> SwitchAssignment
        //{
        //    get { return this._switchAssignmt; }
        //}

        public uint TotalGroupCount
        {
            get
            {
                if (this._seqAssignmt == null)
                {
                    return 0;
                }

                return (uint)this._seqAssignmt.Count;
            }
        }

        public uint MaxSwitchChannelCount
        {
            get { return this._maxSwitchChannelCount; }
        }

        public uint MaxSrcChannelCount
        {
            get { return this._maxSrcChannelCount; }
        }

        #endregion

        #region >>> Public Mehtod <<<

        public bool Set(List<ChannelAssignmentData> channelAssignment)
        {
            bool isActiveSwitch = false;
            
            switch (this._seqtype)
            {
                case ETesterSequenceType.Parallel:
                case ETesterSequenceType.Series:
                    {
                        int group = 0;

                        foreach (var assignmt in channelAssignment)
                        {
                            SequenceAssignment newOgj = new SequenceAssignment();
                        
                            newOgj.SwitchSlot = 0;

                            newOgj.SrcModel = assignmt.SourceModel;
                            newOgj.SrcSMU = assignmt.SourceCH;
                            newOgj.SrcConnectPort = assignmt.DeviceIpAddress;
                            newOgj.SrcSerialNum = assignmt.DeviceSerialNum;
                            newOgj.DutChannel.Add((uint)group);

                            this._seqAssignmt.Add(newOgj);
                            
                            group++;
                        }
                        
                        break;
                    }
                //case ETesterSequenceType.Mix:
                //    {
                //        isActiveSwitch = true;
                        
                //        int group = -1;
                //        int count = 0;

                //        foreach (var assignmt in channelAssignment)
                //        {
                //            bool isContainSlot = false;

                //            foreach (var checkData in this._seqAssignmt)
                //            {
                //                if (assignmt.SwitchSlot == checkData.SwitchSlot)
                //                {
                //                    isContainSlot = true;
                                
                //                    break;
                //                }
                //            }

                //            if (isContainSlot)
                //            {
                //                this._seqAssignmt[group].SwitchChannel.Add(assignmt.SwtichChannel);

                //                this._seqAssignmt[group].DutChannel.Add((uint)count);
                //            }
                //            else
                //            {
                //                group++;
                                
                //                SequenceAssignment newOgj = new SequenceAssignment();
                               
                //                newOgj.SwitchModel = assignmt.SwitchModel;
                //                newOgj.SwitchSlot = (uint)assignmt.SwitchSlot;
                //                newOgj.SwitchChannel.Add(assignmt.SwtichChannel);

                //                newOgj.SrcModel = assignmt.SourceModel;
                //                newOgj.SrcSMU = assignmt.SourceCH;
                //                newOgj.SrcConnectPort = assignmt.DeviceIpAddress;
                //                newOgj.SrcSerialNum = assignmt.DeviceSerialNum;
                //                newOgj.SrcChannel = (uint)group;

                //                newOgj.DutChannel.Add((uint)count);

                //                this._seqAssignmt.Add(newOgj);
                //            }

                //            count++;
                //        }

                //        break;
                //    }
                default:
                    return false;
            }
            
            
            //-----------------------------------------------------------------------------
            // Set SourceMeterAssignmrntData

            this._smuAssignmt.Clear();

            uint smuCount = 0;

            foreach (var data in this._seqAssignmt)
            {
                SourceMeterAssignmentData newObj = new SourceMeterAssignmentData();
                
                newObj.Model = data.SrcModel;
                newObj.SMU = data.SrcSMU;
                newObj.ConnectionPort = data.SrcConnectPort;
                newObj.SerialNumber = data.SrcSerialNum;
                newObj.Channel = data.SrcChannel;

                newObj.RelateSwitchSlot = data.SwitchSlot;

                this._smuAssignmt.Add(newObj);

                smuCount++;
            }

            this._maxSrcChannelCount = smuCount;

            //-----------------------------------------------------------------------------
            // Set SwitchAssignmrntData

            //if (isActiveSwitch)
            //{
            //    foreach (var data in this._seqAssignmt)
            //    {
            //        SwitchAssignmentData newObj = new SwitchAssignmentData();
            //        newObj.Model = data.SwitchModel;
            //        newObj.Slot = (uint)data.SwitchSlot;

            //        newObj.RelateSrcChannel = data.SrcChannel;

            //        foreach (var ch in data.SwitchChannel)
            //        {
            //            newObj.Channel.Add(ch);
            //        }
 
            //        this._switchAssignmt.Add(newObj);

            //        //---------------------------------------------------------------
            //        int switchCountPerSlot = newObj.Channel.Count;

            //        if (switchCountPerSlot > this._maxSwitchChannelCount)
            //        {
            //            this._maxSwitchChannelCount = (uint)switchCountPerSlot;
            //        }
            //    }
            //}

            return true;
        }

        #endregion
    }

    public class SequenceAssignment
    {
        public uint SwitchSlot;
        
        public uint SrcChannel;

        public string SrcModel;

        public string SrcSMU;

        public string SrcConnectPort;

        public string SrcSerialNum;

        public string SwitchModel;

        public string SwitchConnectPort;

        public List<uint> SwitchChannel = new List<uint>();

        public List<uint> DutChannel = new List<uint>();

        public SequenceAssignment()
        {
 
        }

    }
}
