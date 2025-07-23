using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Terminal : IdentifiedObject
    {

        private long conductingEquipment;

        public Terminal(long globalId) : base (globalId) { }

        public long ConductingEquipment
        {
            get { return conductingEquipment; }
            set { conductingEquipment = value; }
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
                Terminal ter = (Terminal)x;
                return ter.ConductingEquipment == this.ConductingEquipment;
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
                case ModelCode.TERM_CONDUCTINGEQUIPMENT:
                    return true;
                default: return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.TERM_CONDUCTINGEQUIPMENT: prop.SetValue(conductingEquipment); break;
                default: base.GetProperty(prop); break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TERM_CONDUCTINGEQUIPMENT:
                    ConductingEquipment = property.AsLong();
                    break;

                default: base.SetProperty(property); break;
            }
        }

        #endregion IAccess implementation


        #region IReference implementation	


        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (ConductingEquipment != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TERM_CONDUCTINGEQUIPMENT] = new List<long>();
                references[ModelCode.TERM_CONDUCTINGEQUIPMENT].Add(ConductingEquipment);
            }

            base.GetReferences(references, refType);
        }


        #endregion IReference implementation
    }
}
