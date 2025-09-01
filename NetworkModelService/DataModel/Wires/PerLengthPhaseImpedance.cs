using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
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
        private List<long> phaseImpedanceDatas = new List<long>();
        public PerLengthPhaseImpedance(long globalId) : base(globalId) { }

        public int ConductorCount
        {
            get { return conductorCount; }
            set { conductorCount = value; }
        }
        public List<long> PhaseImpedanceDatas
        {
            get { return phaseImpedanceDatas; }
            set { phaseImpedanceDatas = value; }
        }

        #region Overrides
        //Provjeri
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
                case ModelCode.PLPI_PHASEIMPEDANCEDATAS:
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
                case ModelCode.PLPI_PHASEIMPEDANCEDATAS:
                    property.SetValue(PhaseImpedanceDatas);
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
        #region IReference implementation	
        public override bool IsReferenced
        {
            get
            {
                return PhaseImpedanceDatas.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (PhaseImpedanceDatas != null && PhaseImpedanceDatas.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PLPI_PHASEIMPEDANCEDATAS] = PhaseImpedanceDatas.GetRange(0, PhaseImpedanceDatas.Count);
            }
            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.PID_PHASEIMPEDANCE:
                    PhaseImpedanceDatas.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.PID_PHASEIMPEDANCE:
                    if (PhaseImpedanceDatas.Contains(globalId))
                    {
                        PhaseImpedanceDatas.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity doesn't contain reference");
                    }
                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

            #endregion IReference implementation
    }
}
