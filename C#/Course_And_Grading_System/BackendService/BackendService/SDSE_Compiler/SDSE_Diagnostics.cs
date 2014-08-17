using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    class SDSE_Diagnostics
    {
        DateTime startTime;
        DateTime lastRow;

        int atRow = 0;

        SDSE_Diagnostics_Result finishedResult;

        public SDSE_Diagnostics()
        {
            finishedResult = new SDSE_Diagnostics_Result(0);
        }

        public void Start()
        {
            ++finishedResult.ResultVersion;
            atRow = 0;
            startTime = DateTime.Now;
            lastRow = DateTime.Now;
        }

        public void AddNewTime()
        {
            if (atRow < 40)
                finishedResult.RowTimes[atRow++] = (DateTime.Now - lastRow).Milliseconds;

            lastRow = DateTime.Now;
        }

        public void Stop()
        {
            finishedResult.Total = (DateTime.Now - startTime).Milliseconds;
            finishedResult.NumberOfRows = atRow;
        }

        public SDSE_Diagnostics_Result GetResult()
        {
            return finishedResult;
        }
    }

    struct SDSE_Diagnostics_Result
    {
        public int ResultVersion;
        public int Total;
        public int[] RowTimes;
        public int NumberOfRows;

        public SDSE_Diagnostics_Result(int ResultVersion)
        {
            this.ResultVersion = ResultVersion;
            Total = 0;
            RowTimes = new int[40];
            NumberOfRows = 0;
        }
    }
}
