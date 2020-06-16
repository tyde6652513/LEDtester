using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
	public partial class frmItemContactCheck : Form, IConditionUICtrl, IConditionElecCtrl
	{
		private TestItemData _item;
        private bool _isEnableSwitchChannel;

		public frmItemContactCheck()
		{
			InitializeComponent();

			this._item = new ContactCheckTestItem();

			this.cmbSpeed.Items.AddRange(Enum.GetNames(typeof(EContactCheckSpeed)));

			this.cmbSpeed.SelectedIndex = 0;

            this._isEnableSwitchChannel = false;
		}

		public frmItemContactCheck(TestItemDescription description)
			: this()
        {
        }

        #region >>> Public Property <<<

        public bool IsAutoSelectForceRange
		{
			get;
			set;
		}

		public bool IsAutoSelectMsrtRange
		{
			get;
			set;
		}

		public bool IsVisibleNPLC
		{
			get;
			set;
		}

		public bool IsVisibleFilterCount
		{
			get;
			set;
		}

        public bool IsEnableSwitchChannel
        {
            get; 
            set;
        }

        public bool IsEnableMsrtForceValue
        {
            get; 
            set;
        }

        public uint MaxSwitchingChannelCount
        {
            get;
            set;
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
		{
		}

		public bool CheckUI(out string msg)
		{
			msg = "";

			return true;
		}

		public void UpdateCondtionDataToComponent(TestItemData data)
		{
			this._item = (data as ContactCheckTestItem).Clone() as ContactCheckTestItem;

			this.cmbSpeed.SelectedItem = this._item.ElecSetting[0].ConatctCheckSpeed.ToString();
		}

		public TestItemData GetConditionDataFromComponent()
		{
			this._item.ElecSetting[0].ConatctCheckSpeed = (EContactCheckSpeed)Enum.Parse(typeof(EContactCheckSpeed), this.cmbSpeed.SelectedItem.ToString());

			return this._item;
        }

        #endregion
    }
}
