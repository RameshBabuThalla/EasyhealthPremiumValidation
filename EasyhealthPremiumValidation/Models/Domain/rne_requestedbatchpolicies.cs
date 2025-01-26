using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERGPremiumValidationSchedular.Models.Domain
{
    internal class rne_requestedbatchpolicies
    {
        public string? sourcesystem { get; set; }
        public string? jobid { get; set; }
        public string? batchid { get; set; }
        public decimal? product { get; set; }
        public decimal? referencenum { get; set; }
        public string? policynumber { get; set; }
        public string? polstatus { get; set; }
        public DateTime? requestedon { get; set; }
        public DateTime? plannedon { get; set; }
        public short requeststatus { get; set; }
        public string? errortype { get; set; }
        public string? processmesg { get; set; }
        public short isrectification { get; set; }
        public string? rectificationtypes { get; set; }
        public DateTime? processedon { get; set; }
        public string? processstarttime { get; set; }
        public string? processendtime { get; set; }
        public string? threadid { get; set; }
        public string? qcbatchid { get; set; }
        public short qcstatus { get; set; }
        public string? qcmesg { get; set; }
        public DateTime? qcon { get; set; }
        public string? qcby { get; set; }
        public short ispreintreq { get; set; }
        public DateTime? printreqon { get; set; }
        public short printstatus { get; set; }
        public string? printmesg { get; set; }
        public DateTime? printon { get; set; }
        public short isarchived { get; set; }
        public DateTime? archivedon { get; set; }
        public string? vendorpolicyno { get; set; }

    }
}
