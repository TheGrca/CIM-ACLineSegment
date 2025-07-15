using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{

    //ZAVRSI
    public class PerLengthPhaseImpedance : PerLengthImpedance
    {

        private int conductorCount;

        public PerLengthPhaseImpedance(long globalId) : base(globalId) { }



        public int ConductorCount
        {
            get { return conductorCount; }
            set { conductorCount = value; }
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
                PerLengthPhaseImpedance plpi = (PerLengthPhaseImpedance)x;
                return plpi.ConductorCount == this.ConductorCount;
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
                case ModelCode.PLPI_CONDUCTORCOUNT:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PLPI_CONDUCTORCOUNT:
                    property.SetValue(ConductorCount);
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
                case ModelCode.PLPI_CONDUCTORCOUNT:
                    ConductorCount = property.AsInt();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation
    }
}
