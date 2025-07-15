using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PerLengthImpedance : IdentifiedObject
    {
        private List<long> acLineSegments = new List<long>();
        public PerLengthImpedance(long globalId) : base(globalId) { }

        public List<long> ACLineSegments
        {
            get { return acLineSegments; }
            set { acLineSegments = value; }

        }


        #region Overrides

        public override bool Equals(object x)
        {
            return base.Equals(x);
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
                case ModelCode.PLI_ACLINESEGMENTS:
                    return true;

                default: return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PLI_ACLINESEGMENTS: property.SetValue(acLineSegments); break;
                default:
                    base.GetProperty(property); break;
            }
        }

        public override void SetProperty(Property property)
        {

            base.SetProperty(property);

        }

        #endregion IAccess implementation

        #region IReference implementation	

        public override bool IsReferenced
        {
            get
            {
                return ACLineSegments.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (ACLineSegments != null && ACLineSegments.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PLI_ACLINESEGMENTS] = ACLineSegments.GetRange(0, ACLineSegments.Count);
            }
            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ACLS_PERLENGTHIMPEDANCE:
                    ACLineSegments.Add(globalId);
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
                case ModelCode.ACLS_PERLENGTHIMPEDANCE:
                    if (ACLineSegments.Contains(globalId))
                    {
                        ACLineSegments.Remove(globalId);
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
