using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class SeriesCompensator : ConductingEquipment
    {
        private float r;
        private float r0;
        private float x;
        private float x0;
        public SeriesCompensator(long globalId) : base(globalId) { }
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
                SeriesCompensator sc = (SeriesCompensator)x;
                return ((sc.R == this.R) && (sc.R0 == this.R0) && (sc.X == this.X) && (sc.X0 == this.X0));
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
                case ModelCode.ACLS_R:
                case ModelCode.ACLS_R0:
                case ModelCode.ACLS_X:
                case ModelCode.ACLS_X0:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {

                case ModelCode.SC_R:
                    property.SetValue(R);
                    break;

                case ModelCode.SC_R0:
                    property.SetValue(R0);
                    break;

                case ModelCode.SC_X:
                    property.SetValue(X);
                    break;

                case ModelCode.SC_X0:
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
                case ModelCode.SC_R:
                    R = property.AsFloat();
                    break;

                case ModelCode.SC_R0:
                    R0 = property.AsFloat();
                    break;

                case ModelCode.SC_X:
                    X = property.AsFloat();
                    break;

                case ModelCode.SC_X0:
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
