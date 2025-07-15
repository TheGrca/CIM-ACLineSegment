using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ACLineSegment : Conductor
    {
        private float b0ch;
        private float bch;
        private float g0ch;
        private float gch;
        private float r;
        private float r0;
        private float x;
        private float x0;
        private long perLengthImpedance;

        public ACLineSegment(long globalId) : base(globalId) { }

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
        public long PerLengthImpedance
        {
            get { return perLengthImpedance; }
            set { perLengthImpedance = value; }
        }

        public override bool Equals(object x)
        {
            if (Object.ReferenceEquals(x, null))
            {
                return false;
            }
            else
            {
                ACLineSegment acls = (ACLineSegment)x;
                return ((acls.B0ch == this.B0ch) && (acls.Bch == this.Bch) && (acls.G0ch == this.G0ch) && (acls.Gch == this.Gch) && (acls.R == this.R) && (acls.R0 == this.R0) && (acls.X == this.X) && (acls.X0 == this.X0));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation		

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ACLS_B0CH:
                case ModelCode.ACLS_BCH:
                case ModelCode.ACLS_G0CH:
                case ModelCode.ACLS_GCH:
                case ModelCode.ACLS_R:
                case ModelCode.ACLS_R0:
                case ModelCode.ACLS_X:
                case ModelCode.ACLS_X0:
                case ModelCode.ACLS_PERLENGTHIMPEDANCE:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLS_B0CH:
                    property.SetValue(B0ch);
                    break;

                case ModelCode.ACLS_BCH:
                    property.SetValue(Bch);
                    break;

                case ModelCode.ACLS_G0CH:
                    property.SetValue(G0ch);
                    break;

                case ModelCode.ACLS_GCH:
                    property.SetValue(Gch);
                    break;

                case ModelCode.ACLS_R:
                    property.SetValue(R);
                    break;

                case ModelCode.ACLS_R0:
                    property.SetValue(R0);
                    break;

                case ModelCode.ACLS_X:
                    property.SetValue(X);
                    break;

                case ModelCode.ACLS_X0:
                    property.SetValue(X0);
                    break;

                case ModelCode.ACLS_PERLENGTHIMPEDANCE:
                    property.SetValue(perLengthImpedance);
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
                case ModelCode.ACLS_B0CH:
                    B0ch = property.AsFloat();
                    break;

                case ModelCode.ACLS_BCH:
                    Bch = property.AsFloat();
                    break;

                case ModelCode.ACLS_G0CH:
                    G0ch = property.AsFloat();
                    break;

                case ModelCode.ACLS_GCH:
                    Gch = property.AsFloat();
                    break;

                case ModelCode.ACLS_R:
                    R = property.AsFloat();
                    break;

                case ModelCode.ACLS_R0:
                    R0 = property.AsFloat();
                    break;

                case ModelCode.ACLS_X:
                    X = property.AsFloat();
                    break;

                case ModelCode.ACLS_X0:
                    X0 = property.AsFloat();
                    break;
                case ModelCode.ACLS_PERLENGTHIMPEDANCE:
                    PerLengthImpedance = property.AsLong();
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
            if (PerLengthImpedance != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.ACLS_PERLENGTHIMPEDANCE] = new List<long>();
                references[ModelCode.ACLS_PERLENGTHIMPEDANCE].Add(PerLengthImpedance);
            }


            base.GetReferences(references, refType);
        }


        #endregion IReference implementation

    }
}
