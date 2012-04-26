using System;
using System.Collections.Generic;

namespace Volta.Core.Domain.Batches
{
    public class Batch
    {
        public Guid Id { get; set; }

        public string BatchId { get; set; } // 20110612-DB

        public Phase ComponentPhase { get; set; }
        public Phase AssemblyPhase { get; set; }
        public Phase OperationPhase { get; set; }

        public List<Cell> Cells { get; set; }

        public string GloveBox { get; set; }  // Thor, Hobgoblin
        public double OperatingTemperature { get; set; } // C
        public double CyclingCurrent { get; set; } // A

        public Guid ScheduleId { get; set; }
        public string ScheduleName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }

    public class Phase
    {
        public string Operator { get; set; } // DJB
        public string SOP { get; set; } // 110.02, 111.02, 112.02
    }

    public class Cell
    {
        public string Chemistry { get; set; } // LiSbPb, LiSb, NaBi
        public double NominalCapacity { get; set; } // Ah, 1Ah, 20, Ah, 200Ah
        public double InnerDiameter { get; set; } // mm
        public double PretreatmentMass { get; set; } // g

        public double DepthToPOS { get; set; } // mm
        public double NCCTipToPOS { get; set; } // mm

        public string PCC { get; set; } // Positive current collector: w-disk, steel crucible
        public string NCC { get; set; } // Negative current collector: spear, foam (pretreated, 1x15mm/2/3x12mm layers), ribbon
        public double NCCLength { get; set; } // Negative current collector: mm

        public Electrode POS { get; set; } // Positive electrode

        public Sheath Sheath { get; set; }
        public Electrolyte Electrolyte { get; set; }
        public Crucible Crucible { get; set; }
    }

    public class Crucible
    {
        public string BatchId { get; set; } // 20110612-DB
        public string Vender { get; set; }
        public double InnerHeight { get; set; } // mm
    }

    public class Sheath
    {
        public string BatchId { get; set; } // 20110612-DB
        public string Material { get; set; } // BN, MgO, Y2O3, none
        public double InnerDiameter { get; set; } // mm
        public double WallThickness { get; set; } // mm
        public string Vendor { get; set; } // Saint-Gobain
        public string PreparationMethod { get; set; } // sonicated, sonicated then rinsed with DI
    }

    public class Electrode
    {
        public double Length { get; set; } // mm
        public List<Element> Elements { get; set; } // Li, Sb, Pb
    }

    public class Electrolyte
    {
        public string BatchId { get; set; } // 20110612-DB
        public string Components { get; set; } // LiF:LiCl:LiI
        public string MolarComposition { get; set; } // 20:50:30 mol%
        public double Length { get; set; } // mm
        public double Density { get; set; } // g/cm3
        public double Mass { get; set; } // g
    }

    public class Element
    {
        public string Name { get; set; }
        public double Mass { get; set; } // g
        public string Vender { get; set; }
        public string StockNumber { get; set; }
    }
}
