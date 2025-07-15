using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PerLengthSequenceImpedance : PerLengthImpedance
    {
        private float b0ch;
        private float bch;
        private float g0ch;
        private float gch;
        private float r;
        private float r0;
        private float x;
        private float x0;

        public PerLengthSequenceImpedance(long globalId) : base(globalId) { }

        public float B0ch
        {
            get { return b0ch; }
            set { b0ch = value; }
        }
        public float Bch
        {
            get { return bch; }
            set { bch = value; }
        }
        public float G0ch
        {
            get { return g0ch; }
            set { g0ch = value; }
        }
        public float Gch
        {
            get { return gch; }
            set { gch = value; }
        }
        public float R
        {
            get { return r; }
            set { r = value; }
        }
        public float R0
        {
            get { return r0; }
            set { r0 = value; }
        }
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float X0
        {
            get { return x0; }
            set { x0 = value; }
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
                PerLengthSequenceImpedance plsi = (PerLengthSequenceImpedance)x;
                return ((plsi.B0ch == this.B0ch) && (plsi.Bch == this.Bch) && (plsi.G0ch == this.G0ch) && (plsi.Gch == this.Gch) && (plsi.R == this.R) && (plsi.R0 == this.R0) && (plsi.X == this.X) && (plsi.X0 == this.X0));
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
                case ModelCode.PLSI_B0CH:
                case ModelCode.PLSI_BCH:
                case ModelCode.PLSI_G0CH:
                case ModelCode.PLSI_GCH:
                case ModelCode.PLSI_R:
                case ModelCode.PLSI_R0:
                case ModelCode.PLSI_X:
                case ModelCode.PLSI_X0:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PLSI_B0CH:
                    property.SetValue(B0ch);
                    break;

                case ModelCode.PLSI_BCH:
                    property.SetValue(Bch);
                    break;

                case ModelCode.PLSI_G0CH:
                    property.SetValue(G0ch);
                    break;

                case ModelCode.PLSI_GCH:
                    property.SetValue(Gch);
                    break;

                case ModelCode.PLSI_R:
                    property.SetValue(R);
                    break;

                case ModelCode.PLSI_R0:
                    property.SetValue(R0);
                    break;

                case ModelCode.PLSI_X:
                    property.SetValue(X);
                    break;

                case ModelCode.PLSI_X0:
                    property.SetValue(X0);
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
                case ModelCode.PLSI_B0CH:
                    B0ch = property.AsFloat();
                    break;

                case ModelCode.PLSI_BCH:
                    Bch = property.AsFloat();
                    break;

                case ModelCode.PLSI_G0CH:
                    G0ch = property.AsFloat();
                    break;

                case ModelCode.PLSI_GCH:
                    Gch = property.AsFloat();
                    break;

                case ModelCode.PLSI_R:
                    R = property.AsFloat();
                    break;

                case ModelCode.PLSI_R0:
                    R0 = property.AsFloat();
                    break;

                case ModelCode.PLSI_X:
                    X = property.AsFloat();
                    break;

                case ModelCode.PLSI_X0:
                    X0 = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
    }
}
