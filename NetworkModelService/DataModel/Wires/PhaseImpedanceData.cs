using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{

    //Zavrsiti
    public class PhaseImpedanceData : IdentifiedObject
    {
        private float b;
        private float r;
        private int sequenceNumber;
        private float x;
        public PhaseImpedanceData(long globalId) : base(globalId) { }

        public float B
        {
            get { return b; }
            set { b = value; }
        }
        public float R
        {
            get { return r; }
            set { r = value; }
        }
        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        #region Overrides

        public override bool Equals(object x)
        {
            if (Object.ReferenceEquals(x, null))
            {
                return false;
            }
            else
            {
                PhaseImpedanceData pid = (PhaseImpedanceData)x;
                return ((pid.B == this.B) && (pid.R == this.R) && (pid.SequenceNumber == this.SequenceNumber) && (pid.X == this.X));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IAccess implementation		
        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.PID_B:
                case ModelCode.PID_R:
                case ModelCode.PID_SEQUENCENUMBER:
                case ModelCode.PID_X:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {

                case ModelCode.PID_B:
                    property.SetValue(B);
                    break;

                case ModelCode.PID_R:
                    property.SetValue(R);
                    break;

                case ModelCode.PID_SEQUENCENUMBER:
                    property.SetValue(SequenceNumber);
                    break;

                case ModelCode.PID_X:
                    property.SetValue(X);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PID_B:
                    B = property.AsFloat();
                    break;

                case ModelCode.PID_R:
                    R = property.AsFloat();
                    break;

                case ModelCode.PID_SEQUENCENUMBER:
                    SequenceNumber = property.AsInt();
                    break;

                case ModelCode.PID_X:
                    X = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

    }
}
