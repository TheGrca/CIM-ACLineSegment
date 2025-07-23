using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{

    public class PhaseImpedanceData : IdentifiedObject
    {
        private float b;
        private float r;
        private int sequenceNumber;
        private float x;
        private long phaseImpedance;
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

        public long PhaseImpedance
        {
                get { return phaseImpedance; }
                set { phaseImpedance = value; }
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
                return ((pid.B == this.B) && (pid.R == this.R) && (pid.SequenceNumber == this.SequenceNumber) && (pid.X == this.X) && (pid.PhaseImpedance == this.PhaseImpedance));
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
                case ModelCode.PID_PHASEIMPEDANCE:
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

                case ModelCode.PID_PHASEIMPEDANCE:
                    property.SetValue(PhaseImpedance);
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

                case ModelCode.PID_PHASEIMPEDANCE:
                    PhaseImpedance = property.AsLong();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation



        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if(PhaseImpedance != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.PID_PHASEIMPEDANCE] = new List<long>();
                references[ModelCode.PID_PHASEIMPEDANCE].Add(PhaseImpedance);
            }
            base.GetReferences(references, refType);
        }

        #endregion

    }
}
