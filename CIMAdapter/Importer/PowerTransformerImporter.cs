using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            ///

            ImportPerLengthPhaseImpendance();
            ImportPhaseImpedanceData();
            ImportPerLengthSequenceImpedance();
            ImportACLineSegment();
            ImportDCLineSegment();
			ImportSeriesCompensator();
			ImportTerminal();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import


		//SAMO ZA DMS TYPES


        private void ImportPerLengthPhaseImpendance()
		{
            SortedDictionary<string, object> cimPerLengthPhaseImpedances = concreteModel.GetAllObjectsOfType("FTN.PerLengthPhaseImpedance");
            if (cimPerLengthPhaseImpedances != null)
            {
                foreach (KeyValuePair<string, object> cimPerLengthPhaseImpedancePair in cimPerLengthPhaseImpedances)
                {
                    FTN.PerLengthPhaseImpedance cimPerLengthPhaseImpedance = cimPerLengthPhaseImpedancePair.Value as FTN.PerLengthPhaseImpedance;

                    ResourceDescription rd = CreatePerLengthPhaseImpendanceResourceDescription(cimPerLengthPhaseImpedance);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("PerLengthPhaseImpedance ID = ").Append(cimPerLengthPhaseImpedance.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("PerLengthPhaseImpedance ID = ").Append(cimPerLengthPhaseImpedance.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreatePerLengthPhaseImpendanceResourceDescription(FTN.PerLengthPhaseImpedance cimPerLengthPhaseImpedance)
        {
            ResourceDescription rd = null;
            if (cimPerLengthPhaseImpedance != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PLPI, importHelper.CheckOutIndexForDMSType(DMSType.PLPI));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimPerLengthPhaseImpedance.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulatePerLengthPhaseImpedanceProperties(cimPerLengthPhaseImpedance, rd);
            }
            return rd;
        }
        private void ImportPhaseImpedanceData()
        {
            SortedDictionary<string, object> cimPhaseImpedanceDatas = concreteModel.GetAllObjectsOfType("FTN.PhaseImpedanceData");
            if (cimPhaseImpedanceDatas != null)
            {
                foreach (KeyValuePair<string, object> cimPhaseImpedanceDataPair in cimPhaseImpedanceDatas)
                {
                    FTN.PhaseImpedanceData cimPhaseImpedanceData = cimPhaseImpedanceDataPair.Value as FTN.PhaseImpedanceData;

                    ResourceDescription rd = CreatePhaseImpedanceDataResourceDescription(cimPhaseImpedanceData);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("PhaseImpedanceData ID = ").Append(cimPhaseImpedanceData.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("PhaseImpedanceData ID = ").Append(cimPhaseImpedanceData.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreatePhaseImpedanceDataResourceDescription(FTN.PhaseImpedanceData cimPhaseImpedanceData)
        {
            ResourceDescription rd = null;
            if (cimPhaseImpedanceData != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PID, importHelper.CheckOutIndexForDMSType(DMSType.PID));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimPhaseImpedanceData.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulatePhaseImpedanceDataProperties(cimPhaseImpedanceData, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportACLineSegment()
        {
            SortedDictionary<string, object> cimACLineSegments = concreteModel.GetAllObjectsOfType("FTN.ACLineSegment");
            if (cimACLineSegments != null)
            {
                foreach (KeyValuePair<string, object> cimACLineSegmentsPair in cimACLineSegments)
                {
                    FTN.ACLineSegment cimACLineSegment = cimACLineSegmentsPair.Value as FTN.ACLineSegment;

                    ResourceDescription rd = CreateACLineSegmentResourceDescription(cimACLineSegment);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("ACLineSegment ID = ").Append(cimACLineSegment.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("ACLineSegment ID = ").Append(cimACLineSegment.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateACLineSegmentResourceDescription(FTN.ACLineSegment cimACLineSegment)
        {
            ResourceDescription rd = null;
            if (cimACLineSegment != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ACLS, importHelper.CheckOutIndexForDMSType(DMSType.ACLS));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimACLineSegment.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateACLineSegmentProperties(cimACLineSegment, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportPerLengthSequenceImpedance()
        {
            SortedDictionary<string, object> cimPerLengthSequenceImpedances = concreteModel.GetAllObjectsOfType("FTN.PerLengthSequenceImpedance");
            if (cimPerLengthSequenceImpedances != null)
            {
                foreach (KeyValuePair<string, object> cimPerLengthSequenceImpedancePair in cimPerLengthSequenceImpedances)
                {
                    FTN.PerLengthSequenceImpedance cimPerLengthSequenceImpedance = cimPerLengthSequenceImpedancePair.Value as FTN.PerLengthSequenceImpedance;

                    ResourceDescription rd = CreatePerLengthSequenceImpedanceResourceDescription(cimPerLengthSequenceImpedance);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("PerLengthSequenceImpedance ID = ").Append(cimPerLengthSequenceImpedance.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("PerLengthSequenceImpedance ID = ").Append(cimPerLengthSequenceImpedance.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreatePerLengthSequenceImpedanceResourceDescription(FTN.PerLengthSequenceImpedance cimPerLengthSequenceImpedance)
        {
            ResourceDescription rd = null;
            if (cimPerLengthSequenceImpedance != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PLSI, importHelper.CheckOutIndexForDMSType(DMSType.PLSI));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimPerLengthSequenceImpedance.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulatePerLengthSequenceImpedanceProperties(cimPerLengthSequenceImpedance, rd);
            }
            return rd;
        }


        private void ImportDCLineSegment()
        {
            SortedDictionary<string, object> cimDCLineSegments = concreteModel.GetAllObjectsOfType("FTN.DCLineSegment");
            if (cimDCLineSegments != null)
            {
                foreach (KeyValuePair<string, object> cimDCLineSegmentsPair in cimDCLineSegments)
                {
                    FTN.DCLineSegment cimDCLineSegment = cimDCLineSegmentsPair.Value as FTN.DCLineSegment;

                    ResourceDescription rd = CreateDCLineSegmentResourceDescription(cimDCLineSegment);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("cimDCLineSegment ID = ").Append(cimDCLineSegment.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("cimDCLineSegment ID = ").Append(cimDCLineSegment.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateDCLineSegmentResourceDescription(FTN.DCLineSegment cimDCLineSegment)
        {
            ResourceDescription rd = null;
            if (cimDCLineSegment != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DCLS, importHelper.CheckOutIndexForDMSType(DMSType.DCLS));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimDCLineSegment.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateDCLineSegmentProperties(cimDCLineSegment, rd);
            }
            return rd;
        }

        private void ImportSeriesCompensator()
        {
            SortedDictionary<string, object> cimSeriesCompensators = concreteModel.GetAllObjectsOfType("FTN.SeriesCompensator");
            if (cimSeriesCompensators != null)
            {
                foreach (KeyValuePair<string, object> cimSeriesCompensatorsPair in cimSeriesCompensators)
                {
                    FTN.SeriesCompensator cimSeriesCompensator = cimSeriesCompensatorsPair.Value as FTN.SeriesCompensator;

                    ResourceDescription rd = CreateSeriesCompensatorResourceDescription(cimSeriesCompensator);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("SeriesCompensator ID = ").Append(cimSeriesCompensator.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("SeriesCompensator ID = ").Append(cimSeriesCompensator.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSeriesCompensatorResourceDescription(FTN.SeriesCompensator cimSeriesCompensator)
        {
            ResourceDescription rd = null;
            if (cimSeriesCompensator != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SC, importHelper.CheckOutIndexForDMSType(DMSType.SC));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSeriesCompensator.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateSeriesCompensatorProperties(cimSeriesCompensator, rd);
            }
            return rd;
        }
        private void ImportTerminal()
        {
            SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType("FTN.Terminal");
            if (cimTerminals != null)
            {
                foreach (KeyValuePair<string, object> cimTerminalsPair in cimTerminals)
                {
                    FTN.Terminal cimTerminal = cimTerminalsPair.Value as FTN.Terminal;

                    ResourceDescription rd = CreateTerminalResourceDescription(cimTerminal);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateTerminalResourceDescription(FTN.Terminal cimTerminal)
        {
            ResourceDescription rd = null;
            if (cimTerminal != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERM, importHelper.CheckOutIndexForDMSType(DMSType.TERM));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTerminal.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateTerminalProperties(cimTerminal, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Import
    }
}

