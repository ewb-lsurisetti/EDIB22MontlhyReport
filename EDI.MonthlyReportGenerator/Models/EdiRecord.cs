// Ignore Spelling: Fiserv

using EDIMonthlyReportGenerator.Models;
using EDIMonthlyReportGenerator.Record;

namespace EdiMonthlyReportGenerator.Models
{
    public class EDIRecord
    {
        public List<string> Block { get; set; }
        public ISAHeader ISAHeader { get; set; }
        public GSHeader GSHeader { get; set; }
        
        public STSegment StSegment{ get; set; }

        public List<BgnDetails> BgnSegments { get; set; }

        public List<DtmDetails> DtmSegments { get; set; }

        public List<EntDetails> EntSegments { get; set; }

        public List<N1Segment> N1Segments { get; set; }
        public List<N2Details> N2Segments { get; set; }
        public List<N3Details> N3Segments { get; set; }
        public List<N4Details> N4Segments { get; set; }

        //public List<ActDetails> ActSegments { get; set; }

        public List<CurDetails> CurSegments { get; set; }
        public List<RteDetails> RteSegments { get; set; }
        public List<LxDetails> LxSegments { get; set; }
        public List<BlnDetails> BlnSegments { get; set; }
        public List<ServiceDetails> SerSegments { get; set; }
        public IEATrailer IeaTrailer { get; set; }
    }
}
